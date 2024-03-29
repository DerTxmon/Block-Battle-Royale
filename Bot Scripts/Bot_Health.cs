using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bot_Health : MonoBehaviour
{
    public GameObject Player;
    public float health;
    public float Maxhealth;
    public GameObject World, Healthbar, Bot;
    private Bot_Inventory Inv;
    public float prcent = 7.97f, healthpercent, displayhealth; //1 Prozent der healthbar in units
    public GameObject smallammo, midammo, bigammo, M4, Ak, Glock, Sniper;
    private GameObject item;
    public bool InZone = false;
    // Start is called before the first frame update
    void Start()
    {
        health = 200f;
        Maxhealth = 200f;
        Bot = this.gameObject;
        Inv = Bot.GetComponent<Bot_Inventory>();
        StartCoroutine(ZoneDamage());
        Player = GameObject.Find("Player");
    }

    public void Damage(int Dmg, string oponent, int WeaponID){
        health -= Dmg;
        //Check ob tod
        if(health <= 0){
            DropInv(this.gameObject.transform.position);
            if(oponent == "Player") Player.GetComponent<Shoot>().CallKillfeedFromBot(Bot.GetComponentInChildren<TextMeshPro>().text, "Player", WeaponID);
            else if(oponent == "Zone") Player.GetComponent<Shoot>().CallKillfeedFromBot(Bot.GetComponentInChildren<TextMeshPro>().text, "Zone", WeaponID);
            else Player.GetComponent<Shoot>().CallKillfeedFromBot(Bot.GetComponentInChildren<TextMeshPro>().text, oponent, WeaponID);
            
            Destroy(this.gameObject);
        }
        //Healthbar aktualisieren
        healthpercent = health / 2; // weil wir 200 health haben und wenn wir durch 200 teilen haben wir sofort die prozent
        displayhealth = healthpercent * prcent; //aktuelle leben in % * 1%
        Healthbar.GetComponent<Transform>().localScale = new Vector3(displayhealth, Healthbar.GetComponent<Transform>().localScale.y, Healthbar.GetComponent<Transform>().localScale.z); //x achse weil die bar um 90 grad gedreht ist
    }
    IEnumerator ZoneDamage(){
        while(true){
            if(InZone) Damage(Zone_Manager.instance.currentzonedamage, "Zone", 6);
            yield return new WaitForSeconds(Zone_Manager.instance.Zoneticktime);
        }
    }

    private IEnumerator TESTDeath(){
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(6f); //Bis Killfeed weg geht
        Destroy(this.gameObject);
    }

    private void DropInv(Vector2 Pos){
        //Drop Ammo
        //Small Ammo
        int randx = Random.Range(0,4);
        int randy = Random.Range(0,4);
        if(Bot.GetComponent<Bot_Inventory>().small_ammo > 0){
            item = Instantiate(smallammo, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
            item.GetComponent<Ammo_Info>().Ammo = Bot.GetComponent<Bot_Inventory>().small_ammo; 
        }
        //Mid Ammo
        randx = Random.Range(0,4);
        randy = Random.Range(0,4);
        if(Bot.GetComponent<Bot_Inventory>().mid_ammo > 0){
            item = Instantiate(midammo, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
            item.GetComponent<Ammo_Info>().Ammo = Bot.GetComponent<Bot_Inventory>().mid_ammo; 
        }
        //Big Ammo
        randx = Random.Range(0,4);
        randy = Random.Range(0,4);
        if(Bot.GetComponent<Bot_Inventory>().big_ammo > 0){
            item = Instantiate(bigammo, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
            item.GetComponent<Ammo_Info>().Ammo = Bot.GetComponent<Bot_Inventory>().big_ammo; 
        }
        //Weapons
        int iterations = Inv.lootcount + 1; //Wir setzen iterations und i auf 1 damit wir in der loop i direkt für die inv slots nutzen können
        for(int i = 1; i < iterations; i++){
            if(i == 1){
                //Random offset
                randx = Random.Range(0,4);
                randy = Random.Range(0,4);
                //Drop Slot 1
                if(Inv.Slot1_Item == "Glock_18"){
                    item = Instantiate(Glock, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "M4"){
                    item = Instantiate(M4, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "Ak47"){
                    item = Instantiate(Ak, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "Sniper"){
                    item = Instantiate(Sniper, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }
            }
            else if(i == 2){
                //Random offset
                randx = Random.Range(0,4);
                randy = Random.Range(0,4);
                //Drop Slot 1
                if(Inv.Slot1_Item == "Glock_18"){
                    item = Instantiate(Glock, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "M4"){
                    item = Instantiate(M4, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "Ak47"){
                    item = Instantiate(Ak, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "Sniper"){
                    item = Instantiate(Sniper, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }
            }
            else if(i == 3){
                //Random offset
                randx = Random.Range(0,4);
                randy = Random.Range(0,4);
                //Drop Slot 1
                if(Inv.Slot1_Item == "Glock_18"){
                    item = Instantiate(Glock, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "M4"){
                    item = Instantiate(M4, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "Ak47"){
                    item = Instantiate(Ak, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }else if(Inv.Slot1_Item == "Sniper"){
                    item = Instantiate(Sniper, new Vector3(Pos.x + randx, Pos.y + randy, 0f), Quaternion.identity);
                }
            }
        }
    }
}
