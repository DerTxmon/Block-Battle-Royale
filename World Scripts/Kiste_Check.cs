using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiste_Check : MonoBehaviour
{
    public bool Contact, open;
    [SerializeField] GameObject Lootbtn;
    
    private void Awake(){
        Lootbtn = GameObject.Find("Loot");
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Player" || collision.tag == "Bot"){
            try{
                collision.gameObject.GetComponent<Movement>().lootable = true;
            }catch{
                collision.gameObject.GetComponent<Bot_Behavior>().lootable = true;
            }
            Contact = true;
            try{
                if(collision.gameObject.GetComponent<Movement>().lootbutton == true){
                    //Zu Checken: isopen, Collision, lootbutton.
                    if(transform.GetComponentInParent<Kiste>().isopen == false){
                        //Öffne die kiste (führe die öffnungs funktion im Kisten Script selber aus)
                        StartCoroutine(gameObject.GetComponentInParent<Kiste>().Open());
                    }
                }
            }catch{
                if(collision.gameObject.GetComponent<Bot_Behavior>().lootbutton == true) open = true;
            }
        }else Contact = false;
        if(collision.gameObject.tag == "Player" && this.gameObject.GetComponentInParent<Kiste>().isopen == false){ //und kiste noch nicht geöffnet
            Lootbtn.GetComponent<Image>().enabled = true;
            Lootbtn.GetComponent<Button>().enabled = true;
        }else if(collision.gameObject.tag == "Player" && this.gameObject.GetComponentInParent<Kiste>().isopen == true){
            Lootbtn.GetComponent<Image>().enabled = false;
            Lootbtn.GetComponent<Button>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "Player" || collision.tag == "Bot"){
            try{
                collision.gameObject.GetComponent<Movement>().lootable = false;
            }catch{
                collision.gameObject.GetComponent<Bot_Behavior>().lootable = false;
            }
            //Lootbutton
            if(collision.gameObject.tag == "Player"){
                Lootbtn.GetComponent<Image>().enabled = false;
                Lootbtn.GetComponent<Button>().enabled = false;
            }
            Contact = false;
            open = false;
        }
    }
}
