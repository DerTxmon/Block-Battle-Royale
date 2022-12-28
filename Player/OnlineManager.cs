using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class OnlineManager : MonoBehaviour
{
    public static string[] RecievedData; //Vor zugriff muss GetAllUserData() gcallt werden um die die daten aus der datenbank zu bekommen //[0] = Nickname [1] = SelectedSkin [2] = Coins [3] = Emeralds [4] = Wins [5] = Kills
    public static string[][] GlobalBest = new string[5][]; //20 Spiele mit jeweils 7 Datensätzen
    public static int NewID = 0;
    public static int FriendExistance;

    private void Awake() {
        //init GlobalBest Array
        for(int i = 0; i != 4; i++){
            GlobalBest[i] = new string[7];
        }
    }
    public static IEnumerator RegisterNewUserOnDatabase(){
        WWWForm form = new WWWForm();
        form.AddField("Register", "1");
        form.AddField("Nickname", Menu_Handler.loadeddata.Saved_Player_Name);
        form.AddField("SelectedSkin", Menu_Handler.loadeddata.SelectedSkin);
        form.AddField("Coins", Menu_Handler.loadeddata.Saved_Coins);
        form.AddField("Emeralds", Menu_Handler.loadeddata.Saved_Emeralds);
        form.AddField("Wins", Menu_Handler.loadeddata.Wins);
        form.AddField("Kills", Menu_Handler.loadeddata.Kills);
        form.AddField("Level", Menu_Handler.loadeddata.Level);
        Debug.Log("Loaded Level:" + Menu_Handler.loadeddata.Level);
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer(); //neuer download buffer weil sonst bug
        www.downloadHandler = dh; //download buffer zuweisen
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        if((www.result == UnityWebRequest.Result.ConnectionError)){ //schau ob error
            Debug.Log("SERVER CONNECTION ERROR");
        }else{
            Debug.Log(www.downloadHandler.text);
            //Verarbeite Rückgabe vom Server
            RecievedData = www.downloadHandler.text.Split("\t");
            NewID = Int32.Parse(RecievedData[0]);
        }
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
    public static IEnumerator GetAllUserData(string id){
        WWWForm form = new WWWForm();
        form.AddField("getuserdata", "1");
        form.AddField("id", id); //User Daten zum suchen in der Datenbank
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer(); //neuer download buffer weil sonst bug
        www.downloadHandler = dh; //download buffer zuweisen
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        if((www.result == UnityWebRequest.Result.ConnectionError)){ //schau ob error
            Debug.Log("SERVER CONNECTION ERROR");
            yield return null;
        }else{
            //Macht eine Array aus den bekommenen Daten
            //[0] = Nickname [1] = SelectedSkin [2] = Coins [3] = Emeralds [4] = Wins [5] = Kills [6] = Level
            RecievedData = www.downloadHandler.text.Split("\t");
        }
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
    public static IEnumerator CheckforExistence(string id){
        FriendExistance = 0; //waiting for Internet
        WWWForm form = new WWWForm();
        form.AddField("checkforexistance", "1");
        form.AddField("id", id); //User Daten zum suchen in der Datenbank
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer(); //neuer download buffer weil sonst bug
        www.downloadHandler = dh; //download buffer zuweisen
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        Debug.Log("Web Request Send");
        if((www.result == UnityWebRequest.Result.ConnectionError)){ //schau ob error
            Debug.Log("SERVER CONNECTION ERROR");
            yield return null;
        }else{
            if(www.downloadHandler.text == "FriendExists"){
                FriendExistance = 1; //found
                Debug.Log("Found");
            }else{
                FriendExistance = 2; //not found
                Debug.Log("NOT Found");
            }
        }
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
    public static IEnumerator UpdateUserData(){
        WWWForm form = new WWWForm();
        //Daten Aktualisieren
        form.AddField("Update","1");
        form.AddField("Nickname", Menu_Handler.loadeddata.Saved_Player_Name);
        form.AddField("SelectedSkin", Menu_Handler.loadeddata.SelectedSkin);
        form.AddField("Coins", Menu_Handler.loadeddata.Saved_Coins);
        form.AddField("Emeralds", Menu_Handler.loadeddata.Saved_Emeralds);
        form.AddField("Wins", Menu_Handler.loadeddata.Wins);
        form.AddField("Kills", Menu_Handler.loadeddata.Kills);
        form.AddField("Level", Menu_Handler.loadeddata.Level);
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
    public static IEnumerator UpdateNickname(){
        WWWForm form = new WWWForm();
        //Daten Aktualisieren
        form.AddField("UpdateNickname","1");
        form.AddField("id", Menu_Handler.loadeddata.PlayerID);
        form.AddField("Nickname", Menu_Handler.loadeddata.Saved_Player_Name);
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        if((www.result == UnityWebRequest.Result.ConnectionError)){ //schau ob error
            Debug.Log("SERVER CONNECTION ERROR");
        }else{
            Debug.Log(www.downloadHandler.text);
            //Verarbeite Rückgabe vom Server
        }
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
    public static IEnumerator UpdateAfterGame(){
        WWWForm form = new WWWForm();
        //Daten Aktualisieren
        form.AddField("updateaftergame", "1");
        form.AddField("Coins", Menu_Handler.loadeddata.Saved_Coins);
        form.AddField("Emeralds", Menu_Handler.loadeddata.Saved_Emeralds);
        form.AddField("Wins", Menu_Handler.loadeddata.Wins);
        form.AddField("Kills", Menu_Handler.loadeddata.Kills);
        form.AddField("Level", Menu_Handler.loadeddata.Level);
        form.AddField("id", Menu_Handler.loadeddata.PlayerID);
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
    public static IEnumerator UpdateSelectedSkin(){
        WWWForm form = new WWWForm();
        //Daten Aktualisieren
        form.AddField("updateselectedskin", "1");
        form.AddField("id", Menu_Handler.loadeddata.PlayerID);
        form.AddField("SelectedSkin", Menu_Handler.loadeddata.SelectedSkin);
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
    public static IEnumerator GetTop30(){
        //Frage Ganze liste an allen Spielern an.
        //Server Sortiert nach Top 30 und gibt die Platzierung wieder.
        //Zum schluss wird dann die eigene Position errechnet
        WWWForm form = new WWWForm();
        //Daten Aktualisieren
        form.AddField("gettop", "1");
        form.AddField("id", Menu_Handler.loadeddata.PlayerID);
        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.187/BBR/BBR.php", form); //Connect to Webserver
        yield return www.SendWebRequest(); //Warte bis Anfrage fertig ist.
        if((www.result == UnityWebRequest.Result.ConnectionError)){ //schau ob error
            Debug.Log("SERVER CONNECTION ERROR");
        }else{
            RecievedData = www.downloadHandler.text.Split("|");
            int dimension1 = 0;
            int dimension2 = 0;
            foreach(string i in RecievedData){
                Debug.Log(i);                
            }
            /*foreach(string i in RecievedData){ //Teile die eine Array in einzelne Datensätze aus
                GlobalBest[dimension1][dimension2] = i; //hier weiter arbeiten
                dimension2++;
                if(i == "\n"){
                    dimension1++;
                    dimension2 = 0;
                }
            }*/
            Debug.Log(GlobalBest[0][0]);
        }
        www.Dispose(); //Schieße die Connection damit C# keinen Memory leck erleided
    }
}
