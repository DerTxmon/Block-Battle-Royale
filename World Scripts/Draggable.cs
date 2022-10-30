using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour
{
    public Camera MainCam;
    Vector3 MousePositionOffset;

    private Vector3 GetMouseWorldPosition(){
        return MainCam.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDown() {
        MousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }
    private void OnMouseDrag() {
        transform.position = GetMouseWorldPosition() + MousePositionOffset;
    }
}
