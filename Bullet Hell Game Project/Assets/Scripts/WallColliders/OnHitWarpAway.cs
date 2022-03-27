using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitWarpAway : MonoBehaviour
{
    bool m_Started;


    private GameObject HostBody; 

    private Vector3 hitboxSize; 


    public Controller_EnemyBullets bullets; //this points towards the holder of the KillBullet function?
    //Vector3 spawnlocation;
    void Start()
    {
        bullets = GameObject.Find("Controller").GetComponent<Controller_EnemyBullets>();
        // spawnlocation = new Vector3(0, 0, 0);
        m_Started = true; //This activates OnDrawGizmos() which shows the hitbox with a red outline
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
                //bullets.KillBullet(c.gameObject);  //Safely GTFO's a bullet without breaking the game
                c.gameObject.transform.position = new Vector3(1000, 0, 0);  //Could just warp it behind bounds!!
                                                                            // c.gameObject.transform.position = new Vector3(0, 0, 0);
                Debug.Log("Blocked a player bullet");
            }
            // bullets.FireBullet(Vector 3 location, Vector3 direction)

            //else if (c.gameObject.tag == "EnemyBullet")   //This section can be used for "Bullet amplification"
            //{
            //  Debug.Log("Found a enemy bullet");
            // }
            

        }

    }

 
    //void OnDrawGizmos()  //Show the collider box (Doesnt show rotation)
    //{
    //    Gizmos.color = Color.red;
    //    //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
    //    if (m_Started)
            
    //    //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //    //Gizmos.DrawWireCube(transform.position, gameObject.transform.localScale);
    //    //Gizmos.DrawWireCube(transform.position, new Vector3(1*sizex, 1, 0.4f*5));
    //    Gizmos.DrawWireCube(transform.position, hitboxSize);
          
    //}
}