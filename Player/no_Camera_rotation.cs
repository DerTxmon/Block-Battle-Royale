using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class no_Camera_rotation : MonoBehaviour
{
    public GameObject player;

    void Awake() {
        player = GameObject.Find("Player");
    }

    void Update(){
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

}

