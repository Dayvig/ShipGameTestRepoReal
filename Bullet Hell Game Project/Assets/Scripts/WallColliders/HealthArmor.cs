using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthArmor : MonoBehaviour
{

    private GameObject HostBody; 

    private Vector3 hitboxSize;
    public int Armor_Health;

    public Controller_EnemyBullets bullets; 
 
    void Start()
    {
        bullets = GameObject.Find("Controller").GetComponent<Controller_EnemyBullets>();
        HostBody = transform.parent.gameObject;

        hitboxSize = Vector3.Scale(gameObject.transform.localScale, HostBody.transform.localScale);

    }


    void Update()
    {
        hitboxSize = Vector3.Scale(gameObject.transform.localScale, HostBody.transform.localScale);
        var around = Physics.OverlapBox(transform.position, hitboxSize * 0.5f, transform.rotation); //gets all the colliders in the 
                                                                                   //shape of the parented object
        foreach (Collider c in around)
        {
            //Debug.Log("Something Detected");
            if (c.gameObject.tag == "PlayerBullet")   //If a player bullet hits the core
            {
               
                
                if (Armor_Health >= 1)
                {
                    c.gameObject.transform.position = new Vector3(1000, 0, 0);  //Warp a bullet far far away
                    Armor_Health = Armor_Health - 1;
                }
                else
                {
                    Debug.Log("Armor is broken");
                    transform.gameObject.SetActive(false);
                }
                Debug.Log(Armor_Health);
                //Debug.Log("Blocked a player bullet");
            }
           

            //else if (c.gameObject.tag == "EnemyBullet")   //This section can be used for "Bullet amplification"
            //{
            //  Debug.Log("Found a enemy bullet");
            // }
            

        }

    }

 

}