using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
public class Map_Manager : MonoBehaviour
{
    public int Playercount;
    private GameObject[] x;
    private int y = 1;
    public GameObject[] EntryPoints;
    public float Timespeed;
    public GameObject GlobalLight;
    public GameObject[] Laternen;
    public GameObject[] Waffen_Spawns;
    private GameObject[] Ammo_Spawns;
    [SerializeField] private GameObject Glock;
    [SerializeField] private GameObject M4;
    [SerializeField] private GameObject Sniper;
    [SerializeField] private GameObject AK;
    [SerializeField] private GameObject Small_Ammo;
    [SerializeField] private GameObject Mid_Ammo;
    [SerializeField] private GameObject Big_Ammo;

    void Awake(){
        //Registriere alle Laternen in eine Array
        Laternen = GameObject.FindGameObjectsWithTag("Laternen");
        //Mache alle Lichter aus
        TurnOffLights();
        //Rotiert durch Tag und nacht durch
        StartCoroutine(WorldTime());
        Waffen_Spawns = GameObject.FindGameObjectsWithTag("Waffen_Spawn"); //für SpawnItems()
        SpawnItems();
    }
    void Start()
    {
        StartCoroutine(PlayerCheck());

        EntryPoints = GameObject.FindGameObjectsWithTag("Entry");
        foreach(GameObject i in EntryPoints){
            i.GetComponent<Entry>().id = y;
            y++;
        }
        //reset "y" für Update()
        y = 0;
    }

    private IEnumerator WorldTime(){
        while(true){
            //Es wird Nacht
            while(GlobalLight.GetComponent<Light2D>().intensity > .15f){
                GlobalLight.GetComponent<Light2D>().intensity -= .0075f;
                //Mach Lichter AN wenn Abend (.5f) erreicht wird
                if(GlobalLight.GetComponent<Light2D>().intensity < .5f) TurnOnLights();
                yield return new WaitForSeconds(Timespeed);
            }
            //Es wird Tag
            while(GlobalLight.GetComponent<Light2D>().intensity < .9f){
                GlobalLight.GetComponent<Light2D>().intensity += .015f;
                //Mach Lichter AUS wenn Abend (.5f) erreicht wird
                if(GlobalLight.GetComponent<Light2D>().intensity > .5f) TurnOffLights();
                yield return new WaitForSeconds(Timespeed);
            }
        }
    }

    private void TurnOnLights(){
        foreach(GameObject i in Laternen){
            i.GetComponentInChildren<Light2D>().enabled = true;
        }
    }

    private void TurnOffLights(){
        foreach(GameObject i in Laternen){
            i.GetComponentInChildren<Light2D>().enabled = false;
        }
    }

    IEnumerator PlayerCheck(){
        while(true){
        x =  GameObject.FindGameObjectsWithTag("Bot");
        Playercount = x.Length;
        x = GameObject.FindGameObjectsWithTag("Player");
        Playercount += x.Length;
        GameObject.Find("PlayerCount").GetComponent<Text>().text = Playercount.ToString();
        yield return new WaitForSeconds(5f);
        }
    }

    private void SpawnItems(){
        //Waffen
        foreach(GameObject i in Waffen_Spawns){
            int rand = Random.Range(1,3); //(Kann nur zwischen 1 und 2 treffen) Rechne die chance ob überhaupt auf diesem spot irgendwas spawnen soll (50/50)
            if(rand == 1){
                //Rechne die chancen für die Spawnende Waffe aus
                rand = Random.Range(-1,104);//0-100

                if(rand < 41 && rand > -1){//0-40 (40%)
                //Glock
                    GameObject item = Instantiate(Glock, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 12;
                    item = Instantiate(Small_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 20;
                //Small Ammo
                    //GameObject item = 
                }else if(rand < 52 && rand > 40){ //41-51 (10%)
                //Sniper
                    GameObject item = Instantiate(Sniper, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 5;
                    item = Instantiate(Big_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 2;
                }else if(rand < 83 && rand > 51){ //52-82 (30%)
                //M4
                    GameObject item = Instantiate(M4, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 30;
                    item = Instantiate(Mid_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 20;
                }else if(rand < 104 && rand > 82){ //83-103 (20%)
                //AK
                    GameObject item = Instantiate(AK, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 25;
                    item = Instantiate(Mid_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 10;
                }
            }
        }
    }
}
