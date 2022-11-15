using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Visibility : MonoBehaviour
{
    public GameObject Weapons;
    private bool h;

    // Start is called before the first frame update
    void Start()
    {
        Weapons = GameObject.Find("Weapons_Sprites");
        //Alle Waffen am anfang des Games verstecken.
        Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
        Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
        Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
        Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
    }

    public void Checkvisibility(){
        //Glock
        if(gameObject.GetComponent<Inventory_Handler>().Glock_18_Selected == true){
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(true);
            //Slow down
            gameObject.GetComponent<Movement>().speed = 7.5f;
            h = false;
            goto end;
        }else if(gameObject.GetComponent<Inventory_Handler>().Glock_18_Selected == false){
            Weapons.transform.Find("Glock_18_Top_Sprite").gameObject.SetActive(false);
            h = true;
        }
        //M4
        if(gameObject.GetComponent<Inventory_Handler>().M4_Selected == true){
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(true);
            //Slow down
            gameObject.GetComponent<Movement>().speed = 6f;
            h = false;
            goto end;
        }else if(gameObject.GetComponent<Inventory_Handler>().M4_Selected == false){
            Weapons.transform.Find("M4_Top_Sprite").gameObject.SetActive(false);
            h = true;
        }
        //Ak
        if(gameObject.GetComponent<Inventory_Handler>().Ak47_Selected == true){
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(true);
            //Slow down
            gameObject.GetComponent<Movement>().speed = 6f;
            h = false;
            goto end;
        }else if(gameObject.GetComponent<Inventory_Handler>().Ak47_Selected == false){
            Weapons.transform.Find("Ak47_Top_Sprite").gameObject.SetActive(false);
            h = true;
        }
        //Sniper
        if(gameObject.GetComponent<Inventory_Handler>().Sniper_Selected == true){
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(true);
            //Slow down
            gameObject.GetComponent<Movement>().speed = 4f;
            h = false;
            goto end;
        }else if(gameObject.GetComponent<Inventory_Handler>().Sniper_Selected == false){
            Weapons.transform.Find("Sniper_Top_Sprite").gameObject.SetActive(false);
            h = true;
        }
        end:
        //Check ob auf default gesetzt werden soll
        if(h) gameObject.GetComponent<Movement>().speed = 8f; //Default
        h = false; //reset h wieder für nächsten check
    }
}