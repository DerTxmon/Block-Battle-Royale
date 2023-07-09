using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour
{
    public float health;
    public float Maxhealth;
    public Text Lebenstext;
    private GameObject World;
    public GameObject Healthbar;
    public UI_Handler UI;
    
    void Awake(){
        UI = gameObject.GetComponent<UI_Handler>();
    }
    void Start()
    {
        health = 200f;
        Lebenstext = GameObject.Find("Lebenstext").GetComponent<Text>();
        Maxhealth = 200f;
        World = GameObject.Find("World");
    }
    
    public void Damage(int Dmg){
        health -= Dmg;
        //Check for death
        if(health <= 0){
            StartCoroutine(Death());
        }
        //Korrigiere - Zahlen
        if(health < 0 ) health = 0;
        //UI Update
        Lebenstext.text = health.ToString() + "%";
        Healthbar.GetComponent<Image>().fillAmount = health / Maxhealth;
        if(health <= 20){
            Healthbar.GetComponent<Image>().color = Color.red;
        }else{
            Healthbar.GetComponent<Image>().color = Color.green;
        }
        this.gameObject.GetComponent<Player_Health>().enabled = false; //Damit kein negatiever Schaden mehr gemacht werden kann
    }

    public IEnumerator Death(){
        //Langsam Zeit runter schrauben
        while(Time.timeScale != .1f){
            yield return new WaitForSeconds(.1f);
            float applytotime = Time.timeScale -.1f;
            Time.timeScale = (float)System.Math.Round(applytotime * 100) / 100; //Rundet auf 2 Nachkommastellen
        }
        StartCoroutine(UI.EndScreen(false));
    }
}
