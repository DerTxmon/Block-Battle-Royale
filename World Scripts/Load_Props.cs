using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_Props : MonoBehaviour
{
    [SerializeField] private GameObject ParentofProps;

    void Start(){
        ParentofProps.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            ParentofProps.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            ParentofProps.SetActive(false);
        }
    }
}
