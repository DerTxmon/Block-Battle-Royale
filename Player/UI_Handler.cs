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
    public GameObject World;
    void Awake() {
        World = GameObject.Find("World");
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
    public IEnumerator EndScreen(){
        //Spieler Scripts deaktivieren so das er nichts mehr machen kann wie rum laufen oder schießen
        this.gameObject.GetComponent<Movement>().enabled = false;
        this.gameObject.GetComponent<Shoot>().enabled = false;
        //Zieh die infos die die stats brauchen aus den jeweiligen scripts
        Calcstats();
        StatWindow.SetActive(true);
        MinimapCam.SetActive(true);
        //Schalte alle Texte auf der Map aus
        foreach(GameObject i in MapTexte){
            i.gameObject.SetActive(false);
        }
        //Blend Langsam das Fenster ein
        Time.timeScale = 1;
        for(int runs = 0; runs != 255; runs++){
            StatWindow.GetComponent<Image>().color = new Color(StatWindow.GetComponent<Image>().color.r, StatWindow.GetComponent<Image>().color.g, StatWindow.GetComponent<Image>().color.b, StatWindow.GetComponent<Image>().color.a + 0.003921568627451f); //0.003921568627451f stellt 1 dar. da alpha von 0 bis 255 geht und alpha aber im code zwischen 0-1 bestimmt werden muss also 255/1 = 0.003921568627451f
            Kills.gameObject.GetComponent<Text>().color = new Color(0f, 0f, 0f, Kills.gameObject.GetComponent<Text>().color.a + 0.003921568627451f);
            if(Place.text == "1") Place.gameObject.GetComponent<Text>().color = new Color(255f, 255f, 0f, Place.gameObject.GetComponent<Text>().color.a + 0.003921568627451f);
            else Place.gameObject.GetComponent<Text>().color = new Color(0f, 0f, 0f, Place.gameObject.GetComponent<Text>().color.a + 0.003921568627451f); //wenn erster platz mache text gelb
            Damage.gameObject.GetComponent<Text>().color = new Color(0f, 0f, 0f, Damage.gameObject.GetComponent<Text>().color.a + 0.003921568627451f);
            SurvivedTime.gameObject.GetComponent<Text>().color = new Color(0f, 0f, 0f, SurvivedTime.gameObject.GetComponent<Text>().color.a + 0.003921568627451f); //0f bei r g und b für den code schwarz
            CoinsEarned.gameObject.GetComponent<Text>().color = new Color (0f, 255f, 255f, CoinsEarned.GetComponent<Text>().color.a + 0.003921568627451f);
            MinmapDeathTexture.gameObject.GetComponent<RawImage>().color = new Color(255f, 255f, 255f, MinmapDeathTexture.GetComponent<RawImage>().color.a + 0.003921568627451f);
            Stats_BacktohomeButton.gameObject.GetComponent<Image>().color = new Color(255f,255f,255f, Stats_BacktohomeButton.GetComponent<Image>().color.a + 0.003921568627451f);
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
        //StartCoroutine(Zoominout());
        //Save Stats to save.json
        Menu_Handler.localdata.Kills = Menu_Handler.loadeddata.Kills + givecoins; //Kills 
        Menu_Handler.localdata.Saved_Coins = Menu_Handler.loadeddata.Saved_Coins + givecoins; //Coins (Schon gespeicherte coins + neu verdient)
        Menu_Handler.localdata.Wins = Menu_Handler.loadeddata.Wins;
        Menu_Handler.localdata.Saved_Player_Name = Menu_Handler.loadeddata.Saved_Player_Name; //Name bleibt gleich
        Menu_Handler.localdata.PlayerID = Menu_Handler.loadeddata.PlayerID;
        Menu_Handler.Writedata(); //Stage changes
        
        Time.timeScale = 0f;
    }
}