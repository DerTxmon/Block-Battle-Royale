using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kisten_Spawn : MonoBehaviour
{
    public GameObject[] Kiste_Obenarr, Kiste_Untenarr, Kiste_Rechtsarr, Kiste_Linksarr;
    public GameObject Kiste_Oben, Kiste_Unten, Kiste_Rechts, Kiste_Links;
    void Awake()
    {
        Kiste_Untenarr =  GameObject.FindGameObjectsWithTag("Kiste_Unten");
        Kiste_Obenarr = GameObject.FindGameObjectsWithTag("Kiste_Oben");
        Kiste_Linksarr = GameObject.FindGameObjectsWithTag("Kiste_Links");
        Kiste_Rechtsarr = GameObject.FindGameObjectsWithTag("Kiste_Rechts");

        foreach(GameObject i in Kiste_Untenarr){
            int x = Random.Range(1, 5);
            if(x <= 2){ //40% auf Kisten Spawn
                Instantiate(Kiste_Unten, new Vector3(i.transform.position.x, i.transform.position.y, 120f), new Quaternion(0,0,0,0));
            }
        }

        foreach(GameObject i in Kiste_Obenarr){
            int x = Random.Range(1, 5);
            if(x <= 2){ //40% auf Kisten Spawn
                Instantiate(Kiste_Oben, new Vector3(i.transform.position.x, i.transform.position.y, 120f), new Quaternion(0,0,180f,0));
            }
        }

        foreach(GameObject i in Kiste_Linksarr){
            int x = Random.Range(1, 5);
            if(x <= 2){ //40% auf Kisten Spawn
                Instantiate(Kiste_Links, new Vector3(i.transform.position.x, i.transform.position.y, 120f), i.transform.rotation * new Quaternion(0,0,180f,0));
            }
        }

        foreach(GameObject i in Kiste_Rechtsarr){
            int x = Random.Range(1, 5);
            if(x <= 2){ //40% auf Kisten Spawn
                Instantiate(Kiste_Rechts, new Vector3(i.transform.position.x, i.transform.position.y, 120f), i.transform.rotation);
            }
        }
    }
}
