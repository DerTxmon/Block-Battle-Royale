using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
    StartCoroutine(Removefootstep());
    }

    private IEnumerator Removefootstep(){
        for(int i = 0; i < 19; i++){
            //add .1 alpha to the footstep
            gameObject.GetComponent<SpriteRenderer>().color -= new Color(0,0,0,.05f);
            yield return new WaitForSeconds(.2f);
        }
        Destroy(this.gameObject);
    }
}
