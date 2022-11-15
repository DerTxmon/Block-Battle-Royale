using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone_Manager : MonoBehaviour
{
    public static Zone_Manager instance;
    private GameObject Player;
    private Transform Zonetransform;
    private Transform toptransform;
    private Transform righttransform;
    private Transform lefttransform;
    private Transform bottomtransform;
    private Vector3 ZoneSize;
    private Vector3 ZonePosition;
    public float ZonenSchrumpfGeschwindigkeit = 1;
    private Vector3 targetZoneSize;
    public bool TakingDamage;
    public float Zoneticktime = 1f;
    public bool InZone;
    public int currentzonedamage = 2;
    private void Start() {
        StartCoroutine(ZoneDamage());
        StartCoroutine(IncreaseZoneDMG());
    }
    private void Awake() {
        instance = this;

        Player = GameObject.Find("Player");
        Zonetransform = GameObject.Find("Zone").transform;
        toptransform = GameObject.Find("zone_top").transform;
        lefttransform = GameObject.Find("zone_left").transform;
        righttransform = GameObject.Find("zone_right").transform;
        bottomtransform = GameObject.Find("zone_bottom").transform;

        SetZoneSize(new Vector3(0f,0f, -9.199997f), new Vector3(1344.96f,1344.96f, 0f));
    }

    void Update(){
        ZonenSchrumf();
    }

    void ZonenSchrumf(){
        targetZoneSize = new Vector3(20f,20f,0f);

        Vector3 sizechangeVector = (targetZoneSize - ZoneSize).normalized;
        Vector3 newZoneSize = ZoneSize + sizechangeVector * Time.deltaTime * ZonenSchrumpfGeschwindigkeit;
        SetZoneSize(ZonePosition, newZoneSize);
    }

    private void SetZoneSize(Vector3 position ,Vector3 size){
        ZonePosition = position;
        ZoneSize = size;

        //transform.position = position; vorerst unnötig da sich zone nicht bewegen muss
        Zonetransform.localScale = size;

        //Alle Zonen Teile werden auf Die Zonen Größe und Position zugeschnitten.
        toptransform.localScale = new Vector3(2000, 2000);
        toptransform.localPosition = new Vector3(0f, toptransform.localScale.y * .5f + size.y * .5f,-9.199997f);

        bottomtransform.localScale = new Vector3(2000, 2000);
        bottomtransform.localPosition = new Vector3(0f, -toptransform.localScale.y * .5f - size.y * .5f,-9.199997f);

        lefttransform.localScale = new Vector3(2000, size.y);
        lefttransform.localPosition = new Vector3(-lefttransform.localScale.x * .5f - size.x * .5f, 0f, -9.199997f);

        righttransform.localScale = new Vector3(2000, size.y);
        righttransform.localPosition = new Vector3(+lefttransform.localScale.x * .5f + size.x * .5f, 0f, -9.199997f);
    }

    IEnumerator IncreaseZoneDMG(){
        while(true){
            yield return new WaitForSeconds(15f);
            currentzonedamage += 5;
        }
    }
    IEnumerator ZoneDamage(){
        while (true){
            TakingDamage = true;
            if(InZone == true) Player.GetComponent<Player_Health>().Damage(currentzonedamage);
            TakingDamage = false;
            yield return new WaitForSeconds(Zoneticktime);
        }
    }
}