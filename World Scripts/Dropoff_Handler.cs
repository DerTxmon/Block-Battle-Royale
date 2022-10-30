using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dropoff_Handler : MonoBehaviour
{
    public static float DropoffX;
    public static float DropoffY;
    [SerializeField] private GameObject Cross;
    public GameObject CrossParent;
    [SerializeField] private GameObject Deploy_Button;
    public bool Dropable = true;
    [SerializeField] private Camera PrevCam;

    private void Update() {
        DropoffX = CrossParent.GetComponent<RectTransform>().localPosition.x * 1.127850267379679f /*UI to World umrechnung*/;
        DropoffY = CrossParent.GetComponent<RectTransform>().localPosition.y * 1.127850267379679f /*UI to World umrechnung*/;
        //Preview Cam anpassen
        PrevCam.transform.position = new Vector3(DropoffX + 700f, DropoffY, -100);
    }

    public void DropButton(){
        if(Dropable){
            //Blende Black Screen ein
            //Lade das Game
            SceneManager.LoadScene(sceneName:"Game");
        }
    }
}
