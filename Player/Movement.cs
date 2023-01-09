using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.U2D.Animation;
using System;

public class Movement : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    public FixedJoystick joystick1;
    public FixedJoystick joystick2;
    public Animator animatior;
    Vector3 lastpos;
    public float rotationSpeed;
    private Vector3 prevPosition = Vector3.zero;
    public GameObject Weapons;
    public Vector3 moveVector;
    public GameObject World;
    public Vector2 movement; 
    public bool tap;
    public Touch touch;
    public Shoot Shoot;
    public GameObject Player;
    public bool crouch;
    public float RotateX, RotateY;
    private bool steping;
    public GameObject Footsteps;
    public GameObject Footsteps2;
    public int schleifenx;
    public bool lootable;
    public bool lootbutton;
    public string Player_Name_Ingame;
    public GameObject Name_Text;
    public GameObject PostProcessingVolume, Wald;
    public SpriteLibraryAsset DefaultSkinLibrary, AgentSkinLibrary, BetaSkinLibrary, ClownSkinLibrary, AlienSkinLibrary, OttoSkinLibrary, ChrisSkinLibrary;
    private GameObject Joystick1handle;

    void Awake(){
        //Performance Settings from Menu
        if(Menu_Handler.performancemode == false){
            //-Low Performance settings-
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            //Disable Post Processing
            PostProcessingVolume.SetActive(false);
        }else{
            //-High Performance settings-
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 999;

            PostProcessingVolume.SetActive(true);
        }
        //Setz den Player auf die im Dropoff Screen angegebene Position
        //Skin Laden
        LoadPlayerSkin();
        Joystick1handle = joystick1.transform.Find("Handle").gameObject;
    }

    public void LoadPlayerSkin(){
        if(Menu_Handler.loadeddata.SelectedSkin == "Default"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = DefaultSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Beta"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = BetaSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Alien"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = AlienSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Clown"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = ClownSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Otto"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = OttoSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Chris"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = ChrisSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Agent"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = AgentSkinLibrary;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Dropoff_Handler.DropoffX != 0 || Dropoff_Handler.DropoffY != 0) Player.transform.position = new Vector3(Dropoff_Handler.DropoffX, Dropoff_Handler.DropoffY, 100f);//if statement nur für in editor bequemlichkeit
        Debug.Log(Dropoff_Handler.DropoffX);
        //Player_Name_Ingame = Menu_Handler.Player_Name;
        Name_Text.GetComponent<TextMeshPro>().text = Player_Name_Ingame;
        World = GameObject.Find("World");
        rb = GetComponent<Rigidbody2D>();
        animatior = gameObject.GetComponent<Animator>();
        Shoot = Player.GetComponent<Shoot>();
        StartCoroutine(FootstepGen());
    }

    // Update is called once per frame
    void Update(){
        //Namen überm Kopf anzeigen
        //Name_Text.GetComponent<Rigidbody2D>().rotation = 0;
        //Name_Text.GetComponent<RectTransform>().position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1.5f);
    }

    void FixedUpdate()
    {   
        float X = joystick1.Horizontal;
        float Y = joystick1.Vertical;

        Vector2 movementDir = new Vector2(X  * speed * Time.fixedDeltaTime, Y  * speed * Time.fixedDeltaTime);

        transform.Translate(movementDir, Space.World);

        if(transform.position != lastpos){
            animatior.SetBool("isrunning", true);
            //Animation speed
            float animspeed;
            animspeed = ((Vector2.Distance(Joystick1handle.transform.position, joystick1.transform.position)) / 2.37f) * 1.5f; //distanz vom halde vom center berechnen(max ist 2.37 um also auf eine range von 0-1 zu kommen teilen wir durch max)
            animatior.SetFloat("runspeed", animspeed);
            steping = true;
        }else{
            animatior.SetBool("isrunning", false);
            animatior.SetFloat("runspeed", 1f);
            steping = false;
        }
        lastpos = transform.position;

        //Rotation für rotationsstick setzen
        if(joystick2.Horizontal != 0f || joystick2.Vertical != 0f){
            RotateY = joystick2.Vertical;
            RotateX = joystick2.Horizontal;
        }else{
            RotateX = 0f;
            RotateY = 0f;
        }

        //in die richtung drehen in die man läuft falls roatations stick nicht bewegt wird
        if(X != 0 || Y != 0 && new Vector2(RotateX,RotateY) == Vector2.zero){
            transform.up = new Vector2(X,Y);
        }

        if(new Vector2(RotateX,RotateY) != Vector2.zero){
            transform.up = new Vector2(RotateX,RotateY);
        }
    }

    private IEnumerator FootstepGen(){
        begin:
        for( ;steping; ){
            Instantiate(Footsteps,new Vector3(transform.position.x, transform.position.y, 121f), transform.rotation);
            yield return new WaitForSeconds(.2f);
            if(!steping) break;
            Instantiate(Footsteps2, new Vector3(transform.position.x, transform.position.y, 121f), transform.rotation);
            yield return new WaitForSeconds(.2f);
        }
        if(!steping){
            yield return new WaitForSeconds(.2f);
            goto begin;
        }
    }

    public void enterlooting(){
        lootbutton = true;
    }

    public void exitlooting(){
        lootbutton = false;
    }
}