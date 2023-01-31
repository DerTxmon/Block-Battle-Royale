using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiste : MonoBehaviour
{
    public GameObject PlayerCheckobj;
    public GameObject Kisteobj;
    private Kiste_Check KisteCheck;
    public bool isopen;
    public GameObject Sniper, M4, Glock, Ak47, Small_Ammo, Mid_Ammo, Big_Ammo, Heal;
    private float Kiste_x_range1, Kiste_x_range2, Kiste_y_range1, Kiste_y_range2;

    // Start is called before the first frame update
    void Start()
    {
        KisteCheck =  PlayerCheckobj.GetComponent<Kiste_Check>();
        //Schaue um welche der 4 kisten (rechts, links, oben, unten) es sich handelt und wechsel dann die 
        //richtungs variable die angibt in welche richtung die items geschmissen werden sollen (in x,y).
        if(gameObject.name == "Kiste_Unten(Clone)"){
            Kiste_x_range1 = -1f;
            Kiste_x_range2 = 1f;
            Kiste_y_range1 = .5f;
            Kiste_y_range2 = 1.5f;
        }else if(gameObject.name == "Kiste_Oben Variant(Clone)"){
            Kiste_x_range1 = -1.5f;
            Kiste_x_range2 = 1.5f;
            Kiste_y_range1 = 1f;
            Kiste_y_range2 = 2f;
        }else if(gameObject.name == "Kiste_Rechts Variant(Clone)"){
            Kiste_x_range1 = 1f;
            Kiste_x_range2 = 2f;
            Kiste_y_range1 = -1f;
            Kiste_y_range2 = 1f;
        }else if(gameObject.name == "Kiste_Links Variant(Clone)"){
            Kiste_x_range1 = 1f;
            Kiste_x_range2 = 2f;
            Kiste_y_range1 = -1f;
            Kiste_y_range2 = 1f;
        }
    }

    public IEnumerator Open(){ // Diese Funktion wird vom Kisten Check aufgerufen
        isopen = true;
        if(gameObject.name == "Kiste_Unten(Clone)" || gameObject.name == "Kiste_Rechts Variant(Clone)"){
            //-------
            //Für Kiste Unten und Kiste Rechts
            //-------
            //2 Waffen spawnen
            for(int i = 0 ;i != 2; i++){
                int weaponnum =  Random.Range(1, 100);
                if(weaponnum >= 1 && weaponnum <= 20){ //20% auf Ak
                    Instantiate(Ak47,  new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(weaponnum > 20 && weaponnum <= 50){ //30% auf M4
                    Instantiate(M4,  new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(weaponnum > 50 && weaponnum <= 60){ //10% auf Sniper
                    Instantiate(Sniper,  new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(weaponnum > 60 && weaponnum <= 100){ //40% auf Glock
                    Instantiate(Glock, new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }
                yield return new WaitForSeconds(0.01f);
            }
            //2 Munitionsstapel Spawnen
            for(int i = 0 ;i != 2; i++){
                int Ammonum =  Random.Range(1, 3);
                if(Ammonum == 1){ //33% auf Small ammo
                    Instantiate(Small_Ammo,  new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(Ammonum == 2){ //33% auf Mid ammo
                    Instantiate(Mid_Ammo, new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(Ammonum == 3){ //33% auf Big ammo
                    Instantiate(Big_Ammo,  new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }
                yield return new WaitForSeconds(0.01f);
            }
            //Heal Spawnen
            int healnum =  Random.Range(0, 4);
            if(healnum == 3){ //Kleine Prozent Chance das Heal gespawned wird
                Instantiate(Heal,  new Vector3(Kisteobj.transform.position.x + Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y - Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
            }
        }else if(gameObject.name == "Kiste_Oben Variant(Clone)" || gameObject.name == "Kiste_Links Variant(Clone)"){
            //-------
            //Für Kiste Oben und Kiste Links
            //-------
            for(int i = 0 ;i != 2; i++){
                int weaponnum =  Random.Range(1, 100);
                if(weaponnum >= 1 && weaponnum <= 20){ //20% auf Ak
                    Instantiate(Ak47,  new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(weaponnum > 20 && weaponnum <= 50){ //30% auf M4
                    Instantiate(M4,  new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(weaponnum > 50 && weaponnum <= 60){ //10% auf Sniper
                    Instantiate(Sniper,  new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(weaponnum > 60 && weaponnum <= 100){ //40% auf Glock
                    Instantiate(Glock, new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }
                yield return new WaitForSeconds(0.01f);
            }
            //2 Munitionsstapel Spawnen
            for(int i = 0 ;i != 2; i++){
                int Ammonum =  Random.Range(1, 3);
                if(Ammonum == 1){ //33% auf Small ammo
                    Instantiate(Small_Ammo,  new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(Ammonum == 2){ //33% auf Mid ammo
                    Instantiate(Mid_Ammo, new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }else if(Ammonum == 3){ //33% auf Big ammo
                    Instantiate(Big_Ammo,  new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
                }
                yield return new WaitForSeconds(0.01f);
            }
            //Heal Spawnen
            int healnum =  Random.Range(0, 4);
            if(healnum == 3){ //Kleine Prozent Chance das Heal gespawned wird
                Instantiate(Heal,  new Vector3(Kisteobj.transform.position.x - Random.Range(Kiste_x_range1, Kiste_x_range2), Kisteobj.transform.position.y + Random.Range(Kiste_y_range1,Kiste_y_range2), 120f), Quaternion.identity);
            }
        }
        
    }
}
