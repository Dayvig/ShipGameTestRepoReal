using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGrazerHitbox : MonoBehaviour
{
    //bool m_Started;   //This is for the red outline during testing


    private GameObject HostBody; 
    private Vector3 hitboxSize;
    public Model_Player playerModel;

    public float elapsed = 0f; // a timer?
    private bool debounce = false;

    public Controller_EnemyBullets bullets; //this points towards the holder of the KillBullet function?
    //Vector3 spawnlocation;
    void Start()
    {
        bullets = GameObject.Find("Controller").GetComponent<Controller_EnemyBullets>();
        playerModel = GameObject.Find("Model").GetComponent<Model_Player>();

        // spawnlocation = new Vector3(0, 0, 0);
        //m_Started = true; //This activates OnDrawGizmos() which shows the hitbox with a red outline
        HostBody = transform.parent.gameObject;

        hitboxSize = Vector3.Scale(gameObject.transform.localScale, HostBody.transform.localScale);

    }


    void Update()
    {
        hitboxSize = Vector3.Scale(gameObject.transform.localScale, HostBody.transform.localScale);
        var around = Physics.OverlapBox(transform.position, hitboxSize * 0.5f, transform.rotation); //gets all the colliders in the 

        foreach (Collider c in around)
        {
            //Debug.Log("Something Detected");
            if (c.gameObject.tag == "EnemyBullet" && debounce == false)   //If a player bullet hits the core
            {
                debounce = true;

                Debug.Log("Grazed a enemy bullet");
                playerModel.bulletGrazes++;
                
                GetComponent<ParticleSystem>().Play();
                ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
                em.enabled = true;

                StartCoroutine(ExampleCoroutine());
          
            }
            // bullets.FireBullet(Vector 3 location, Vector3 direction)

            //else if (c.gameObject.tag == "EnemyBullet")   //This section can be used for "Bullet amplification"
            //{
            //  Debug.Log("Found a enemy bullet");
            // }
        }
    }


    IEnumerator ExampleCoroutine() //Waiter
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(1); //wait()

        GetComponent<ParticleSystem>().Play();
        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission; 
        em.enabled = false;

        yield return new WaitForSeconds(1); //wait()
        //Debug.Log("Set up for next bullet");
        debounce = false;

       // Debug.Log("Finished Coroutine at timestamp : " + Time.time);
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