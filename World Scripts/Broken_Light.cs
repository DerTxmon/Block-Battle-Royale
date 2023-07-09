using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;


public class Broken_Light : MonoBehaviour
{
    public Light2D Light;
    private void Awake() {
        Light = GetComponent<Light2D>();
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
