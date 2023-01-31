using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bot"){
            collision.gameObject.GetComponent<Bot_Behavior>().EntryPoint = null;
            collision.gameObject.GetComponentInChildren<Bot_PlayerContact>().Entry2 = false;
        }
    }
}
