using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy(){
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
