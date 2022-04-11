using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;


public class Boss1_Behavior : Base_Enemy_Behavior
{
    private Boss1Enemy boss1Value;

    public bool isLeft; 
    public static string BULLET_NAME = "MotorcycleBullet";

    private Material DefaultShader;
    public Material MidHealthShader;
    public Material LowHealthShader;

    private float OgShootInterval; 
    private double HPThird_Top;
    private double HPThird_Bottom;

    private bool AlmostDead = false;
    private int spinnerCounter = 0;

    private float healthSpeedMultiplier;
 

    private int defaultSpinRate;
    private float defaultSpeed;
    //bool defeated = false;  

    public override void MovementUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, boss1Value.moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextWaypoint) < 1 && currentWaypointIndex != Waypoints.Count-1)
        {
            //Debug.Log(currentWaypointIndex);
            // BELOW ARE SPEED AND PATHING MANIPULATION PER SPECIFIC INDEX
            // || = OR , && = and
            if (currentWaypointIndex >= 0 && currentWaypointIndex <= 2) 
            {
                
                boss1Value.fireRate = 0.07f;
                bulletSpeed = 1; //og
                rate = 30;
                boss1Value.moveSpeed = 1.8f * healthSpeedMultiplier;
            }
            else if (currentWaypointIndex == 3 || currentWaypointIndex == 7) //Downwards and upwards movements
            {
                rate = 9000;
                boss1Value.moveSpeed = 20 * healthSpeedMultiplier;
                boss1Value.fireRate = OgShootInterval;
            }
            else if (currentWaypointIndex == 4 || currentWaypointIndex == 5) //Panning under
            {
                boss1Value.fireRate = 0.05f;
                rate = -9000;
                boss1Value.moveSpeed = 1 * healthSpeedMultiplier;
                bulletSpeed = 11;
            }
            else if (currentWaypointIndex == 8)
            {
                bulletSpeed = 8; //og
                rate = 9000;
                boss1Value.moveSpeed = 20 * healthSpeedMultiplier;
            }
            else
            {
                rate = defaultSpinRate;
                boss1Value.moveSpeed = defaultSpeed * healthSpeedMultiplier;
            }
            SetToNextWaypoint();
        }
        else if (currentWaypointIndex >= Waypoints.Count - 1) // if hit the end of the waypoint list
        {
            //Debug.Log("Looping back to first waypoint");
            currentWaypointIndex = -1;
            SetToNextWaypoint();
        }
        shootInterval = boss1Value.fireRate;
    }

    public override void SetupEnemy()
    {
        boss1Value = GameObject.Find("Model").GetComponent<Boss1Enemy>(); //Gets the prefab
        shootInterval = boss1Value.fireRate;
        OgShootInterval = boss1Value.fireRate;
        bulletSpeed = boss1Value.bulletSpeed;
        healthSpeedMultiplier = 1.0f;
        //
        shootTimer = Random.Range(0, shootInterval);  //THIS SHOULDNT BE RANDOM??
        ///
        hitPoints = boss1Value.hp;
        defaultSpinRate = rate;
        defaultSpeed = boss1Value.moveSpeed;

        DefaultShader = gameObject.GetComponent<MeshRenderer>().material;
        HPThird_Top = boss1Value.hp * 0.66;
        HPThird_Bottom = boss1Value.hp * 0.33;

    }

    public override bool Immune()
    {
        return false;
    }

    public override bool canShoot()
    {
        return inScreen();
    }

    public override void UpdateVisuals()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rate); //Rotation

        //if (defeated == true)
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(1); //Change scene
        //}
        //var HPThird1 = hitPoints * 0.33f;
        //var HPThird2;
        //var HPThird3;

        //Debug.Log(hitPoints);
        
        if (hitPoints <= HPThird_Top && hitPoints >= HPThird_Bottom) //Turn it yellow
        {
           // Debug.Log("Yellow");
            gameObject.GetComponent<MeshRenderer>().material = MidHealthShader;
            healthSpeedMultiplier = 1.3f;
        }
        else if (hitPoints <= HPThird_Bottom)  //Turn it red
        {
            //Debug.Log("Red");
            gameObject.GetComponent<MeshRenderer>().material = LowHealthShader;
            healthSpeedMultiplier = 1.6f;
            AlmostDead = true;
        }
        else //Its green
        {
           // Debug.Log("Green");
            gameObject.GetComponent<MeshRenderer>().material = DefaultShader;
            healthSpeedMultiplier = 1.0f;
        }

    }

    public override void FiringPattern()  
    {
        bullets.FireBullet(transform.position, (playerModel.ship.transform.position - transform.position).normalized, BULLET_NAME, this);
        if (AlmostDead == true)
        {
            if (spinnerCounter >= 360)
            {
                spinnerCounter = 0;
            }
            SpreadPattern(BULLET_NAME, spinnerCounter, spinnerCounter + 90, 2);
            spinnerCounter+=15;
        }
        
    }


}