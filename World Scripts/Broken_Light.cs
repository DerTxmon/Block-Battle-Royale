using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Broken_Light : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D Light;
    private void Awake() {
        Light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        StartCoroutine(BrokenLight());    
    }
    private IEnumerator BrokenLight() {
        while(true){
            yield return new WaitForSeconds(Random.Range(.1f, .8f));
            Light.enabled = true;
            yield return new WaitForSeconds(Random.Range(.1f, .8f));
            Light.enabled = false;
        }
    }
}
