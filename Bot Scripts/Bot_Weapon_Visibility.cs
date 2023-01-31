using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Weapon_Visibility : MonoBehaviour
{
    public GameObject Glock_18_Top;
    public GameObject M4_Top;
    public GameObject Ak47_Top;
    public GameObject Sniper_Top;
    // Start is called before the first frame update
    void Start()
    {
        //Alle Waffen am anfang des Games verstecken.
        Glock_18_Top.transform.gameObject.SetActive(false);
        M4_Top.transform.gameObject.SetActive(false);
        Ak47_Top.transform.gameObject.SetActive(false);
        Sniper_Top.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*Glock_18visibility();
        M4visibility();
        Ak47visibility();
        Snipervisibility();*/
    }

//MUSS FÜR BOT INV CHECK UMGESCHRIBEN WERDEN
    public void Glock_18visibility(){
        if(gameObject.GetComponent<Inventory_Handler>().Glock_18_Selected == true){
            Glock_18_Top.transform.gameObject.SetActive(true);
        }else{
            Glock_18_Top.transform.gameObject.SetActive(false);
        }
    }

    public void M4visibility(){
        if(gameObject.GetComponent<Inventory_Handler>().M4_Selected == true){
            M4_Top.transform.gameObject.SetActive(true);
        }else{
            M4_Top.transform.gameObject.SetActive(false);
        }
    }

    public void Ak47visibility(){
        if(gameObject.GetComponent<Inventory_Handler>().Ak47_Selected == true){
            Ak47_Top.transform.gameObject.SetActive(true);
        }else{
            Ak47_Top.transform.gameObject.SetActive(false);
        }
    }

    public void Snipervisibility(){
        if(gameObject.GetComponent<Inventory_Handler>().Sniper_Selected == true){
            Sniper_Top.transform.gameObject.SetActive(true);
        }else{
            Sniper_Top.transform.gameObject.SetActive(false);
        }
    }
}
