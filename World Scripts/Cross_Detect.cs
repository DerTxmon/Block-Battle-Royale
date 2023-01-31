using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cross_Detect : MonoBehaviour
{
    [SerializeField] Camera MainCam;
    public GameObject DeployButton;
    public Dropoff_Handler drphandler;
    public Sprite Red_Button;
    public Sprite Green_Button;

    private void Awake() {
        drphandler = MainCam.GetComponent<Dropoff_Handler>();
        drphandler.Dropable = true;
    }
    private void OnTriggerStay2D(Collider2D collision) {
        //Update den Deploy button
        if(collision.gameObject.tag == "DontDropHere"){
            DeployButton.GetComponent<Image>().sprite = Red_Button;
            drphandler.Dropable = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
       //Update den Deploy button
        if(collision.gameObject.tag == "DontDropHere"){
            DeployButton.GetComponent<Image>().sprite = Green_Button;
            drphandler.Dropable = true;
        } 
    }
}
