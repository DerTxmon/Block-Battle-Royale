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

    public void Start(){
       //AllrenderableGameObjects = FindObjectsOfType<Render_Manager>().Select(rm => rm.gameObject).ToArray();
       Menu = gameObject.GetComponent<Menu_Handler>();
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
    public void EndScreen(){
        //Save Stats to save.json
        Menu.localdata.Kills = Menu.loadeddata.Kills + Inv.Kills; //Scenen Übergreifend. Muss statisch sein. Workaround needed
        Menu.localdata.Saved_Coins = Menu.loadeddata.Saved_Coins + Inv.Kills;
        Menu.Writedata();
    }
}
