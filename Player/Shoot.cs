using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Inventory_Handler Inv;
    [SerializeField] private GameObject Player;
    public GameObject Bullet;
    public GameObject Feuerpunkt; 
    public bool isshooting, ishitting;
    public bool shootbttn = false;
    public bool reloadbttn = false;
    public bool inreload, autoshootbttn = false;
    [SerializeField] private Sprite Greenbutton, Redbutton;
    [SerializeField] private GameObject Impactanimation;
    [SerializeField] private LineRenderer lineRenderer;
    public LayerMask layerMask;
    private Color impactobjectcolor;
    public static int Damage_dealt;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Inv = Player.GetComponent<Inventory_Handler>();
        Damage_dealt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(shootbttn == true && !isshooting) StartCoroutine(shoot()); //Wird nur ausgeführt wenn spieler den letzten schuss zuende geführt hat
        

        if(reloadbttn == true){
            StartCoroutine(reload());
        }
    }
    private IEnumerator Hit(){
        if(!isshooting && !ishitting){
            ishitting = true;
            //Play Animation
            animator.SetBool("ishitting", true);
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.up, 1.5f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(17);
                    Damage_dealt += 17;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 17) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(17);
                    Damage_dealt += 17;
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
            }
            //Wait
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("ishitting", false);
            ishitting = false;
        }
    }

    private IEnumerator shoot(){
        if(Inv.Slot1_Selected == true){
        if(Inv.Slot1_Item == "" || Inv.Slot1_Item == null && !ishitting && !isshooting) StartCoroutine(Hit()); //Schlagen
        //Glock-18
        if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    Damage_dealt += 15;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(15);
                    Damage_dealt += 15;
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            lineRenderer.enabled = false;

            Inv.slot1_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Glock-18
        if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot1_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2f);
            Inv.slot1_mag_ammo += 12;
            reloadend:
            inreload = false;
        }
        
        //Für Automatische Gewähre. (M4)
        if(Inv.M4_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                        Damage_dealt += 30;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 30) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30);
                        Damage_dealt += 30;
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload M4
        if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
            inreload = true;
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot1_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.5f);
            Inv.slot1_mag_ammo += 25;
            reloadend:
            inreload = false;
        }

        //Für Automatische Gewähre. (Ak47)
        if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        Damage_dealt += 35;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 35) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(35);
                        Damage_dealt += 35;
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload Ak47
        if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
            inreload = true;
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot1_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.5f);
            Inv.slot1_mag_ammo += 25;
            reloadend:
            inreload = false;
        }

        //Sniper
        if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                        Damage_dealt += 150;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 150) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(150);
                        Damage_dealt += 150;
                    }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.2f);
            lineRenderer.enabled = false;
            Inv.slot1_mag_ammo--;
            yield return new WaitForSeconds(4f); 
            isshooting = false;
        }

        //Reload Sniper
        if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
            inreload = true;
            Inv.big_ammo -= 5;
            if(Inv.big_ammo < 0){
                Inv.big_ammo += 5;
                Inv.slot1_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
                yield return new WaitForSeconds (6f);
                goto reloadend;
            }
            Inv.slot1_mag_ammo += 5;
            yield return new WaitForSeconds (6f);
            reloadend:
            inreload = false;
        }
    }

    if(Inv.Slot2_Selected == true){
        if(Inv.Slot2_Item == "" || Inv.Slot2_Item == null && !ishitting && !isshooting) StartCoroutine(Hit());
         //Glock-18
        if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    Damage_dealt += 15;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(15);
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            lineRenderer.enabled = false;
            Inv.slot2_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Glock-18
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot2_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2.5f);
            Inv.slot2_mag_ammo += 12;
            reloadend:
            inreload = false;
        }
        
        //Für Automatische Gewähre. (M4)
        if(Inv.M4_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                        Damage_dealt += 30;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 30) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30);
                        Damage_dealt += 30;
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Color impactobjectcolor = Color.red;
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                        int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                        if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                        else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload M4
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
            inreload = true;
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot2_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.5f);
            Inv.slot2_mag_ammo += 25;
            reloadend:
            inreload = false;
        }

        //Für Automatische Gewähre. (Ak47)
        if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        Damage_dealt += 35;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 35) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(35);
                        Damage_dealt += 35;
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Color impactobjectcolor = Color.red;
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                        int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                        if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                        else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload Ak47
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
            inreload = true;
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot2_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.5f);
            Inv.slot2_mag_ammo += 25;
            reloadend:
            inreload = false;
        }

        //Sniper
        if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                    Damage_dealt += 150;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 150) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(150);
                    Damage_dealt += 150;
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.2f);
            lineRenderer.enabled = false;
            Inv.slot2_mag_ammo--;
            yield return new WaitForSeconds(4f); 
            isshooting = false;
        }

        //Reload Sniper
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
            inreload = true;
            Inv.big_ammo -= 5;
            if(Inv.big_ammo < 0){
                Inv.big_ammo += 5;
                Inv.slot2_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
                yield return new WaitForSeconds (6f);
                goto reloadend;
            }
            Inv.slot2_mag_ammo += 5;
            yield return new WaitForSeconds (6f);
            reloadend:
            inreload = false;
        }
    }

    if(Inv.Slot3_Selected == true){
        if(Inv.Slot3_Item == "" || Inv.Slot3_Item == null && !ishitting && !isshooting) StartCoroutine(Hit());
         //Glock-18
        if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    Damage_dealt += 15;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(15);
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            lineRenderer.enabled = false;
            Inv.slot3_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Glock-18
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot3_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2.5f);
            Inv.slot3_mag_ammo += 12;
            reloadend:
            inreload = false;
        }
        
        //Für Automatische Gewähre. (M4)
        if(Inv.M4_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                        Damage_dealt += 30;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 30) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30);
                        Damage_dealt += 30;
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Color impactobjectcolor = Color.red;
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                        int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                        if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                        else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload M4
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
            inreload = true;
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot3_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.5f);
            Inv.slot3_mag_ammo += 25;
            reloadend:
            inreload = false;
        }

        //Für Automatische Gewähre. (Ak47)
        if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        Damage_dealt += 35;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 35) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(35);
                        Damage_dealt += 35;
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Color impactobjectcolor = Color.red;
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                        int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                        if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                        else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                        GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                        ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                        main.startColor = impactobjectcolor;
                    }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload Ak47
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
            inreload = true;
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot3_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.5f);
            Inv.slot3_mag_ammo += 25;
            reloadend:
            inreload = false;
        }

        //Sniper
        if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                    Damage_dealt += 150;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 150) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(150);
                    Damage_dealt += 150;
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.2f);
            lineRenderer.enabled = false;
            Inv.slot3_mag_ammo--;
            yield return new WaitForSeconds(4f); 
            isshooting = false;
        }

        //Reload Sniper
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
            inreload = true;
            Inv.big_ammo -= 5;
            if(Inv.big_ammo < 0){
                Inv.big_ammo += 5;
                Inv.slot3_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
                yield return new WaitForSeconds (6f);
                goto reloadend;
            }
            Inv.slot3_mag_ammo += 5;
            yield return new WaitForSeconds (6f);
            reloadend:
            inreload = false;
        }
    }
}
        
    //Shootbutton variablen
    public void shootbutton(){
        shootbttn = true;
    }
    public void shootbuttonrelease(){
        if(!autoshootbttn){
            shootbttn = false;
        }
    }

    //Autoshoot button
    public void Autoshoot(){
        if(!autoshootbttn){
            shootbttn = true;
            autoshootbttn = true;
            GameObject.Find("Autoshoot").GetComponent<Image>().sprite = Greenbutton;
        }else if(autoshootbttn){
            shootbttn = false;
            autoshootbttn = false;
            GameObject.Find("Autoshoot").GetComponent<Image>().sprite = Redbutton;
        }
    }

    //Reloadbutton variablen
    public void reloadbutton(){
        reloadbttn = true;
    }
    public void reloadbuttonrelease(){
        reloadbttn = false;
    }

    public IEnumerator reload(){
        if(Inv.Slot1_Selected){
        //Reload Glock-18
        if(Inv.slot1_mag_ammo != 12 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot1_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2f);
            Inv.slot1_mag_ammo += 12;
            reloadend:
            inreload = false;
        }

        //Reload M4
        if(inreload == false && Inv.M4_Selected == true && isshooting == false && Inv.slot1_mag_ammo != 25){
            inreload = true;
            yield return new WaitForSeconds(4f);
            Inv.mid_ammo += Inv.slot1_mag_ammo;
            if(Inv.mid_ammo >= 25){
                Inv.slot1_mag_ammo = 25;
                Inv.mid_ammo -= 25;
            }else if(Inv.mid_ammo < 25){
                Inv.slot1_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
            }
            inreload = false;
        }

        //Reload M4
        if(inreload == false && Inv.Ak47_Selected == true && isshooting == false && Inv.slot1_mag_ammo != 25){
            inreload = true;
            yield return new WaitForSeconds(4f);
            Inv.mid_ammo += Inv.slot1_mag_ammo;
            if(Inv.mid_ammo >= 25){
                Inv.slot1_mag_ammo = 25;
                Inv.mid_ammo -= 25;
            }else if(Inv.mid_ammo < 25){
                Inv.slot1_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
            }
            inreload = false;
        }

        //Reload Sniper
        if(inreload == false && Inv.Sniper_Selected == true && isshooting == false && Inv.slot1_mag_ammo != 5){
            inreload = true;
            yield return new WaitForSeconds(5f);
            Inv.big_ammo += Inv.slot1_mag_ammo;
            if(Inv.big_ammo >= 5){
                Inv.slot1_mag_ammo = 5;
                Inv.big_ammo -= 5;
            }else if(Inv.big_ammo < 5){
                Inv.slot1_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
            }
            inreload = false;
        }
    }
    else if(Inv.Slot2_Selected){
        //Reload Glock-18
        if(Inv.slot2_mag_ammo != 12 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot2_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2f);
            Inv.slot2_mag_ammo += 12;
            reloadend:
            inreload = false;
        }

        //Reload M4
        if(inreload == false && Inv.M4_Selected == true && isshooting == false && Inv.slot2_mag_ammo != 25){
            inreload = true;
            yield return new WaitForSeconds(4f);
            Inv.mid_ammo += Inv.slot2_mag_ammo;
            if(Inv.mid_ammo >= 25){
                Inv.slot2_mag_ammo = 25;
                Inv.mid_ammo -= 25;
            }else if(Inv.mid_ammo < 25){
                Inv.slot2_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
            }
            inreload = false;
        }

        //Reload M4
        if(inreload == false && Inv.Ak47_Selected == true && isshooting == false && Inv.slot2_mag_ammo != 25){
            inreload = true;
            yield return new WaitForSeconds(4f);
            Inv.mid_ammo += Inv.slot2_mag_ammo;
            if(Inv.mid_ammo >= 25){
                Inv.slot2_mag_ammo = 25;
                Inv.mid_ammo -= 25;
            }else if(Inv.mid_ammo < 25){
                Inv.slot2_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
            }
            inreload = false;
        }

        //Reload Sniper
        if(inreload == false && Inv.Sniper_Selected == true && isshooting == false && Inv.slot2_mag_ammo != 5){
            inreload = true;
            yield return new WaitForSeconds(5f);
            Inv.big_ammo += Inv.slot2_mag_ammo;
            if(Inv.big_ammo >= 5){
                Inv.slot2_mag_ammo = 5;
                Inv.big_ammo -= 5;
            }else if(Inv.big_ammo < 5){
                Inv.slot2_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
            }
            inreload = false;
        }
    }
    else if(Inv.Slot3_Selected){
        //Reload Glock-18
        if(inreload == false && Inv.Glock_18_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 12 && Inv.slot3_mag_ammo != 0){
            inreload = true;
            yield return new WaitForSeconds(2f);
            Inv.small_ammo += Inv.slot3_mag_ammo;
            if(Inv.small_ammo >= 12){
                Inv.slot3_mag_ammo = 12;
                Inv.small_ammo -= 12;
            }else if(Inv.small_ammo < 12){
                Inv.slot3_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
            }
            inreload = false;
        }

        //Reload M4
        if(inreload == false && Inv.M4_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 25){
            inreload = true;
            yield return new WaitForSeconds(4f);
            Inv.mid_ammo += Inv.slot3_mag_ammo;
            if(Inv.mid_ammo >= 25){
                Inv.slot3_mag_ammo = 25;
                Inv.mid_ammo -= 25;
            }else if(Inv.mid_ammo < 25){
                Inv.slot3_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
            }
            inreload = false;
        }

        //Reload M4
        if(inreload == false && Inv.Ak47_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 25){
            inreload = true;
            yield return new WaitForSeconds(4f);
            Inv.mid_ammo += Inv.slot3_mag_ammo;
            if(Inv.mid_ammo >= 25){
                Inv.slot3_mag_ammo = 25;
                Inv.mid_ammo -= 25;
            }else if(Inv.mid_ammo < 25){
                Inv.slot3_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
            }
            inreload = false;
        }

        //Reload Sniper
        if(inreload == false && Inv.Sniper_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 5){
            inreload = true;
            yield return new WaitForSeconds(5f);
            Inv.big_ammo += Inv.slot3_mag_ammo;
            if(Inv.big_ammo >= 5){
                Inv.slot3_mag_ammo = 5;
                Inv.big_ammo -= 5;
            }else if(Inv.big_ammo < 5){
                Inv.slot3_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
            }
            inreload = false;
        }
    }
    }
}
