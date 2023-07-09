using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject CarObjkect;
    public GameObject CamPoint;
    /*private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == "Player"){
            collider.gameObject.GetComponent<UI_Handler>().isAtCar();
            PassCar(collider.gameObject);
            //Schalte RigidBody aus so das das auto nicht vom Spieler geschoben wird
            CarObjkect.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.tag == "Player"){
            collider.gameObject.GetComponent<UI_Handler>().isNotAtCar();
            CarObjkect.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
    public void PassCar(GameObject Player){
        Player.GetComponent<Movement>().EnterableCar = CarObjkect;
        Player.GetComponent<Movement>().CarCamPoint = CamPoint;
    }*/
}