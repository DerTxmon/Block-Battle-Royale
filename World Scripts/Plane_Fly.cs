using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Fly : MonoBehaviour
{
    public float FlugzeugStartTime;
    private Rigidbody2D rb;
    void Awake(){
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(Start());
    }

    IEnumerator Start(){
        yield return new WaitForSeconds(FlugzeugStartTime); //Flugzeug wartet auf Start
        Debug.Log("Flugzeug startet");
        for(int i = 0; i != 694; i++){
            this.gameObject.transform.Translate(Vector3.up * 0.0198f,Space.Self);//
            yield return new WaitForSeconds(0.007f);
        }

        for(int i = 0; i < 180; i++){
            rb.rotation += 0.25f;
            this.gameObject.transform.Translate(Vector3.up * 0.055f,Space.Self);
            yield return new WaitForSeconds(0.02f);
        }

        for(int i = 0; i != 4493; i++){
            this.gameObject.transform.Translate(Vector3.up * 0.0198f,Space.Self);//
            yield return new WaitForSeconds(0.007f);
        }

        for(int i = 0; i < 180; i++){
            rb.rotation -= 0.25f;
            this.gameObject.transform.Translate(Vector3.up * 0.055f,Space.Self);
            yield return new WaitForSeconds(0.02f);
        }

        for(int i = 0; i < 180; i++){
            rb.rotation -= 0.25f;
            this.gameObject.transform.Translate(Vector3.up * 0.0451f,Space.Self);
            yield return new WaitForSeconds(0.02f);
        }

        for(int i = 0; i != 69; i++){
            this.gameObject.transform.Translate(Vector3.up * 0.0198f,Space.Self);//
            yield return new WaitForSeconds(0.007f);
        }

        yield return new WaitForSeconds(4f);
        float beschleuning = 0.08f;
        for(int i = 0; beschleuning <= 60f; i++){
            rb.velocity = new Vector2(0f, beschleuning); //Abheben
            beschleuning *= 1.2f;
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider2D>().isTrigger = true;
        //Scale the Plane down
        for(int i = 0; i < 60; i++){
            this.gameObject.transform.localScale -= new Vector3(0.005f,0.005f,0f);
            yield return new WaitForSeconds(0.07f);
        }
        Destroy(this.gameObject);
    }
}
