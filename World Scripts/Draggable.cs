using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour
{
    public Camera MainCam;
    Vector3 MousePositionOffset;
    public GameObject Cross;

    private Vector3 GetMouseWorldPosition(){
        return MainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDown() {
        Cross.gameObject.transform.position = new Vector3(GetMouseWorldPosition().x, GetMouseWorldPosition().y, -1);
    }
    private void OnMouseDrag() {
        Cross.gameObject.transform.position = new Vector3(GetMouseWorldPosition().x, GetMouseWorldPosition().y, -1);
    }
}
