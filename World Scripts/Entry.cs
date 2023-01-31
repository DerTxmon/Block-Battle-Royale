using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] public int id;
    [SerializeField] private GameObject Entry2; 
    [SerializeField] public bool dosomthing;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bot"){
            foreach(int i in collision.gameObject.GetComponentInChildren<Bot_PlayerContact>().Entryids){
                if(i != id){
                    dosomthing = true;
                }else{
                    dosomthing = false;
                    break; 
                }
            }
            if(dosomthing){
                collision.gameObject.GetComponentInChildren<Bot_PlayerContact>().Entry2 = true;
                collision.gameObject.GetComponentInChildren<Bot_PlayerContact>().Entryids.Add(id);
                StartCoroutine(Hs(collision));
            }else{
                collision.gameObject.GetComponentInChildren<Bot_PlayerContact>().Entry2 = false;
                collision.gameObject.GetComponent<Bot_Behavior>().EntryPoint = null;
            }
            
        }
    }
    private IEnumerator Hs(Collider2D Bot){
        yield return new WaitForEndOfFrame();
        Bot.gameObject.GetComponent<Bot_Behavior>().EntryPoint = Entry2;
    }
}
