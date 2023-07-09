using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public float ZonenSchrumpfGeschwindigkeit;
    private Vector3 targetZoneSize;
    public bool TakingDamage;
    public float Zoneticktime = 1f;
    public bool InZone;
    public int currentzonedamage = 2;
    public int ZoneMoves = 0;
    public int TimeUntilZone;
    public TextMeshProUGUI ZoneTimerText;
    private void Awake() {
        instance = this;

        Player = GameObject.Find("Player");
        Zonetransform = GameObject.Find("Zone").transform;
        toptransform = GameObject.Find("zone_top").transform;
        lefttransform = GameObject.Find("zone_left").transform;
        righttransform = GameObject.Find("zone_right").transform;
        bottomtransform = GameObject.Find("zone_bottom").transform;

        SetZoneSize(new Vector3(0f,0f, -9.199997f), new Vector3(1153.6f,1153.6f, 0f));
    }
    private void Start(){
        StartCoroutine(ZonePhasen());
        StartCoroutine(ZoneDamage());
        StartCoroutine(IncreaseZoneDMG());
    }
    private IEnumerator ZonePhasen(){
        //Phase 1
        Debug.Log("Zone Phase 1");
        targetZoneSize = new Vector3(1153.6f,1153.6f, 0f);
        //Zähl runter
        for(TimeUntilZone = 0; TimeUntilZone != 0; TimeUntilZone--){
            UpdateZoneTimer();
            yield return new WaitForSeconds(1f);
        }
        //Phase 2
        Debug.Log("Zone Phase 2");
        targetZoneSize = new Vector3(800f, 800f, 0f);
        TimeUntilZone = 0;
        UpdateZoneTimer();
        yield return new WaitForSeconds(105f); //105 sekunden warten
        //Zähl runter
        for(TimeUntilZone = 180; TimeUntilZone != 0; TimeUntilZone--){
            UpdateZoneTimer();
            yield return new WaitForSeconds(1f);
        }/*
        //Phase 3
        Debug.Log("Zone Phase 3");
        targetZoneSize = new Vector3(288.4f,288.4f, 0f);
        //Zähl runter
        for(TimeUntilZone = 320; TimeUntilZone != 0; TimeUntilZone--){
            UpdateZoneTimer();
            yield return new WaitForSeconds(1f);
        }*/

        yield return null;
    }
    public void UpdateZoneTimer(){
        //Convert into minutes format
        int minutes = 0;
        int seconds = 0;
        if(TimeUntilZone >= 60){
            minutes = TimeUntilZone / 60;
            seconds = TimeUntilZone % 60;
        }else{
            seconds = TimeUntilZone;
        }
        //Update Text
        ZoneTimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void Update(){
        ZonenSchrumf();
    }

    void ZonenSchrumf(){
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
            TakingDamage = true; //??
            if(InZone == true) Player.GetComponent<Player_Health>().Damage(currentzonedamage);
            TakingDamage = false; //??
            yield return new WaitForSeconds(Zoneticktime);
        }
    }
}