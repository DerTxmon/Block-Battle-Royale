using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class UI_Handler : MonoBehaviour
{
    [SerializeField] GameObject BigMap;
    [SerializeField] GameObject X_Button;
    [SerializeField] GameObject MinimapCam;
    [SerializeField] GameObject Home_Button;
    private GameObject[] AllrenderableGameObjects;
    [SerializeField] private GameObject Point;
    private Menu_Handler Menu;
    private Inventory_Handler Inv;
    public GameObject WinText;
    public Text Kills, Damage, SurvivedTime, Place, CoinsEarned;
    [SerializeField] private GameObject[] HUD;
    private int givecoins;
    public GameObject StatWindow, KillNumber, TimeNumber, DamageNumber, PlaceNumber, MinmapDeathTexture, Stats_BacktohomeButton;
    public List<Image> HudComponents_Img;
    public List<SpriteRenderer> HudComponents_Sprites;
    public List<Text> HudComponents_Text;
    private float x;
    public GameObject[] MapTexte;
    public GameObject World, DeathWinPoint;
    public Sprite WinIcon, DeathIcon;
    private int GiveXP;
    void Awake() {
        foreach(GameObject i in HUD){
            if(i.GetComponent<SpriteRenderer>() != null){
                HudComponents_Sprites.Add(i.gameObject.GetComponent<SpriteRenderer>()); //Nimmt jedes GameObject aus dem Hud und trägt den Color component in eine liste ein um beim tod das hud langsam aus zublenden
            }else if(i.GetComponent<Image>() != null){
                HudComponents_Img.Add(i.gameObject.GetComponent<Image>());
            }else if(i.GetComponent<Text>() != null){
                HudComponents_Text.Add(i.gameObject.GetComponent<Text>());
            }
        }   
    }
    public void Start(){
       //AllrenderableGameObjects = FindObjectsOfType<Render_Manager>().Select(rm => rm.gameObject).ToArray();
       Inv = gameObject.GetComponent<Inventory_Handler>();
    }

    private void FixedUpdate() {
        //Minimap Punkt synchron halten
        float PointX = transform.position.x / 7f; //Aktuelle Position durch Differenz zur minimap in real world to UI Verhältniss
        float PointY = transform.position.y / 7f;
        Point.GetComponent<RectTransform>().localPosition = new Vector3(PointX, PointY, 0f);
    }

    public void Mapbutton(){
        //Mache Alles auf der Karte sichtbar
        /*foreach(GameObject i in AllrenderableGameObjects){
            if(i != null){
                i.GetComponent<Renderer>().enabled = true;
            }
        }*/
        MinimapCam.SetActive(true);
        BigMap.SetActive(true);
        X_Button.SetActive(true);
        Home_Button.SetActive(true);
        //Zone Alpha muss auf 0 gesetzt werden da sonst minimap textur transparent ist

        Time.timeScale = 0; //Pausiere das Spiel
    }

    public void ResumeGame(){
        //Map + X Button Entfernen
        BigMap.SetActive(false);
        X_Button.SetActive(false);
        Home_Button.SetActive(false);
        //Cam Aus
        MinimapCam.SetActive(false);
        //Spiel geht weiter
        Time.timeScale = 1; 
    }

    public IEnumerator Wait(){
        yield return new WaitForSeconds(.2f);
    }

    public void BacktoHome(){
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName:"Title_Menu");
    }
    
    private void Calcstats(){
        Kills.text = Inv.Kills.ToString(); //Kills
        Place.text = "#" +  World.GetComponent<Map_Manager>().Playercount.ToString(); //Placement
        SurvivedTime.text = World.GetComponent<Map_Manager>().SurvivedTime.ToString(); //Survived time
        Damage.text = Shoot.Damage_dealt.ToString();
        //calc coins to get and then apply to text component
        givecoins = Inv.Kills;
        if(World.GetComponent<Map_Manager>().Playercount == 1){ //Wenn Gewonnen + 10 Extra Coins
            givecoins += 10;
        }
        //Calc XP
        GiveXP = 0;
        GiveXP += Inv.Kills * 7; //Jeder Kill gibt 7XP
        double tobeconverted = (int)World.GetComponent<Map_Manager>().SurvivedTime * 0.05; //runde die survived time auf einen int
        GiveXP += (int)tobeconverted; //25 Sekunden survived time geben 1 XP

        //Zähl die coins später in einer animation hoch
        CoinsEarned.text = "0";
    }

    private IEnumerator Zoominout(){
        // Win Text rein raus zoom loop
        while(true){
            for(int i = 0; i != 10; i++){ //ZOOM IN
                WinText.GetComponent<RectTransform>().localScale = new Vector3(WinText.GetComponent<RectTransform>().localScale.x +.01f,WinText.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.03f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                WinText.GetComponent<RectTransform>().localScale = new Vector3(WinText.GetComponent<RectTransform>().localScale.x - .01f,WinText.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.03f);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void CalcDeathWinPoint(){
        //oben Rechts Auf der Map: X 527 Y 527
        //oben Rechts Auf der Minimap: X 223 Y 223 
        //Differenz: 2,36322869955157
        DeathWinPoint.SetActive(true);
        //Entscheide ob der Todes sprite oder Win Sprite angezeigt werden soll
        if(World.GetComponent<Map_Manager>().Playercount == 1) DeathWinPoint.GetComponent<Image>().sprite = WinIcon;
        else DeathWinPoint.GetComponent<Image>().sprite = DeathIcon;
        //Übersetz die Welt Position in Minimap Cooridinaten und Setz den Punkt da hin
        Vector3 thispos = this.gameObject.transform.position;
        DeathWinPoint.GetComponent<RectTransform>().localPosition = new Vector3(thispos.x / 2.36322869955157f, thispos.y / 2.36322869955157f, 0);
    }
    public IEnumerator EndScreen(bool win){
        //Spieler Scripts deaktivieren so das er nichts mehr machen kann wie rum laufen oder schießen
        this.gameObject.GetComponent<Movement>().enabled = false;
        this.gameObject.GetComponent<Shoot>().enabled = false;
        //Zieh die infos die die stats brauchen aus den jeweiligen scripts
        Calcstats();
        //Setz den Win oder Lose point auf der Minimap
        CalcDeathWinPoint();
        StatWindow.SetActive(true);
        MinimapCam.SetActive(true);
        //Schalte alle Texte auf der Map aus
        foreach(GameObject i in MapTexte){
            i.gameObject.SetActive(false);
        }
        Image StatwindowIMG = StatWindow.GetComponent<Image>(); //Jedes Stat element einmal in den Cache packen damit wir nicht jeden frame GetComponent machen müssen.
        Text KillsTEXT = Kills.gameObject.GetComponent<Text>();
        Text PlaceTEXT = Place.gameObject.GetComponent<Text>();
        Text DamageTEXT = Damage.gameObject.GetComponent<Text>();
        Text SurvivedTimeTEXT = SurvivedTime.gameObject.GetComponent<Text>();
        Text CoinsearnedTEXT = CoinsEarned.gameObject.GetComponent<Text>();
        RawImage MinimapDeathIMG = MinmapDeathTexture.gameObject.GetComponent<RawImage>();
        Image StatsBacktoHomeIMG = Stats_BacktohomeButton.gameObject.GetComponent<Image>();
        Image DeathPointIMG = DeathWinPoint.gameObject.GetComponent<Image>();
        //Blend Langsam das Fenster ein
        for(int runs = 0; runs != 255; runs++){
            // Debug.Log(runs);
            // Debug.Log(StatwindowIMG.color.a);
            // Debug.Log(Time.timeScale);
            StatwindowIMG.color = new Color(StatwindowIMG.color.r, StatwindowIMG.color.g, StatwindowIMG.color.b, StatwindowIMG.color.a + 0.003921568627451f); //0.003921568627451f stellt 1 dar. da alpha von 0 bis 255 geht und alpha aber im code zwischen 0-1 bestimmt werden muss also 255/1 = 0.003921568627451f
            KillsTEXT.color = new Color(0f, 0f, 0f, KillsTEXT.color.a + 0.003921568627451f);
            if(Place.text == "1") PlaceTEXT.color = new Color(255f, 255f, 0f, PlaceTEXT.color.a + 0.003921568627451f);
            else PlaceTEXT.color = new Color(0f, 0f, 0f, PlaceTEXT.color.a + 0.003921568627451f); //wenn erster platz mache text gelb
            DamageTEXT.color = new Color(0f, 0f, 0f, DamageTEXT.color.a + 0.003921568627451f);
            SurvivedTimeTEXT.color = new Color(0f, 0f, 0f, SurvivedTimeTEXT.color.a + 0.003921568627451f); //0f bei r g und b für den code schwarz
            CoinsearnedTEXT.color = new Color (0f, 255f, 255f, CoinsearnedTEXT.color.a + 0.003921568627451f);
            MinimapDeathIMG.color = new Color(255f, 255f, 255f, MinimapDeathIMG.color.a + 0.003921568627451f);
            StatsBacktoHomeIMG.color = new Color(255f,255f,255f, StatsBacktoHomeIMG.color.a + 0.003921568627451f);
            DeathPointIMG.color = new Color(255f, 255f, 255f, DeathPointIMG.color.a + 0.003921568627451f);
            //Hintergrund parallel ausblenden
            foreach(Image i in HudComponents_Img){
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - 0.003921568627451f);
                
                if(runs == 254){
                    i.gameObject.SetActive(false); //wenn alles durchsitig ist dann ganz deaktivieren damit nichts ausversehen gedrückt wird
                }
            }
            //Das selbe für alle Hud Componenten mit einem Sprite Renderer
            foreach(SpriteRenderer i in HudComponents_Sprites){
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - 0.003921568627451f);
                
                if(runs == 254){
                    i.gameObject.SetActive(false); //wenn alles durchsitig ist dann ganz deaktivieren damit nichts ausversehen gedrückt wird
                }
            }
            //und Text
            foreach(Text i in HudComponents_Text){
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - 0.003921568627451f);
                
                if(runs == 254){
                    i.gameObject.SetActive(false); //wenn alles durchsitig ist dann ganz deaktivieren damit nichts ausversehen gedrückt wird
                }
            }

            yield return new WaitForSeconds(.0013f);
        }
        //Deactivate HUD
        foreach(GameObject i in HUD){
            i.gameObject.SetActive(false); //Alles was noch nicht deaktiviert wird jetzt deaktivieren.
        }
        //Count up the coins
        for(float y = 0; y != givecoins + 1; y++){
            CoinsEarned.text = y.ToString();
            yield return new WaitForEndOfFrame();
        }
        Menu_Handler.localdata = Menu_Handler.loadeddata;
        Menu_Handler.localdata.Kills = Menu_Handler.loadeddata.Kills + givecoins; //Kills 
        Menu_Handler.localdata.Saved_Coins = Menu_Handler.loadeddata.Saved_Coins + givecoins; //Coins (Schon gespeicherte coins + neu verdient)
        if(win){
            Menu_Handler.localdata.Wins = Menu_Handler.loadeddata.Wins + 1;
            GiveXP += 25;
        }
        Menu_Handler.Writedata(Menu_Handler.localdata); //Stage changes
        Menu_Handler.AddXP(GiveXP); //XP Geben
        //Nach 1 runde darf man wieder Ads Schauen
        Menu_Handler.Watchedads = 0;
        //Write to Database
        StartCoroutine(OnlineManager.UpdateAfterGame());
    }
}