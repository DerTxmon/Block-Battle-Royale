using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Destroy : MonoBehaviour
{
    public GameObject player;
    private Inventory_Handler inv_handler;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); 
        //player.GetComponent<Inventory_Handler>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player" && collision.GetComponent<Inventory_Handler>().lootcount < 3 || collision.gameObject.tag == "Bot" && collision.GetComponent<Bot_Inventory>().lootcount < 3){
            Destroy(this.gameObject);
        }
    }
}