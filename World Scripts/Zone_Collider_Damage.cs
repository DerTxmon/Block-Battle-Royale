using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone_Collider_Damage : MonoBehaviour
{
    public GameObject Zone;

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player" && Zone.GetComponent<Zone_Manager>().TakingDamage == false){
            Zone.GetComponent<Zone_Manager>().InZone = true;
        }else if(collision.gameObject.tag == "Bot"){
            collision.gameObject.GetComponent<Bot_Health>().InZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Zone.GetComponent<Zone_Manager>().InZone = false;
        }else if(collision.gameObject.tag == "Bot"){
            collision.gameObject.GetComponent<Bot_Health>().InZone = false;
        }
    }
    
}
