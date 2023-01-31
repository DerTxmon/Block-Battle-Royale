using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Optimazation : MonoBehaviour
{
    [SerializeField] private GameObject Playercheck;
    public List<GameObject> enemy_bots = new List<GameObject>();
    public GameObject Player;
    public bool is_object_in_range;
    private GameObject[] Kisten_Unten;
    private GameObject[] Kisten_Oben;
    private GameObject[] Kisten_Rechts;
    private GameObject[] Kisten_Links;
    private GameObject[] Gebäude;
    private List<GameObject> Items = new List<GameObject>();

    void Start(){
        Player = GameObject.Find("Player");
        StartCoroutine(Getallbots());
        //Alle Kisten in eine Liste packen.
        Kisten_Unten = GameObject.FindGameObjectsWithTag("Kiste_Unten");
        Kisten_Oben = GameObject.FindGameObjectsWithTag("Kiste_Oben");
        Kisten_Rechts = GameObject.FindGameObjectsWithTag("Kiste_Rechts");
        Kisten_Links = GameObject.FindGameObjectsWithTag("Kiste_Links");
        Gebäude = GameObject.FindGameObjectsWithTag("Gebäude");


        StartCoroutine(RegisterItems());
        StartCoroutine(Check());        
    }
    private IEnumerator Getallbots(){
        yield return new WaitForSeconds(5f);
        //Setze alle Bots auf die Liste (Außer sich selber). Der Spieler wird nicht in die Liste aufgenommen sondern einzeld bearbeitet.
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Bot")){
            if(i.Equals(this.gameObject)){
                continue;
            }
            enemy_bots.Add(i);
        }
    }

    private IEnumerator Check(){
        while(true){
            //Prüfe alle Bots auf der Liste auf die entfernung um dann das Player contact script an oder aus zu schalten.
            //Bots
            foreach(GameObject bot in enemy_bots){
                if(bot != null){
                    if(Vector2.Distance(bot.transform.position, this.gameObject.transform.position) < 30f){
                        is_object_in_range = true;
                        goto end;
                    }else{
                        is_object_in_range = false;
                    }
                }
            }
            //Player
            if(Vector2.Distance(Player.transform.position, this.gameObject.transform.position) < 75f){
                is_object_in_range = true;
                goto end;
            }else{
                is_object_in_range = false;
            }
            //Kisten
            foreach(GameObject kiste in Kisten_Unten){
                if(Vector2.Distance(kiste.transform.position, this.gameObject.transform.position) < 45f){
                    is_object_in_range = true;
                    goto end;
                }else{
                    is_object_in_range = false;
                }
            }
            foreach(GameObject kiste in Kisten_Oben){
                if(Vector2.Distance(kiste.transform.position, this.gameObject.transform.position) < 45f){
                    is_object_in_range = true;
                    goto end;
                }else{
                    is_object_in_range = false;
                }
            }
            foreach(GameObject kiste in Kisten_Rechts){
                if(Vector2.Distance(kiste.transform.position, this.gameObject.transform.position) < 45f){
                    is_object_in_range = true;
                    goto end;
                }else{
                    is_object_in_range = false;
                }
            }
            foreach(GameObject kiste in Kisten_Links){
                if(Vector2.Distance(kiste.transform.position, this.gameObject.transform.position) < 45f){
                    is_object_in_range = true;
                    goto end;
                }else{
                    is_object_in_range = false;
                }
            }
            //Gebäude
            foreach(GameObject gebäude in Gebäude){
                if(Vector2.Distance(gebäude.transform.position, this.gameObject.transform.position) < 80f){
                    is_object_in_range = true;
                    goto end;
                }else{
                    is_object_in_range = false;
                }
            }
            //Items
            foreach(GameObject item in Items){
                if(item != null){
                    if(Vector2.Distance(item.transform.position, this.gameObject.transform.position) < 30f){
                        is_object_in_range = true;
                        goto end;
                    }else{
                        is_object_in_range = false;
                    }
                }
            }
            end:
            if(is_object_in_range){
                Playercheck.GetComponent<Bot_PlayerContact>().enabled = true;
            }else{
                Playercheck.GetComponent<Bot_PlayerContact>().enabled = false;
            }
            yield return new WaitForSeconds(2.5f);
        }
    }

    private IEnumerator RegisterItems(){
        //Wird alle 15 Sekunden ausgeführt und registriert alle Items in eine Liste.
        //Alle Glocks
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Glock_18")){
            Items.Add(item);
        }
        //Alle M4
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("M4")){
            Items.Add(item);
        }
        //Alle AK-47
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("AK_47")){
            Items.Add(item);
        }
        //Alle Sniper
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Sniper")){
            Items.Add(item);
        }
        //Alle Heals
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Heal")){
            Items.Add(item);
        }
        //Big, Mid und Small Ammo
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Big_Ammo")){
            Items.Add(item);
        }
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Mid_Ammo")){
            Items.Add(item);
        }
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Small_Ammo")){
            Items.Add(item);
        }
        yield return new WaitForSeconds(15f);
    }
}
