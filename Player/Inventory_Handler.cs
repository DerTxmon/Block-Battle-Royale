using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory_Handler : MonoBehaviour
{
    //Bools von denen aber nur eine gleichzeitig aktiv seien kann da ja nur eine Waffe in der hand gehalten werden kann.
    public bool Glock_18_Selected;
    public bool M4_Selected;
    public bool Ak47_Selected;
    public bool Sniper_Selected;
    public bool Mp7_Selected;
    public bool Shotgun_Selected;
    //Strings die besagen Welchen Item sich exact im Slot befindet.
    public string Slot1_Item;
    public string Slot2_Item;
    public string Slot3_Item;
    //Sind Slot Belegt bools.
    public bool Slot1 = false;
    public bool Slot2 = false;
    public bool Slot3 = false;
    //Aktuell Ausgewählter Slot
    public bool Slot1_Selected = false;
    public bool Slot2_Selected = false;
    public bool Slot3_Selected = false;
    //UI
    public Text Healtxt;
    //FPS Counter
    public Text FPS_Counter;
    //Ammonition
    public int small_ammo;
    public int mid_ammo;
    public int big_ammo;
    public int shotgun_ammo;
    public int slot1_mag_ammo;
    public int slot2_mag_ammo;
    public int slot3_mag_ammo;
    //Andere Objekte
    public GameObject Weapons;
    public int lootcount;
    public int lootcount2;
    public int Player_Heal;
    public int Player_Heal2;
    public GameObject Player;
    //Aktuelles Magazin und Reserve
    public int CurrentMag;
    public int CurrentMaxAmmo;
    public Camera MainCamera;
    public int schleifenx = 0;
    private float i;
    private string[] weapon_names = new string [7]{"Glock_18", "M4", "AK_47", "Sniper", "Mp7", "Fernglas", "Shotgun"};
    [SerializeField] private Sprite[] weapon_icons = new Sprite[7];
    int Average, counter, DisplayAverage; //FPS Variablen
    private int clickcount, DropWeaponAmmo, LastAmmo;
    private string lastslot;
    private float dropoffsetx, dropoffsety;
    [SerializeField] public GameObject Glock_18_Item, M4_Item, AK_47_Item, Sniper_Item, Mp7_Item, Fernglas_Item, Shotgun_Item, Slot1_GameObject, Slot2_GameObject, Slot3_GameObject;
    [SerializeField] private Sprite Placeholder;
    private Animator animator;
    public int Kills;
    public TextMeshProUGUI Small_Ammo_Reserve, Mid_Ammo_Reserve, Big_Ammo_Reserve, Shotgun_Ammo_Reserve;
    public Text Ammo_Reserve, Ammo_Mag;
    //UI
    public Image ShootbuttonSpriteObject;
    public Sprite ShootbuttonSprite;
    public Sprite HitbuttonSprite;
    public Image Healthbar;
    public Text HealthbarText;
    //else
    private bool iszooming = false;
    [SerializeField] private SpriteRenderer Sniperlaser;
    [SerializeField] private Transform Sniperlaserstart; //Ist einfach wo alle Waffen in der Hirachy sind
    public LayerMask layerMask;

    void Awake(){
        Small_Ammo_Reserve = GameObject.Find("Small Ammo Reserve").GetComponent<TextMeshProUGUI>();
        Mid_Ammo_Reserve = GameObject.Find("Mid Ammo Reserve").GetComponent<TextMeshProUGUI>();
        Big_Ammo_Reserve = GameObject.Find("Big Ammo Reserve").GetComponent<TextMeshProUGUI>();
        Shotgun_Ammo_Reserve = GameObject.Find("Shotgun Ammo Reserve").GetComponent<TextMeshProUGUI>();
        Ammo_Reserve = GameObject.Find("Ammo_Reserve").GetComponent<Text>();
        Ammo_Mag =GameObject.Find("Ammo_Mag").GetComponent<Text>();
    }
    void Start()
    {
        Healtxt = GameObject.Find("Heal_Count").GetComponent<Text>();
        Player = GameObject.Find("Player");
        Player_Heal = 0;
        StartCoroutine(Counting_FPS());
        animator = gameObject.GetComponent<Animator>();
        Slot1_function(); //Ersten Slot sofort wählen
    }
    // Update is called once per frame
    void Update(){
        //Laser folgt spieler (Gehört eigentlich in Shoot.cs)
        if(Sniper_Selected == true){
            float x = Sniperlaserstart.position.x;
            float y = Sniperlaserstart.position.y;
            Sniperlaser.enabled = true;
            //Schieß einen Raycast
            RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.up, 150f, layerMask); //Laser Calculation
            if(hitinfo){ //Wenn der Laser was trifft
                //schalte den Punkt an und setze ihn an collisions position
                Sniperlaser.enabled = true;
                Sniperlaser.transform.position = new Vector3(hitinfo.point.x, hitinfo.point.y, -26f);
            }
        }
        
        //Aktualisierung des Heal-Counters im Inv.
        Healtxt.text = Player_Heal.ToString(); //WTF hab ich hier geschrieben

        //Heal Count auf Rot stellen falls Kein Heal mehr übrig ist.
        if(Player_Heal == 0){
            Healtxt.color = new Color(255f, Healtxt.color.g, Healtxt.color.b, Healtxt.color.a); //Rot
        }else{ //WTF hab ich hier geschrieben2
            Healtxt.color = new Color(255f, 255f, 255f, Healtxt.color.a); //Schwarz
        }

        if(Player_Heal == 6) Player_Heal -= 1;
        if(Player_Heal2 == 6) Player_Heal -= 1; //Und hier

        try{
            //Munition im UI anzeigen
            Small_Ammo_Reserve.text = small_ammo.ToString();
            Mid_Ammo_Reserve.text = mid_ammo.ToString();
            Big_Ammo_Reserve.text = big_ammo.ToString(); //und hier
            Shotgun_Ammo_Reserve.text = shotgun_ammo.ToString();

            //MagUI Funktion darstellen.
            Ammo_Reserve.text = CurrentMaxAmmo.ToString();
            Ammo_Mag.text = CurrentMag.ToString();
        }catch{}

        //Mag wird aufs Aktuelle gewechselt. Mann Könnte bei den Conditionen bei Problemen andere Waffen auf False setzen.
        if(Glock_18_Selected){
            CurrentMaxAmmo = small_ammo;
        }else if(M4_Selected){
            CurrentMaxAmmo = mid_ammo;
        }else if(Ak47_Selected){
            CurrentMaxAmmo = mid_ammo;
        }else if(Sniper_Selected){
            CurrentMaxAmmo = big_ammo;
        }else if(Mp7_Selected){
            CurrentMaxAmmo = small_ammo;
        }else if(Shotgun_Selected){
            CurrentMaxAmmo = shotgun_ammo;
        }else{
            CurrentMaxAmmo = 0;
        }

        //Stellt das Aktuelle Slot mag auf den Current mag um dann mit dem CurrentMag zu reloden und anzuzeigen.
        if(Slot1_Selected == true){
            CurrentMag = slot1_mag_ammo;
        }else if(Slot2_Selected == true){
            CurrentMag = slot2_mag_ammo;
        }else if(Slot3_Selected == true){ //Wichtig!!! -> Ganze Update Funktion ist fürn Arsch optimiert. Wird später nochmal überarbeitet.
            CurrentMag = slot3_mag_ammo;
        }else{
            CurrentMag = 0;
        }
    }

    void LateUpdate() {
        lootcount = lootcount2; //Ist dazu da den Lootcount ein wenig später zu aktualisieren damit langsame handys keine Probleme beim Lootcount haben
        Player_Heal = Player_Heal2;
    }


    IEnumerator Counting_FPS(){
        //FPS counter für das UI
        while(true){
            int current = (int)(1f / Time.unscaledDeltaTime); //Calc current FPS
            Average = Average + current; // Calc Average FPS
            counter++;
            DisplayAverage = Average / counter;
            FPS_Counter.text = "FPS:" + current.ToString() + "\n" + "Average:" + DisplayAverage.ToString();
            yield return new WaitForSeconds(.1f);
        }
    }
    public IEnumerator CameraZoomOut(){
        //Camera passt sich zur Waffe an
            //if(!iszooming){ //Könnte bugs fixen oder auch dazu führen. Muss noch getestet werden. -> Funktioniert nicht
                iszooming = true;
                if(Sniper_Selected){
                    //Mach die Waffe in der Hand animation an
                    Player.GetComponent<Animator>().SetBool("Weaponactive", true);
                    ShootbuttonSpriteObject.sprite = ShootbuttonSprite;
                    for(i = MainCamera.GetComponent<Camera>().orthographicSize; i <= 20f; ){
                        if(i <= 20f){
                            i++;
                        }else{
                            i--;
                        }
                        MainCamera.GetComponent<Camera>().orthographicSize = i; //Sniper
                        yield return new WaitForSeconds(0.025f);
                    }
                    iszooming = false;
                }else if(M4_Selected){
                    //Mach die Waffe in der Hand animation an
                    Player.GetComponent<Animator>().SetBool("Weaponactive", true);
                    ShootbuttonSpriteObject.sprite = ShootbuttonSprite;
                    for(i = MainCamera.GetComponent<Camera>().orthographicSize; i != 14f; ){
                        if(i > 12f){
                            i--;
                        }else{
                            i++;
                        }
                        MainCamera.GetComponent<Camera>().orthographicSize = i; //M4
                        yield return new WaitForSeconds(0.01f);
                        iszooming = false;
                    }
                }else if(Ak47_Selected){
                    //Mach die Waffe in der Hand animation an
                    Player.GetComponent<Animator>().SetBool("Weaponactive", true);
                    ShootbuttonSpriteObject.sprite = ShootbuttonSprite;
                    for(i = MainCamera.GetComponent<Camera>().orthographicSize;i != 14f; ){
                        if(i > 12f){
                            i--;
                        }else{
                            i++;
                        }
                        MainCamera.GetComponent<Camera>().orthographicSize = i; //Ak
                        yield return new WaitForSeconds(0.01f);
                        iszooming = false;
                    }
                }else if(Mp7_Selected){
                    //Mach die Waffe in der Hand animation an
                    Player.GetComponent<Animator>().SetBool("Weaponactive", true);
                    ShootbuttonSpriteObject.sprite = ShootbuttonSprite;
                    for(i = MainCamera.GetComponent<Camera>().orthographicSize;i != 12f; ){
                        if(i > 12f){
                            i--;
                        }else{
                            i++;
                        }
                        MainCamera.GetComponent<Camera>().orthographicSize = i; //Ak
                        yield return new WaitForSeconds(0.01f);
                        iszooming = false;
                    }
                }else if(Glock_18_Selected){
                    //Mach die Waffe in der Hand animation an
                    Player.GetComponent<Animator>().SetBool("Weaponactive", true);
                    ShootbuttonSpriteObject.sprite = ShootbuttonSprite;
                    for(i = MainCamera.GetComponent<Camera>().orthographicSize;i != 10f; ){
                        if(i > 10f){
                            i--;
                        }else{
                            i++;
                        }
                        MainCamera.GetComponent<Camera>().orthographicSize = i; //Default
                        yield return new WaitForSeconds(0.01f);
                        iszooming = false;
                    }
                }else if(Shotgun_Selected){
                    //Mach die Waffe in der Hand animation an
                    Player.GetComponent<Animator>().SetBool("Weaponactive", true);
                    ShootbuttonSpriteObject.sprite = ShootbuttonSprite;
                    for(i = MainCamera.GetComponent<Camera>().orthographicSize;i != 11f; ){
                        if(i > 10f){
                            i--;
                        }else{
                            i++;
                        }
                        MainCamera.GetComponent<Camera>().orthographicSize = i; //Default
                        yield return new WaitForSeconds(0.01f);
                        iszooming = false;
                    }
                }else{ //Hand
                    //Mach die Waffe in der Hand animation an
                    Player.GetComponent<Animator>().SetBool("Weaponactive", false);
                    ShootbuttonSpriteObject.sprite = HitbuttonSprite;
                    for(i = MainCamera.GetComponent<Camera>().orthographicSize;i != 10f; ){
                        if(i > 10f){
                            i--;
                        }else{
                            i++;
                        }
                        MainCamera.GetComponent<Camera>().orthographicSize = i; //Default
                        yield return new WaitForSeconds(0.01f);
                        iszooming = false;
                    }
                }
            //}
    }
    public IEnumerator CarZoomOut(){
        for(i = MainCamera.GetComponent<Camera>().orthographicSize; i <= 20f; ){
            if(i <= 20f){
                i++;
            }else{
                i--;
            }
            MainCamera.GetComponent<Camera>().orthographicSize = i; //Sniper
            yield return new WaitForSeconds(0.025f);
        }
    }
    public void CheckforWeaponDrop(string x){
        if(x == lastslot){
            clickcount++;
        }else if(x != lastslot){
            clickcount = 1;
        }
        if(clickcount == 3){//Drop Weapon
        dropoffsetx = 0; //reset damit beim nächsten durchlauf die if statements nicht übersprungen werden
        dropoffsety = 0;
            do{
                dropoffsetx = Random.Range(-4.2f, 4.2f);
            }while((dropoffsetx < 4.5 && dropoffsetx > 4.2 || dropoffsetx < -4.2 && dropoffsetx > -4.2));
            do{
                dropoffsety = Random.Range(-3.2f, 3.2f);
            }while((dropoffsety < 3.5 && dropoffsety > 3.2 || dropoffsety < -3.2 && dropoffsety > -3.2));
            if(lastslot == "Slot1"){
                //Get all Information from dropped weapon and give the Ammo back to the Player
                //Droppe die Waffe in nem Random ort in einem kleinem Radius.
                if(Slot1_Item == "Glock_18"){
                    small_ammo += slot1_mag_ammo;
                    Instantiate(Glock_18_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Glock_18_Selected = false;
                }
                else if(Slot1_Item == "M4"){
                    mid_ammo += slot1_mag_ammo;
                    Instantiate(M4_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    M4_Selected = false;
                }
                else if(Slot1_Item == "AK_47"){
                    mid_ammo += slot1_mag_ammo;
                    Instantiate(AK_47_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Ak47_Selected = false;
                } 
                else if(Slot1_Item == "Sniper"){
                    big_ammo += slot1_mag_ammo;
                    Instantiate(Sniper_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Sniper_Selected = false;
                }
                else if(Slot1_Item == "Mp7"){
                    small_ammo += slot1_mag_ammo;
                    Instantiate(Mp7_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Mp7_Selected = false;
                }
                else if(Slot1_Item == "Shotgun"){
                    shotgun_ammo += slot1_mag_ammo;
                    Instantiate(Shotgun_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Shotgun_Selected = false;
                } 
                DeleteSlot(1); //Lösche Slot 1
                CameraZoomOut(); //Pass die Camera an
                clickcount = 0;
                animator.SetBool("Weaponactive", false);
                Slot1_Item = null;
                Slot1 = false;
            }else if(lastslot == "Slot2"){
                //Get all Information from dropped weapon to instantiate it later with the same ammo info
                //Droppe die Waffe in nem Random ort in einem kleinem Radius von 2f bis -2f
                if(Slot2_Item == "Glock_18"){
                    small_ammo += slot1_mag_ammo;
                    Instantiate(Glock_18_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Glock_18_Selected = false;
                }
                else if(Slot2_Item == "M4"){
                    mid_ammo += slot1_mag_ammo;
                    Instantiate(M4_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    M4_Selected = false;
                }
                else if(Slot2_Item == "AK_47"){
                    mid_ammo += slot1_mag_ammo;
                    Instantiate(AK_47_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Ak47_Selected = false;
                } 
                else if(Slot2_Item == "Sniper"){
                    big_ammo += slot1_mag_ammo;
                    Instantiate(Sniper_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Sniper_Selected = false;
                }
                else if(Slot2_Item == "Mp7"){
                    small_ammo += slot2_mag_ammo;
                    Instantiate(Mp7_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Mp7_Selected = false;
                }
                else if(Slot2_Item == "Shotgun"){
                    shotgun_ammo += slot2_mag_ammo;
                    Instantiate(Shotgun_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Shotgun_Selected = false;
                } 
                DeleteSlot(2); //Lösche Slot 2
                CameraZoomOut(); //Pass die Camera an
                clickcount = 0;
                animator.SetBool("Weaponactive", false);
                Slot2_Item = null;
                Slot2 = false;
            }else if(lastslot == "Slot3"){
                //Get all Information from dropped weapon to instantiate it later with the same ammo info
                //Droppe die Waffe in nem Random ort in einem kleinem Radius von 2f bis -2f
                if(Slot3_Item == "Glock_18"){
                    small_ammo += slot1_mag_ammo;
                    Instantiate(Glock_18_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Glock_18_Selected = false;
                }
                else if(Slot3_Item == "M4"){
                    mid_ammo += slot1_mag_ammo;
                    Instantiate(M4_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    M4_Selected = false;
                }
                else if(Slot3_Item == "AK_47"){
                    mid_ammo += slot1_mag_ammo;
                    Instantiate(AK_47_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Ak47_Selected = false;
                } 
                else if(Slot3_Item == "Sniper"){
                    big_ammo += slot1_mag_ammo;
                    Instantiate(Sniper_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Sniper_Selected = false;
                }
                else if(Slot1_Item == "Mp7"){
                    small_ammo += slot3_mag_ammo;
                    Instantiate(Mp7_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Mp7_Selected = false;
                }
                else if(Slot3_Item == "Shotgun"){
                    shotgun_ammo += slot3_mag_ammo;
                    Instantiate(Shotgun_Item, new Vector3(Player.transform.position.x + dropoffsetx, Player.transform.position.y + dropoffsety, 120f), Quaternion.identity);
                    Shotgun_Selected = false;
                } 
                DeleteSlot(3); //Lösche Slot 3
                CameraZoomOut(); //Pass die Camera an
                clickcount = 0;
                animator.SetBool("Weaponactive", false);
                Slot3_Item = null;
                Slot3 = false;
                }
            }
        }
    public void DeleteSlot(int slotnum){
        if(slotnum == 1){
            Slot1_GameObject = null; //Clear Slot 1
            Slot1_Item = null;
            Slot1 = false;
            lootcount2 -= 1;
            slot1_mag_ammo = 0;
            GameObject.Find("Icon1").GetComponent<Image>().sprite = Placeholder; 
        }else if(slotnum == 2){
            Slot2_GameObject = null; //Clear Slot 2
            Slot2_Item = null;
            Slot2 = false;
            lootcount2 -= 1;
            slot2_mag_ammo = 0;
            GameObject.Find("Icon2").GetComponent<Image>().sprite = Placeholder; 
        }else if(slotnum == 3){
            Slot3_GameObject = null; //Clear Slot 3
            Slot3_Item = null;
            Slot3 = false;
            lootcount2 -= 1;
            slot3_mag_ammo = 0;
            GameObject.Find("Icon3").GetComponent<Image>().sprite = Placeholder; 
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        //Slot1
        if(Slot1 == false){
        foreach(string i in weapon_names){
            if(collision.gameObject.tag == i && lootcount < 3){
                Slot1_Item = i;
                Slot1 = true;
                foreach(Sprite img in weapon_icons){
                    if(collision.gameObject.tag + "_Inventory_View" == img.name){
                        GameObject.Find("Icon1").GetComponent<Image>().sprite = img;
                    }
                }
                slot1_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
            }
        }
        }
        //Slot2
        else if(Slot2 == false && Slot1 == true){
        foreach(string i in weapon_names){
            if(collision.gameObject.tag == i && lootcount < 3){
                Slot2_Item = i;
                Slot2 = true;
                foreach(Sprite img in weapon_icons){
                    if(collision.gameObject.tag + "_Inventory_View" == img.name){
                        GameObject.Find("Icon2").GetComponent<Image>().sprite = img;
                    }
                }
                slot2_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
            }
        }
        }
        //Slot3
        else if(Slot3 == false && Slot2 == true && Slot1 == true){
        foreach(string i in weapon_names){
            if(collision.gameObject.tag == i && lootcount < 3){
                Slot3_Item = i;
                Slot3 = true;
                foreach(Sprite img in weapon_icons){
                    if(collision.gameObject.tag + "_Inventory_View" == img.name){
                        GameObject.Find("Icon3").GetComponent<Image>().sprite = img;
                    }
                }
                slot3_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
            }
        }
        }
        
        //Lootcount
        if(collision.gameObject.tag == "Glock_18" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "M4" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "AK_47" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Sniper" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Mp7" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Shotgun" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Heal" && Player_Heal <= 4){
            Player_Heal2 += 1;
        }

        //Munition Aufheben
        if(collision.gameObject.tag == "Small_Ammo"){
            small_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
        else if(collision.gameObject.tag == "Mid_Ammo"){
            mid_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
        else if(collision.gameObject.tag == "Big_Ammo"){
            big_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
        else if(collision.gameObject.tag == "Shotgun_Ammo"){
            shotgun_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
    }
    public void Slot1_function(){
        Debug.Log("Slot 1");
        if(Slot1_Item == "Glock_18"){
            //Aktive Waffe
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(true);
            Glock_18_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "M4"){
            //Aktive Waffe
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(true);
            M4_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "AK_47"){
            //Aktive Waffe
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(true);
            Ak47_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
        }else if(Slot1_Item == "Sniper"){
            //Schalte Laser ein
            //Hier weiter... Gerade Intensiv
            //Aktive Waffe
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(true);
            Sniper_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "Mp7"){
            //Aktive Waffe
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(true);
            Mp7_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "Shotgun"){
            //Aktive Waffe
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(true);
            Shotgun_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
        }else if(Slot1_Item == null || Slot1_Item == ""){
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }
        Slot1_Selected = true;
        Slot2_Selected = false;
        Slot3_Selected = false;
        
        //Check ob die Waffe bei doppelclick gedroppt werden soll
        CheckforWeaponDrop("Slot1");
        lastslot = "Slot1";
        //Pass die Kamera an
        StartCoroutine(CameraZoomOut());
        //Mach die Waffe in der Hand sichtbar
        gameObject.GetComponent<Weapon_Visibility>().Checkvisibility();
    }
    public void Slot2_function(){
        Debug.Log("Slot 2");
        if(Slot2_Item == "Glock_18"){
            //Aktive Waffe
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(true);
            Glock_18_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "M4"){
            //Aktive Waffe
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(true);
            M4_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "AK_47"){
            //Aktive Waffe
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(true);
            Ak47_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "Sniper"){
            //Aktive Waffe
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(true);
            Sniper_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "Mp7"){
            //Aktive Waffe
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(true);
            Mp7_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "Shotgun"){
            //Aktive Waffe
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(true);
            Shotgun_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
        }else if(Slot2_Item == null || Slot2_Item == ""){
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }
        Slot1_Selected = false;
        Slot2_Selected = true;
        Slot3_Selected = false;
        
        //Check ob die Waffe bei doppelclick gedroppt werden soll
        CheckforWeaponDrop("Slot2");
        lastslot = "Slot2";
        //Pass die Kamera an
        StartCoroutine(CameraZoomOut());
        //Mach die Waffe in der Hand sichtbar
        gameObject.GetComponent<Weapon_Visibility>().Checkvisibility();
    }
    public void Slot3_function(){
        Debug.Log("Slot3");
        if(Slot3_Item == "Glock_18"){
            //Aktive Waffe
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(true);
            Glock_18_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "M4"){
            //Aktive Waffe
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(true);
            M4_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "AK_47"){
            //Aktive Waffe
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(true);
            Ak47_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "Sniper"){
            //Aktive Waffe
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(true);
            Sniper_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "Mp7"){
            //Aktive Waffe
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(true);
            Mp7_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "Shotgun"){
            //Aktive Waffe
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(true);
            Shotgun_Selected = true;
            //Alle anderen Deaktivieren
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
        }else if(Slot3_Item == null || Slot3_Item == ""){
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            Sniper_Selected = false;
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            M4_Selected = false;
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            Ak47_Selected = false;
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            Glock_18_Selected = false;
            Weapons.transform.Find("Mp7_Top_Sprite").gameObject.SetActive(false);
            Mp7_Selected = false;
            Weapons.transform.Find("Shotgun_Top_Sprite").gameObject.SetActive(false);
            Shotgun_Selected = false;
        }
        Slot1_Selected = false;
        Slot2_Selected = false;
        Slot3_Selected = true;

        //Check ob die Waffe bei doppelclick gedroppt werden soll
        CheckforWeaponDrop("Slot3");
        lastslot = "Slot3";
        //Pass die Kamera an
        StartCoroutine(CameraZoomOut());
        //Mach die Waffe in der Hand sichtbar
        gameObject.GetComponent<Weapon_Visibility>().Checkvisibility(); 
    }

    public void Slot4_function(){
        //Heal Funktion
        Player_Health playerhealth = Player.GetComponent<Player_Health>(); //Einmal in den Cache laden
        if(Player_Heal > 0 && playerhealth.health < 200){
            Player_Heal2 -= 1;
            playerhealth.health += 20;
            if(playerhealth.health > 200) playerhealth.health = 200; //Prevent overheal
            //Refresh UI
            Healthbar.fillAmount = playerhealth.health / 200;
            HealthbarText.text = playerhealth.health + "%";
        }
    }
}