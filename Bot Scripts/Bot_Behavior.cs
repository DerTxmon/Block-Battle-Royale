using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

public class Bot_Behavior : MonoBehaviour
{

    public float speed;
    public Animator animatior;
    Vector3 lastpos;
    private Vector3 prevPosition = Vector3.zero;
    public GameObject Weapons;
    public GameObject World;
    public float X;
    public float Y;
    public float BewegungsängerungsZeit;
    public int Zyklus;
    private bool waiting, waiting1;
    Quaternion rot;
    Vector2 movement; 
    Rigidbody2D rb;
    public int Z;
    public bool EnemyContact, EnemyContact2 = false;
    public GameObject Enemy;
    private Vector2 movementdir;
    public bool movenormal;
    private float wait;
    public int schleifenx, schleifeny, schleifenz, schleifeni;
    public int newrandom;
    public bool Jiggle;
    public bool Shoot;
    public bool runaway;
    public GameObject Bot;
    public int randomizerweapon, randomizerweaponmax;
    public float angle;
    private float rotationoffset = 270f;
    public float missaim;
    private bool steping;
    public GameObject Footsteps;
    public GameObject Footsteps2;
    public GameObject Currloot;
    public GameObject EntryPoint;
    public bool looting;
    public bool lootable, lootbutton;
    public bool Wallinfront;
    public bool inhouse;
    public float rightsum, leftsum, topsum, bottomsum; //summe der rechten, linken, oberen und unteren nerven distances
    public float[] sums = new float[4];
    public float Player_Distance;
    public bool spawn_steps;
    public GameObject Player, Bot_Name_Text, Healthbar;
    private Bot_Optimazation bot_optimazation;
    private Rigidbody2D Bot_Name_Text_rb, Bot_Healthbar_rb;
    private RectTransform Bot_Name_Text_transform;
    private Bot_Shoot bot_shoot;
    private Bot_Inventory inv;
    private Animator anim;
    void Awake() {
        BewegungsängerungsZeit = 15f;
        StartCoroutine(Bot_Random_Move());
        bot_optimazation = this.gameObject.GetComponent<Bot_Optimazation>();
        Bot_Name_Text_rb = Bot_Name_Text.GetComponent<Rigidbody2D>();
        Bot_Name_Text_transform = Bot_Name_Text.GetComponent<RectTransform>();
        bot_shoot = Bot.GetComponent<Bot_Shoot>();
        Bot_Healthbar_rb = Healthbar.GetComponent<Rigidbody2D>();
        inv = Bot.GetComponent<Bot_Inventory>();
        anim = Bot.GetComponent<Animator>();
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        World = GameObject.Find("World");
        animatior = gameObject.GetComponent<Animator>();        
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FootstepGen());
        StartCoroutine(CheckforPlayerDistance());
        anim.SetBool("isrunning", true); //weil bot nie stehen bleibt
    }

    void Update()
    {
        //Name Bleibt überm Kopf
        Bot_Name_Text_rb.rotation = 0;
        Bot_Name_Text_transform.position = Bot.transform.position + new Vector3(0, 1.5f, 0);

        //Healthbar bleibt rechts
        Bot_Healthbar_rb.rotation = 0;
        Healthbar.transform.position = Bot.transform.position + new Vector3(.5f, 0, 0);

        //Setzt die summen in eine Array ein, damit sie später ausgewertet werden können
        /*sums[0] = rightsum;
        sums[1] = leftsum;
        sums[2] = topsum;
        sums[3] = bottomsum;*/

        //Animatior und Footsteps
        if(transform.position != lastpos){
            steping = true;
        }else{
            steping = false;
        }
        lastpos = transform.position;

        if(bot_optimazation.is_object_in_range == true){ //Nur wenn Objecte in der nähe sind.
            //Schaut ob der bot gerade richtig random gerichtet ist
            StartCoroutine(CheckforY());

            //Bot schaut Player an mit 1 Sec delay
            StartCoroutine(EnemyContactfunc());

            //Bot lootet
            StartCoroutine(Looting());
        }

        //Bot Orientiert sich zum Hause Eingang
        if(!inhouse) Entry_Orientation(); //boolean "inhouse" wird im Roof_Remove Script umgestellt sobald das Haus betreten wird.

        //Bot orientiert sich im Haus
        if(inhouse) StartCoroutine(House_Orientation());
    }

    void FixedUpdate() 
    {
        //Movement in calculierte richtung.
        Vector2 movementdir =  new Vector2(X,Y);
        if(movenormal == true){
            transform.Translate(movementdir * speed * Time.fixedDeltaTime, Space.World);
        }else{
            transform.Translate(movementdir * speed * Time.fixedDeltaTime, Space.Self);
        }
    }

    private IEnumerator CheckforY(){
        if(Y == 0.95f && EnemyContact == false && !looting && !waiting1 && !inhouse && EntryPoint == null || Y == -0.95f && EnemyContact == false && !looting && !waiting1 && !inhouse && EntryPoint == null){ //Macht Bot crazy wenn er weg läuft
            waiting1 = true;
            yield return new WaitForSeconds(.2f);
            X = Random.Range(-1f,1f);
            Y = Random.Range(-1f,1f);
            waiting1 = false;
            //Debug.Log("Successfully checked for Y");
        }
    }

    private void Entry_Orientation(){
        //Suche erstmal den eingang und orientiere dich dann darin.
        if(!looting && !EnemyContact && EntryPoint != null && Currloot == null){
            //Rotiere in Eingang richtung
            if(!EnemyContact && Currloot == null && !looting) movenormal = false;
            try{
                Vector2 lookDir = new Vector2(EntryPoint.transform.position.x, EntryPoint.transform.position.y) - rb.position; 
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 270f;
                rb.rotation = angle;
            }catch{}

            //Laufe in Eingang richtung
            if(EntryPoint != null && !EnemyContact && !looting){
                X = 0f;
                Y = 0.95f;
            }
        }else{
            if(!EnemyContact && Currloot == null && !looting) movenormal = true;
        } 
    }

    private IEnumerator House_Orientation(){
        //Wenn Entry2 noch angelaufen wird
        if(!looting && !EnemyContact && EntryPoint != null && Currloot == null){
            //Rotiere in Eingang richtung
            if(!EnemyContact && Currloot == null && !looting) movenormal = false;
            try{
                Vector2 lookDir = new Vector2(EntryPoint.transform.position.x, EntryPoint.transform.position.y) - rb.position; 
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 270f;
                rb.rotation = angle;
            }catch{}

            //Laufe in Eingang richtung
            if(EntryPoint != null && !EnemyContact && !looting){
                X = 0f;
                Y = 0.95f;
            }
        }else if(!looting && !EnemyContact && EntryPoint == null && Currloot == null){
            //Hier kommt die echte Houseorientation

            //Reset X und Y und stelle movenormal auf World.Space
            if (!looting && Currloot == null && !EnemyContact) movenormal = true;
            X = 0;
            Y = 0;

            yield return new WaitForSeconds(.1f);
            
            //IDEE
            //Loop einmal alle paar millisekunden durch alle nerven durch und setze punkte wo einziger stralen durch z.B. Türen durchgehen um dort hinzulaufen. 
        }
    }

    void Wandvoraugen(){
        //Wenn der Bot gegen eine Wand läuft dann drehe seine richtung nach rechts um.
        //IDEE 2: Bot dreht sich einfach von jeder wand weg die vor ihm ist wie oben erklärt.

    }

    private IEnumerator FootstepGen(){
        begin:
        while(steping){
            if(spawn_steps){ //Mache nur footsteps wenn der Spieler auch nah genug ist um sie in den nächsten 5-10 sekunden zu sehen
                Instantiate(Footsteps,new Vector3(transform.position.x, transform.position.y, 113f), transform.rotation);
                yield return new WaitForSeconds(.13f);
                if(!steping) break;
                Instantiate(Footsteps2, new Vector3(transform.position.x, transform.position.y,113f), transform.rotation);
            }
            yield return new WaitForSeconds(.13f);
        }
        if(!steping){
            yield return new WaitForSeconds(.13f);
            goto begin;
        }
    }

    public IEnumerator CheckforPlayerDistance(){
        while(true){
            Player_Distance = Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.y), new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y)); //misst wie weit der Bot vom Player weg ist um dann zu schauen ob Footsteps generiert werden müssen
            if(Player_Distance < 150f){
                spawn_steps = true;
            }else spawn_steps = false;
            yield return new WaitForSeconds(3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Zone"){
            //Drehe um Wenn Zone berührt wird
            X *= -1;
            Y *= -1;
        }
    }

    public void Weapon_Follow_Player(){
        Weapons.transform.position = transform.position;
        Weapons.transform.rotation = transform.rotation;
    }

    public IEnumerator Bot_Random_Move(){
        for(; ; Zyklus++){
            X =  Random.Range(-1f,1f);
            Y = Random.Range(-1f,1f);
            if(Zyklus == 10) BewegungsängerungsZeit = 15f;
            yield return new WaitForSeconds(BewegungsängerungsZeit);
        }
    }

    private IEnumerator Looting(){
        if(looting && Currloot != null && !EnemyContact){
            if(schleifeni == 0) schleifeni++;
            if(schleifeni == 1){
                yield return new WaitForSeconds(.4f);
            }

            if (looting && Currloot != null && !EnemyContact) movenormal = false;

            //Rotiere in Loot richtung
            try{
                Vector2 lookDir = new Vector2(Currloot.transform.position.x, Currloot.transform.position.y) - rb.position; 
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 270f;
                rb.rotation = angle;
            }catch{
                
            }

            //Laufe in Loot Richtung
            if(Currloot != null && !EnemyContact){
                X = 0f;
                Y = 0.95f;
            }

            //Betätige Lootbutton falls loot in der nähe
            try{
            if(Vector2.Distance(Currloot.transform.position, transform.position) < 3f){
                lootbutton = true;
            }else lootbutton = false;
            }catch{
                lootbutton = false;
            }

        }else{
            if(!EnemyContact && EntryPoint == null){
            if(X != 0 || Y != 0){
            transform.up = new Vector2(X,Y);
            }
            schleifeni = 0;
            looting = false;
            movenormal = true;
            }
        }
        
    }

    private IEnumerator TurnforXdegrees(int degrees){
        for(int i = 0; i != degrees; i++){
            transform.Rotate(0,0,1);
            yield return new WaitForSeconds(.01f);
        }
    }
    public IEnumerator EnemyContactfunc(){
        //Bei Spieler sichutung Augen Kontakt halten
        if(EnemyContact == true && Enemy != null){
            if(Z == 1 && waiting == false){
                waiting = true;
                yield return new WaitForSeconds(1.5f);
                Z++;
                waiting = false;
            }

            Vector2 lookDir = new Vector2(Enemy.transform.position.x, Enemy.transform.position.y) - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + rotationoffset + missaim;
            rb.rotation = angle;

            //Laufe weg wenn low hp 
            if(Enemy != null && Bot.GetComponent<Bot_Health>().health <= 50 || Bot.GetComponent<Bot_Inventory>().lootcount == 0){
                runaway = true;
                yield return new WaitForSeconds(.4f);
                Y = 0.95f;
                X = 0f;
                rotationoffset = 90f;
                //stop shooting
                Shoot = false;
            }else{
                runaway = false;
                rotationoffset = 270f;
            }

            //Verfolge Gegner.
            if(Enemy != null && Vector2.Distance(transform.position,Enemy.transform.position) >= 10.5f && runaway == false){
                X = 0f;
                Y = 0.95f;
                schleifenx = 0;
                schleifenz = 0;
                Jiggle = false;
            }

            //Bewegung stoppen wenn Gegner nah genug ist.
            if(Enemy != null && Vector2.Distance(transform.position,Enemy.transform.position) <= 9.5f && runaway == false){
                //Jiggle Movement:
                Jiggle = true;
                if(schleifenx <= 1) schleifenx++;
                if(schleifenx == 1){
                while(schleifenx < 300000 && Jiggle == true){
                    X = Random.Range(-1f,1f);
                    Y = Random.Range(-0.15f,0.1f);
                    yield return new WaitForSeconds(1f);
                }
                }
            }
            //identisch zu dem if davor
            if(Enemy != null && Vector2.Distance(transform.position,Enemy.transform.position) <= 9.5f && runaway == false){
                if(schleifenz <= 1) schleifenz++;
                if(schleifenz == 1){
                    while(Jiggle == true){
                    //Bot Rotation modifizieren damit er schlechter aimt(Alle paar sekunden).
                    missaim = Random.Range(-12f,12f);
                    yield return new WaitForSeconds(3f);
                    }
                }
            }

            if(EnemyContact && Enemy != null) movenormal = false;
            //Random Waffe raus hohlen und dann schießen.
            if(!runaway){ //Nur wenn er nicht wegläuft schiessen sonst schießt er ins nichts
            StartCoroutine(bot_shoot.Shoot()); //Schießen
            if(schleifeny <= 1 && inv.lootcount > 0) schleifeny++;
            if(schleifeny == 1){
                randomizerweapon = Random.Range(1, inv.lootcount);
                if(randomizerweapon == 1){
                    inv.Slot1_Selected = true;
                    inv.Slot2_Selected = false;
                    inv.Slot3_Selected = false;
                }else if(randomizerweapon == 2){
                    inv.Slot1_Selected = false;
                    inv.Slot2_Selected = true;
                    inv.Slot3_Selected = false;
                }else if(randomizerweapon == 3){
                    inv.Slot1_Selected = false;
                    inv.Slot2_Selected = false;
                    inv.Slot3_Selected = true;
                }
                //Animator soll den bot in Waffen haltung versetz
                anim.SetBool("Weaponactive", true);
            }
            }
        }else{
            if(!looting && EntryPoint == null){
            //Sonst immer in die lauf richtung schauen.
            if(X != 0 || Y != 0){
            transform.up = new Vector2(X,Y);
            }
            Jiggle = false;
            movenormal = true;
            schleifenx = 0;
            schleifeny = 0;
            schleifenz = 0;
            Shoot = false;
            runaway = false;
            missaim = 0;
            Enemy = null;
        }
        }
    }
}