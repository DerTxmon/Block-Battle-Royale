using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outside_lights_Remove : MonoBehaviour
{
    public GameObject Roofobject;
    public Sprite OriginalRoof;
    public Transform OriginalTransform;
    public GameObject Light1,Light2,Light3,Light4,Light5,Light6,Light7,Light8,Light9,Light10,Smoke1,Smoke2;

    private void Awake() {
        OriginalTransform = Roofobject.transform;
        //Deactivate the Lights
    }

    private void Start() {
        if(Light1 != null){
            Light1.SetActive(true);
        }
        if(Light2 != null){
            Light2.SetActive(true);
        }
        if(Light3 != null){
            Light3.SetActive(true);
        }  
        if(Light4 != null){
            Light4.SetActive(true);
        }
        if(Light5 != null){
            Light5.SetActive(true);
        } 
        if(Light6 != null){
            Light6.SetActive(true);
        }
        if(Light7 != null){
            Light7.SetActive(true);
        }
        if(Light8 != null){
            Light8.SetActive(true);
        }
        if(Light9 != null){
            Light9.SetActive(true);
        }
        if(Light10 != null){
            Light10.SetActive(true);
        }
        if(Smoke1 != null){
            Smoke1.SetActive(true);
        }
        if(Smoke2 != null){
            Smoke2.SetActive(true);
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Roofobject.GetComponent<SpriteRenderer>().sprite = null;
            if(Light1 != null){
                Light1.SetActive(false);
            }
            if(Light2 != null){
                Light2.SetActive(false);
            }
            if(Light3 != null){
                Light3.SetActive(false);
            }
            if(Light4 != null){
                Light4.SetActive(false);
            }
            if(Light5 != null){
                Light5.SetActive(false);
            }
            if(Light6 != null){
                Light6.SetActive(false);
            }
            if(Light7 != null){
                Light7.SetActive(false);
            }
            if(Light8 != null){
                Light8.SetActive(false);
            }
            if(Light9 != null){
                Light9.SetActive(false);
            }
            if(Light10 != null){
                Light10.SetActive(false);
            }
            if(Smoke1 != null){
                Smoke1.SetActive(false);
            }
            if(Smoke2 != null){
                Smoke2.SetActive(false);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Roofobject.GetComponent<SpriteRenderer>().sprite = OriginalRoof;
            if(Light1 != null){
                Light1.SetActive(true);
            }
            if(Light2 != null){
                Light2.SetActive(true);
            }
            if(Light3 != null){
                Light3.SetActive(true);
            }
            if(Light4 != null){
                Light4.SetActive(true);
            }
            if(Light5 != null){
                Light5.SetActive(true);
            } 
            if(Light6 != null){
                Light6.SetActive(true);
            } 
            if(Light7 != null){
                Light7.SetActive(true);
            }
            if(Light8 != null){
                Light8.SetActive(true);
            }
            if(Light9 != null){
                Light9.SetActive(true);
            }
            if(Light10 != null){
                Light10.SetActive(true);
            }
            if(Smoke1 != null){
                Smoke1.SetActive(true);
            }
            if(Smoke2 != null){
                Smoke2.SetActive(true);
            }
        }
    }
}
