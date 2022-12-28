using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;

public class Menu_Handler : MonoBehaviour
{
    public GameObject Player, Shop_Button, Play_Button, Friends_Button, Inv_Button, Coin_Counter, Emerald_Counter, Player_Name_Button, Name_Input, BG1, BG2, BG3, BG4, ShopBG, Logo, Settings_Button, Settings_BG, Settings_Menu, Settings_Menu_X, AddBotButton, RemoveBotButton, Bot_Display;
    public GameObject LobbyMusic, Settings_X;
    public Text Coin_Count, Emerald_Count, Player_Name_Text, Bot_Counter;
    public GameObject[] BGPoints;
    public int Coins; //Muss noch Automatisch gezählt werden wird aber vorerst auf 123 Gesetzt
    public static string Player_Name = "Player";
    public static int Menu_Bots = 1;
    public float CAMSPEED = 0.001f;
    public float Playerspinspeed;
    public static bool performancemode = true;
    public Image Black;
    public static UserData localdata = new UserData(); //Sofort das Localdata object erstellen damit sofort zum launch des spiels daten gelesen/geschrieben werden können
    public static UserData loadeddata;
    private GameObject activeBG;
    private string[] sonderzeichen = {"!","§","$","%","&","/","(",")","=","?","}","]","[","{","³","²","^","°","<",">","|","+","*","~",",",":",";","-","_","\u00b4" /*´*/,"`", "'\u0022'"/*backslash*/};
    //Skins
    [SerializeField] private Sprite DefaultSkinSprite, BetaSkinSprite, AgentSkinSprite, OttoSkinSprite, ClownSkinSprite, AlienSkinSprite, ChrisSkinSprite;
    public GameObject Menu_Skin_Display;
    public Image DefaultSkinButton, BetaSkinButton, AgentSkinButton, ClownSkinButton, OttoSkinButton, ChrisSkinButton, AlienSkinButton;
    //
    //Alle Skin buttons
    //
    public Sprite DefaultSkinButtonGreen, DefaultSkinButtonRed, DefaultSkinButtonGray, BetaSkinButtonGreen, BetaSkinButtonGray, BetaSkinButtonRed,
    AgentSkinButtonGreen, AgentSkinButtonRed, AgentSkinButtonGray, ClownSkinButtonGreen, ClownSkinButtonRed, ClownSkinButtonGray,
    OttoSkinButtonGreen, OttoSkinButtonRed, OttoSkinButtonGray, ChrisSkinButtonGreen, ChrisSkinButtonRed, ChrisSkinButtonGray,
    AlienSkinButtonGreen, AlienSkinButtonRed, AlienSkinButtonGray;
    
    //Alle skins nochmal in high res fürs Display
    public Sprite DefaultDispalayModel, BetaDisplayModel, AgentDisplayModel, ClownDisplayModel, OttoDisplayModel, AlienDisplayModel, ChrisDisplayModel;
    //Selbe in einer Array
    public Sprite[] DisplayModels;

    //Inventory
    public GameObject InvBG;
    public GameObject Inv_Hud;
    public GameObject[] SkinFields;
    public GameObject X_Inv;

    //Level
    [SerializeField] private Text Leveltext;
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image ProgressBarBG;
    [SerializeField] private Text neededXP;

    //Friends, Online
    [SerializeField] private GameObject onlineBG;
    [SerializeField] private GameObject FriendWindow;
    [SerializeField] private GameObject LoadingText;
    [SerializeField] private GameObject IDText;
    [SerializeField] private GameObject FriendsScroll;
    [SerializeField] private GameObject X_Friends;
    [SerializeField] private GameObject[] FriendSlots = new GameObject[6];
    [SerializeField] private GameObject MESlot;
    [SerializeField] private GameObject FriendSlot;
    [SerializeField] private GameObject AddFriendSlot;
    [SerializeField] private GameObject AddFriendInput;
    [SerializeField] private List<GameObject> LoadedFriends;
    [SerializeField] private GameObject FriendListButton;
    [SerializeField] private GameObject GlobalLeaderboardButton;
    [SerializeField] private GameObject RankLeaderboardButton;
    [SerializeField] private Sprite FriendListButton_Green;
    [SerializeField] private Sprite FriendListButton_Gray;
    [SerializeField] private Sprite GlobalLeaderboardButton_Green;
    [SerializeField] private Sprite GlobalLeaderboardButton_Gray;
    [SerializeField] private Sprite RankLeaderboardButton_Green;
    [SerializeField] private Sprite RankLeaderboardButton_Gray;

    void Awake(){
        ChooseBG();
        StartCoroutine(PlayerSpin());
        StartCoroutine(BGMove());
        StartCoroutine(ButtonAnimation());
    }

    void Start(){
        try{
            Readdata(); //Versuche Spieler daten zu lesen und wenn keine da sind

            //Set Saved Player name
            Player_Name_Text.GetComponent<Text>().text = loadeddata.Saved_Player_Name;
            //Set Saved Coins to display
            Coin_Count.GetComponent<Text>().text = loadeddata.Saved_Coins.ToString();
            Emerald_Count.GetComponent<Text>().text = loadeddata.Saved_Emeralds.ToString();
            //Setz die anzeige sofort auf 1
            Bot_Counter.text = Menu_Bots.ToString();
            InitSkinButtons();
            InitLevel();
        }catch{
            StartCoroutine(InitUserData()); //Erstelle neue Spielerdaten und lese sie sofort
        }
    }

    public static void Writedata(UserData Data){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Create);
        bf.Serialize(file, Data);
        file.Close();
    }

    public void Readdata(){
        if(File.Exists(Application.persistentDataPath + "/save.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            loadeddata = (UserData)bf.Deserialize(file);
            file.Close();
        }else throw new NullReferenceException();
    }

    public IEnumerator InitUserData(){
        Writedata(new UserData()); //Erstelle frischen Spielstand
        //Record joined version
        yield return new WaitForEndOfFrame();
        Readdata();
        yield return new WaitForEndOfFrame();
        localdata = loadeddata;
        //Record first launch Version
        if(Application.version.Contains("0.9")){
            //Give Beta Skin
            localdata.Beta_Ownage = true;
        }
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        yield return new WaitForEndOfFrame();
        //Register on Database
        StartCoroutine(OnlineManager.RegisterNewUserOnDatabase());
        while(OnlineManager.NewID == 0){ yield return new WaitForEndOfFrame(); }
        localdata.PlayerID = OnlineManager.NewID;
        localdata.Saved_Player_Name = "Player#" + OnlineManager.NewID.ToString();
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
        localdata = loadeddata;
        //Set Saved Player name
        Player_Name_Text.GetComponent<Text>().text = loadeddata.Saved_Player_Name;
        //Set Saved Coins to display
        Coin_Count.GetComponent<Text>().text = loadeddata.Saved_Coins.ToString();
        Emerald_Count.GetComponent<Text>().text = loadeddata.Saved_Emeralds.ToString();
        //Setz die anzeige sofort auf 1
        Bot_Counter.text = Menu_Bots.ToString();
        InitSkinButtons();
        InitLevel();
        yield return null;
    }

    //User Saved Data Object
    [Serializable]
    public class UserData{
        //Save Data
        public string Saved_Player_Name = "Player#0000";
        public int Saved_Coins = 50;
        public int Saved_Emeralds = 5;
        public int Wins = 0;
        public int Kills = 0;
        //Skins
        public string SelectedSkin = "Default";
        public bool Default_Ownage = true;
        public bool Agent_Ownage = false;
        public bool Clown_Ownage = false;
        public bool Beta_Ownage = false;
        public bool Otto_Ownage = false;
        public bool Alien_Ownage = false;
        public bool Chris_Ownage = false;
        public int aktuelleXP = 0;
        public int TonextLevelXP = 50;
        public int Level = 1;
        public int PlayerID = 0; //Web Server teilt einmalig zu und wird ab dann nur noch gelesen.
        public string joinedversion;
        public int[] FriendsIDs = new int[6]; //Wird bei aufrufen der Freunde einmal durchgelooped
    }

    public void InitLevel(){
        //jedes level + 30xp
        //Überfällige XP berechnen
        //auf dem balken richtig anzeigen

        float displayprogressinprecent = (float)loadeddata.aktuelleXP / (float)loadeddata.TonextLevelXP;
        ProgressBar.fillAmount = displayprogressinprecent;

        Leveltext.text = "LEVEL " + loadeddata.Level.ToString();
        neededXP.text = loadeddata.aktuelleXP.ToString() + "XP" + "/" + loadeddata.TonextLevelXP.ToString() + "XP";
    }
    public static void AddXP(int GiveXP){
        float xpuntillvlup = loadeddata.TonextLevelXP - loadeddata.aktuelleXP;
        //schau ob mehr als 1 level up geht Level hoch geht
        for( ; GiveXP != 0; GiveXP--){
            localdata.aktuelleXP++; //1XP pro durchlauf hinzufügen
            if(localdata.aktuelleXP == localdata.TonextLevelXP){
                localdata.Level++; //Level up
                localdata.TonextLevelXP += (int)(localdata.TonextLevelXP / 4) + 30; //Immer 30xp mehr pro höheres level
                if(localdata.Level % 10 == 0){ //Jedes 10 level eine coin
                    AddEmerald(1);
                }
            }
        }
        Writedata(localdata); //Save
    }
    public static void AddEmerald(int ammount){
        localdata.Saved_Emeralds += ammount;
        Writedata(localdata);
    }
    public void ItemAddedWindow(){

    }
    public void ChooseBG(){
        int BGnum = UnityEngine.Random.Range(0,4);
        if(BGnum == 0){
            BG1.SetActive(true);
            BG2.SetActive(false);
            BG3.SetActive(false);
            BG4.SetActive(false);
            activeBG = BG1;
        }else if(BGnum == 1){
            BG1.SetActive(false);
            BG2.SetActive(true);
            BG3.SetActive(false);
            BG4.SetActive(false);
            activeBG = BG2;
        }else if(BGnum == 2){
            BG1.SetActive(false);
            BG2.SetActive(false);
            BG3.SetActive(true);
            BG4.SetActive(false);
            activeBG = BG3;
        }else if(BGnum == 3){
            BG1.SetActive(false);
            BG2.SetActive(false);
            BG3.SetActive(false);
            BG4.SetActive(true);
            activeBG = BG4;
        }
    }
    public IEnumerator ButtonAnimation(){
        //Play Button Rein Raus zoom
        while(true){
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Play_Button.GetComponent<RectTransform>().localScale = new Vector3(Play_Button.GetComponent<RectTransform>().localScale.x +.01f,Play_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.03f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Play_Button.GetComponent<RectTransform>().localScale = new Vector3(Play_Button.GetComponent<RectTransform>().localScale.x - .01f,Play_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.03f);
            }
            //Friends Button
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Friends_Button.GetComponent<RectTransform>().localScale = new Vector3(Friends_Button.GetComponent<RectTransform>().localScale.x +.01f,Friends_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Friends_Button.GetComponent<RectTransform>().localScale = new Vector3(Friends_Button.GetComponent<RectTransform>().localScale.x - .01f,Friends_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            //Shop Button
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Shop_Button.GetComponent<RectTransform>().localScale = new Vector3(Shop_Button.GetComponent<RectTransform>().localScale.x +.01f,Shop_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Shop_Button.GetComponent<RectTransform>().localScale = new Vector3(Shop_Button.GetComponent<RectTransform>().localScale.x - .01f,Shop_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            //Inv Button
            for(int i = 0; i != 10; i++){ //ZOOM IN
                Inv_Button.GetComponent<RectTransform>().localScale = new Vector3(Inv_Button.GetComponent<RectTransform>().localScale.x +.01f,Inv_Button.GetComponent<RectTransform>().localScale.y + .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            for(int i = 10; i != 0; i--){ //ZOOM OUT
                Inv_Button.GetComponent<RectTransform>().localScale = new Vector3(Inv_Button.GetComponent<RectTransform>().localScale.x - .01f,Inv_Button.GetComponent<RectTransform>().localScale.y - .01f,0f);
                yield return new WaitForSeconds(.025f);
            }
            yield return new WaitForSeconds(3.5f);
        }
    }

    public IEnumerator PlayerSpin(){
        while(true){
            Player.GetComponent<Rigidbody2D>().rotation += Playerspinspeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void performanceswitch(bool mode){
        performancemode = mode;
    }

    public IEnumerator BGMove(){
        while(true){
            int rand = UnityEngine.Random.Range(0,4); //Suche einen der 4 Punkte zum Moven aus
            Vector2 nextcampos = BGPoints[rand].transform.position;
            while(new Vector2(transform.position.x, transform.position.y) != new Vector2(BGPoints[rand].transform.position.x, BGPoints[rand].transform.position.y)){
                transform.position = Vector2.MoveTowards(transform.position, BGPoints[rand].transform.position, CAMSPEED * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

    }

    public void FriendButtonMenu(){
        StartCoroutine(FriendsWindowCR());
    }
    public IEnumerator FriendsWindowCR(){
        //Deactivate Hud
        Player.SetActive(false);
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        activeBG.SetActive(false);
        AddBotButton.SetActive(false);
        RemoveBotButton.SetActive(false);
        Bot_Display.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);

        onlineBG.SetActive(true);
        X_Friends.SetActive(true);
        FriendWindow.SetActive(true);
        FriendListButton.SetActive(true);
        GlobalLeaderboardButton.SetActive(true);
        RankLeaderboardButton.SetActive(true);

        FriendListButton.GetComponent<Image>().sprite = FriendListButton_Green; //this
        RankLeaderboardButton.GetComponent<Image>().sprite = RankLeaderboardButton_Gray;
        GlobalLeaderboardButton.GetComponent<Image>().sprite = GlobalLeaderboardButton_Gray;

        LoadingText.SetActive(true);
        //Create you own Slot
        //Positionierung
        GameObject SlotToBeIn = MESlot;
        GameObject Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
        LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
        Slot.transform.parent = SlotToBeIn.transform;
        Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
        //Get Information from Server
        OnlineManager.RecievedData = null;
        StartCoroutine(OnlineManager.GetAllUserData(loadeddata.PlayerID.ToString())); //Start fetching own data
        while(OnlineManager.RecievedData == null){ //wait until 
            yield return new WaitForEndOfFrame();
        }
        //Apply Information
        Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6]; //Eigentlich Kills
        Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
        Slot.transform.Find("Name").GetComponent<Text>().text = "ME";
        Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
        Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
        //Skin
            Image SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
            //Check which skin the database sends and then apply
            foreach(Sprite model in DisplayModels){
                if(model.name == OnlineManager.RecievedData[1]){
                    SkinSlot.sprite = model;
                }
            }
        //Start getting Friends Data...
        int x = 0;
        foreach(int i in loadeddata.FriendsIDs){
            if(i != 0){
                //Positionierung
                SlotToBeIn = FriendSlots[x];
                Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
                LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
                Slot.transform.parent = SlotToBeIn.transform;
                Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
                //Get Information from Server
                OnlineManager.RecievedData = null;
                StartCoroutine(OnlineManager.GetAllUserData(i.ToString())); //Start fetching Friends data
                while(OnlineManager.RecievedData == null){ //wait until 
                    yield return new WaitForEndOfFrame();
                }
                //Apply Information
                Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6];
                Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
                Slot.transform.Find("Name").GetComponent<Text>().text = OnlineManager.RecievedData[0];
                Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
                Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
                //Skin
                SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
                //Check which skin the database sends and then apply
                foreach(Sprite model in DisplayModels){
                    if(model.name == OnlineManager.RecievedData[1]){
                        SkinSlot.sprite = model;
                    }
                }
                x++;
            }
        }
        //Füll den Rest mit Freund Add Slots
        for( ; x != 6; x++){
            SlotToBeIn = FriendSlots[x];
            Slot = Instantiate(AddFriendSlot, SlotToBeIn.transform);
            Slot.transform.parent = SlotToBeIn.transform;
            Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent

            //Funktion OnClick hinzufügen
            Slot.GetComponent<Button>().onClick.AddListener(() => {
                AddFriendButton();
            });
        }
        LoadingText.SetActive(false);
        FriendsScroll.SetActive(true);
        //Show ID
        IDText.SetActive(true);
        IDText.GetComponent<Text>().text = "ID: " + "#" + loadeddata.PlayerID;
    }
    public void AddFriendButton(){
        //deaktiviere Hintergrund
        FriendsScroll.SetActive(false);
        //Aktiviee Input
        AddFriendInput.SetActive(true);
    }   
    public void SubmitFriendButton(string ID){
        StartCoroutine(SubmitFriendButton2(ID));
    }
    private IEnumerator SubmitFriendButton2(string ID){
        //Schau ob die ID in der Datenbank vorhanden ist
        StartCoroutine(OnlineManager.CheckforExistence(ID));
        while(OnlineManager.FriendExistance == 0){
            yield return new WaitForEndOfFrame(); //warte bis Coroutine fertig ist
        } 
        if(OnlineManager.FriendExistance == 1){
            localdata = loadeddata;
            int arrayslot = 0; //Schaue welcher slot der nächste mit 0 is in der array um da die Friends ID rein zu packen.
            foreach(int i in loadeddata.FriendsIDs){
                if(i != 0){ //null == 0 bei int's
                    arrayslot++;
                }
            }
            localdata.FriendsIDs[arrayslot] = Int32.Parse(ID);
            StartCoroutine(InitUserData3());
            yield return new WaitForEndOfFrame(); //Weil zwischen Init User Data und Web Request ein frame sein muss
            StartCoroutine(RefreshFriendList());
            AddFriendInput.SetActive(false);
        }else if(OnlineManager.FriendExistance == 2){
            Debug.Log("Friend not found");
        }
    }  
    public void GlobalLeaderboardButtonFunc(){
        StartCoroutine(GlobalLeaderboardButtonFuncCR());
    }
    private IEnumerator GlobalLeaderboardButtonFuncCR(){
        //Activate this button and deactivate the others
        FriendsScroll.SetActive(false);
        AddFriendInput.SetActive(false);
        FriendListButton.GetComponent<Image>().sprite = FriendListButton_Gray;
        RankLeaderboardButton.GetComponent<Image>().sprite = RankLeaderboardButton_Gray;
        GlobalLeaderboardButton.GetComponent<Image>().sprite = GlobalLeaderboardButton_Green; //this

        LoadingText.SetActive(true);
        //...
        StartCoroutine(OnlineManager.GetTop30());
        yield return null;
    }
    public void RankLeaderboardButtonFunc(){

    }
    public IEnumerator InitUserData3(){
        Writedata(localdata);
        yield return new WaitForEndOfFrame();
        Readdata();
    }
    public IEnumerator RefreshFriendList(){
        //Destroy all friend cards
        foreach(GameObject i in LoadedFriends){
            if(i != null){
                Destroy(i);
            }
        }
        //Build new
        LoadingText.SetActive(true);
        //Create you own Slot
        //Positionierung
        GameObject SlotToBeIn = MESlot;
        GameObject Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
        LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
        Slot.transform.parent = SlotToBeIn.transform;
        Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
        //Get Information from Server
        OnlineManager.RecievedData = null;
        StartCoroutine(OnlineManager.GetAllUserData(loadeddata.PlayerID.ToString())); //Start fetching own data
        while(OnlineManager.RecievedData == null){ //wait until 
            yield return new WaitForEndOfFrame();
        }
        //Apply Information
        Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6]; //Eigentlich Kills
        Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
        Slot.transform.Find("Name").GetComponent<Text>().text = "ME";
        Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
        Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
        //Skin
            Image SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
            //Check which skin the database sends and then apply
            foreach(Sprite model in DisplayModels){
                if(model.name == OnlineManager.RecievedData[1]){
                    SkinSlot.sprite = model;
                }
            }
        //Start getting Friends Data...
        int x = 0;
        foreach(int i in loadeddata.FriendsIDs){
            if(i != 0){
                //Positionierung
                SlotToBeIn = FriendSlots[x];
                Slot = Instantiate(FriendSlot, SlotToBeIn.transform);
                LoadedFriends.Add(Slot); //um später beim neu laden zu löschen
                Slot.transform.parent = SlotToBeIn.transform;
                Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent
                //Get Information from Server
                OnlineManager.RecievedData = null;
                StartCoroutine(OnlineManager.GetAllUserData(i.ToString())); //Start fetching Friends data
                while(OnlineManager.RecievedData == null){ //wait until 
                    yield return new WaitForEndOfFrame();
                }
                //Apply Information
                Slot.transform.Find("Level").GetComponent<Text>().text = "Level:" + OnlineManager.RecievedData[6];
                Slot.transform.Find("Wins").GetComponent<Text>().text = "Wins:" + OnlineManager.RecievedData[4];
                Slot.transform.Find("Name").GetComponent<Text>().text = OnlineManager.RecievedData[0];
                Slot.transform.Find("Coins").GetComponent<Text>().text = OnlineManager.RecievedData[2];
                Slot.transform.Find("Emeralds").GetComponent<Text>().text = OnlineManager.RecievedData[3];
                //Skin
                SkinSlot = Slot.transform.Find("Skin").GetComponent<Image>();
                //Check which skin the database sends and then apply
                foreach(Sprite model in DisplayModels){
                    if(model.name == OnlineManager.RecievedData[1]){
                        SkinSlot.sprite = model;
                    }
                }
                x++;
            }
        }
        //Füll den Rest mit Freund Add Slots
        for( ; x != 6; x++){
            SlotToBeIn = FriendSlots[x];
            Slot = Instantiate(AddFriendSlot, SlotToBeIn.transform);
            Slot.transform.parent = SlotToBeIn.transform;
            Slot.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,-5000f); //setze genau auf sein Parent

            //Funktion OnClick hinzufügen
            Slot.GetComponent<Button>().onClick.AddListener(() => {
                AddFriendButton();
            });
        }
        LoadingText.SetActive(false);
        FriendsScroll.SetActive(true);
    }
    public void ShopButton(){
        //Deactivate Hud
        Player.SetActive(false);
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        activeBG.SetActive(false);
        AddBotButton.SetActive(false);
        RemoveBotButton.SetActive(false);
        Bot_Display.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);

        ShopBG.SetActive(true);
        X_Inv.SetActive(true);
    }

    public void InventoryButton(){
        //Deactivate Hud
        Player.SetActive(false);
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        activeBG.SetActive(false);
        AddBotButton.SetActive(false);
        RemoveBotButton.SetActive(false);
        Bot_Display.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);


        //Activate Inventory
        InvBG.SetActive(true);
        Inv_Hud.SetActive(true);
        X_Inv.SetActive(true);
    }

    public void ChangeName(){
        Player.SetActive(false); //Deaktiviere das Ganze Menü und mache nur den Input sichtbar
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Settings_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Leveltext.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
        ProgressBarBG.gameObject.SetActive(false);
        neededXP.gameObject.SetActive(false);

        Name_Input.SetActive(true); //Input Field
    }

    public void ChangeNameDone(string Nick){
        Player.SetActive(true); 
        Shop_Button.SetActive(true);
        Play_Button.SetActive(true);
        Coin_Counter.SetActive(true);
        Emerald_Counter.SetActive(true);
        Player_Name_Button.SetActive(true);
        Player_Name_Text.gameObject.SetActive(true);
        Friends_Button.gameObject.SetActive(true);
        Inv_Button.gameObject.SetActive(true);
        Settings_Button.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);
        Leveltext.gameObject.SetActive(true);
        ProgressBar.gameObject.SetActive(true);
        ProgressBarBG.gameObject.SetActive(true);
        neededXP.gameObject.SetActive(true);

        Name_Input.SetActive(false); //Input Field

        if(Nick == null){
            Player_Name = "Player";
            
        }else{
            int y = 0;
            foreach(string i in sonderzeichen){
                if(Nick.Contains(i)){
                    y = 1; //blocks if statement
                    //throw window with notifitation
                }
            }
            if(y == 0){
                Player_Name = Nick;
                Refresh_PlayerName();
                StartCoroutine(OnlineManager.UpdateNickname());
            }
        } 
    }

    public void Refresh_CoinCounter(){
        Coin_Count.GetComponent<Text>().text = localdata.Saved_Coins.ToString();
        Writedata(localdata);
    }

    public void Refresh_PlayerName(){
        Player_Name_Text.GetComponent<Text>().text = Player_Name;
        //Write to save variable
        localdata = loadeddata;
        localdata.Saved_Player_Name = Player_Name; //Neuer Name
        Writedata(localdata);
    }

    public void StartGame(){
        //Load the Game
        SceneManager.LoadScene(sceneName:"Dropoff");
    }

    public void AddBot(){
        if(Menu_Bots < 20)Menu_Bots += 1;
        Bot_Counter.text = Menu_Bots.ToString();
    }

    public void RemoveBot(){
        if(Menu_Bots > 0) Menu_Bots -= 1;
        Bot_Counter.text = Menu_Bots.ToString();
    }
    public void Settings(){
        Player.SetActive(false); //Deaktiviere das Ganze Menü und mache nur die Settings sichtbar
        Shop_Button.SetActive(false);
        Play_Button.SetActive(false);
        Coin_Counter.SetActive(false);
        Emerald_Counter.SetActive(false);
        Player_Name_Button.SetActive(false);
        Player_Name_Text.gameObject.SetActive(false);
        Friends_Button.gameObject.SetActive(false);
        Inv_Button.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Name_Input.SetActive(false);
        Settings_Button.gameObject.SetActive(false);

        Settings_Menu_X.gameObject.SetActive(true);
        Settings_Menu.gameObject.SetActive(true);
    }

    public void SettingsExit(){
        Player.SetActive(true); //Deaktiviere das Ganze Menü und mache nur die Settings sichtbar
        Shop_Button.SetActive(true);
        Play_Button.SetActive(true);
        Coin_Counter.SetActive(true);
        Emerald_Counter.SetActive(true);
        Player_Name_Button.SetActive(true);
        Player_Name_Text.gameObject.SetActive(true);
        Friends_Button.gameObject.SetActive(true);
        Inv_Button.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);
        Name_Input.SetActive(false);
        Settings_Button.gameObject.SetActive(true);
        AddBotButton.SetActive(true);
        RemoveBotButton.SetActive(true);
        Bot_Display.SetActive(true);

        Settings_Menu_X.gameObject.SetActive(false);
        Settings_Menu.gameObject.SetActive(false);

        //und Inventory weil gleicher button genutzt wird
        Inv_Hud.SetActive(false);
        InvBG.SetActive(false);
        activeBG.SetActive(true);
        X_Inv.SetActive(false);
        Leveltext.gameObject.SetActive(true);
        ProgressBar.gameObject.SetActive(true);
        ProgressBarBG.gameObject.SetActive(true);
        neededXP.gameObject.SetActive(true);
        ShopBG.gameObject.SetActive(false);
        //und online
        onlineBG.SetActive(false);
        X_Inv.SetActive(false);
        FriendWindow.SetActive(false);
        IDText.SetActive(false);
        X_Friends.SetActive(false);
        FriendsScroll.SetActive(false);
        FriendListButton.SetActive(false);
        GlobalLeaderboardButton.SetActive(false);
        RankLeaderboardButton.SetActive(false);
        AddFriendInput.SetActive(false);
    }

    public void MusicSettingChanged(float newVolume){
        //Auf die neue Lautstärke aktualisieren
        LobbyMusic.GetComponent<AudioSource>().volume = newVolume;
        //Music in Prozent im Menü anzeigen
        
    }

    public void Refresh_Menu_Skin(){
        switch(loadeddata.SelectedSkin){
            case "Default":
            Menu_Skin_Display.GetComponent<Image>().sprite = DefaultSkinSprite;
            break;

            case "Beta":
            Menu_Skin_Display.GetComponent<Image>().sprite = BetaSkinSprite;
            break;

            case "Agent":
            Menu_Skin_Display.GetComponent<Image>().sprite = AgentSkinSprite;
            break;

            case "Clown":
            Menu_Skin_Display.GetComponent<Image>().sprite = ClownSkinSprite;
            break;

            case "Alien":
            Menu_Skin_Display.GetComponent<Image>().sprite = AlienSkinSprite;
            break;

            case "Chris":
            Menu_Skin_Display.GetComponent<Image>().sprite = ChrisSkinSprite;
            break;

            case "Otto":
            Menu_Skin_Display.GetComponent<Image>().sprite = OttoSkinSprite;
            break;

            default:
            //Debug.Log("Couldnt find Skin you search for");
            break;

        }
    }
    public void Skinbutton(string Skinname){
        //Debug.Log(Skinname);
        localdata = loadeddata;
        //alle einmal abfragen weil ich keine lust habe beim Daten lesen alle in eine array zu schreiben
        if(Skinname == "Default"){
            if(loadeddata.Default_Ownage == true)localdata.SelectedSkin = "Default";
        }else if(Skinname == "Beta"){
            if(loadeddata.Beta_Ownage == true) localdata.SelectedSkin = "Beta";
        }else if(Skinname == "Agent"){
            if(loadeddata.Agent_Ownage == true) localdata.SelectedSkin = "Agent";
        }else if(Skinname == "Clown"){
            if(loadeddata.Clown_Ownage == true) localdata.SelectedSkin = "Clown";
        }else if(Skinname == "Alien"){
            if(loadeddata.Alien_Ownage == true) localdata.SelectedSkin = "Alien";
        }else if(Skinname == "Chris"){
            if(loadeddata.Chris_Ownage == true) localdata.SelectedSkin = "Chris";
        }else if(Skinname == "Otto"){
            if(loadeddata.Otto_Ownage == true) localdata.SelectedSkin = "Otto";
        }
        //Write to Savefile
        StartCoroutine(InitUserData2());
    }
    public IEnumerator InitUserData2(){
        Writedata(localdata);
        //Debug.Log("Write...");
        yield return new WaitForEndOfFrame();
        Readdata();
        //Debug.Log("Read...");
        //Refresh Skin buttons
        InitSkinButtons();
        //Write New Skin to Database
        StartCoroutine(OnlineManager.UpdateSelectedSkin());
    }
    public void InitSkinButtons(){
        //Debug.Log("Stell Farbe um");
        //Default Skin
        if(loadeddata.Default_Ownage == true){//Wenn Skin in besitz dann auf rot und gaanz am ende wird der der selected ist auf grün geschaltet
            DefaultSkinButton.sprite = DefaultSkinButtonRed;
        }else{ //und wenn nicht im besitz mach grau
            DefaultSkinButton.sprite = DefaultSkinButtonGray;
        }
        //Beta
        if(loadeddata.Beta_Ownage == true){
            BetaSkinButton.sprite = BetaSkinButtonRed;
        }else{
            BetaSkinButton.sprite = BetaSkinButtonGray;
        }
        //Agent
        if(loadeddata.Agent_Ownage == true){
            AgentSkinButton.sprite = AgentSkinButtonRed;
        }else{
            AgentSkinButton.sprite = AgentSkinButtonGray;
        }
        //Clown
        if(loadeddata.Clown_Ownage == true){
            ClownSkinButton.sprite = ClownSkinButtonRed;
        }else{
            ClownSkinButton.sprite = ClownSkinButtonGray;
        }
        //Otto
        if(loadeddata.Otto_Ownage == true){
            OttoSkinButton.sprite = OttoSkinButtonRed;
        }else{
            OttoSkinButton.sprite = OttoSkinButtonGray;
        }
        //Alien
        if(loadeddata.Alien_Ownage == true){
            AlienSkinButton.sprite = AlienSkinButtonRed;
        }else{
            AlienSkinButton.sprite = AlienSkinButtonGray;
        }
        //Chris
        if(loadeddata.Chris_Ownage == true){
            ChrisSkinButton.sprite = ChrisSkinButtonRed;
        }else{
            ChrisSkinButton.sprite = ChrisSkinButtonGray;
        }
        //Debug.Log(loadeddata.SelectedSkin);
        //Schalte den der gerade equiped ist auf grün (immernoch machbar mit arrays aber keine lust den grund code umzuschreiben (wegen Problemen beim Speichern/Lesen))
        if(loadeddata.SelectedSkin == "Default"){
            DefaultSkinButton.sprite = DefaultSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = DefaultDispalayModel;
        } 
        else if(loadeddata.SelectedSkin == "Beta"){
            BetaSkinButton.sprite = BetaSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = BetaDisplayModel;
        }  
        else if(loadeddata.SelectedSkin == "Agent"){
            AgentSkinButton.sprite = AgentSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = AgentDisplayModel;
        }
        else if(loadeddata.SelectedSkin == "Clown"){
            ClownSkinButton.sprite = ClownSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite =   ClownDisplayModel;
        } 
        else if(loadeddata.SelectedSkin == "Otto"){
            OttoSkinButton.sprite = OttoSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = OttoDisplayModel;
        } 
        else if(loadeddata.SelectedSkin == "Chris"){
            ChrisSkinButton.sprite = ChrisSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = ChrisDisplayModel;
        } 
        else if(loadeddata.SelectedSkin == "Alien"){
            AlienSkinButton.sprite = AlienSkinButtonGreen;
            Menu_Skin_Display.GetComponent<Image>().sprite = AlienDisplayModel;
        } 
    }

}