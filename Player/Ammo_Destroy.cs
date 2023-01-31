using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Destroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) { 
     if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bot"){ 
        Destroy(this.gameObject);
    }
    }
}
