using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flugzeug_Blink : MonoBehaviour
{
    public GameObject Licht_Rot;
    public GameObject Licht_Gruen;
    private void Awake() {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink() {
        while (true) {
            Licht_Rot.SetActive(false);
            Licht_Gruen.SetActive(false);
            yield return new WaitForSeconds(1.2f);
            Licht_Rot.SetActive(true);
            Licht_Gruen.SetActive(true);
            yield return new WaitForSeconds(1.2f);
        }
    }
}
