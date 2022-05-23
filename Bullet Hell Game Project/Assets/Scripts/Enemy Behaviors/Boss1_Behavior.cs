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
    private Material DefaultShellShader;

    public Material MidHealthShader;
    public Material LowHealthShader;

    private float OgShootInterval; 
    private double HPThird_Top;
    private double HPThird_Bottom;

    private bool FirstRunYellow = true;
    private bool FirstRunRed = true;
    public float Armor0;
    public float Armor1;
    public float Armor2;
    public float Armor3;
    public float Armor4;
    public float Armor5;
    public float Armor6;
    public float Armor7;
    public float Armor8;
    public float Armor9;

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
                
                boss1Value.fireRate = 0.2f;
                bulletSpeed = 8; 
                rate = 30;
                boss1Value.moveSpeed = 1.8f * healthSpeedMultiplier;
            }
            else if (currentWaypointIndex == 3 || currentWaypointIndex == 6) //Downwards and upwards movements
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
                bulletSpeed = 8;
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
            //Debug.Log(currentWaypointIndex);
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
        DefaultShellShader = gameObject.transform.GetChild(10).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        HPThird_Top = boss1Value.hp * 0.66;
        HPThird_Bottom = boss1Value.hp * 0.33;

        Armor0 = gameObject.transform.GetChild(0).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor1 = gameObject.transform.GetChild(1).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor2 = gameObject.transform.GetChild(2).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor3 = gameObject.transform.GetChild(3).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor4 = gameObject.transform.GetChild(4).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor5 = gameObject.transform.GetChild(5).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor6 = gameObject.transform.GetChild(6).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor7 = gameObject.transform.GetChild(7).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor8 = gameObject.transform.GetChild(8).gameObject.GetComponent<HealthArmor>().Armor_Health;
        Armor9 = gameObject.transform.GetChild(9).gameObject.GetComponent<HealthArmor>().Armor_Health;

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
        //HealthArmor.Armor_Health = 100;

        
        
        if (hitPoints <= HPThird_Top && hitPoints >= HPThird_Bottom) //Turn it yellow
        {
            // Debug.Log("Yellow");

            if (FirstRunYellow == true)
            {
                //reset the armors health
                FirstRunYellow = false;

                gameObject.transform.GetChild(0).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(1).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(2).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(3).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(4).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(5).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(6).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(7).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(8).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(9).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;

                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                gameObject.transform.GetChild(3).gameObject.SetActive(true);
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                gameObject.transform.GetChild(5).gameObject.SetActive(true);
                gameObject.transform.GetChild(6).gameObject.SetActive(true);
                gameObject.transform.GetChild(7).gameObject.SetActive(true);
                gameObject.transform.GetChild(8).gameObject.SetActive(true);
                gameObject.transform.GetChild(9).gameObject.SetActive(true);


            }
            //gameObject.GetComponent<MeshRenderer>().material = MidHealthShader;
            gameObject.transform.GetChild(10).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = MidHealthShader;
            healthSpeedMultiplier = 1.3f;
        }
        else if (hitPoints <= HPThird_Bottom)  //Turn it red
        {

            if (FirstRunRed == true)
            {
                //reset the armors health
                FirstRunRed = false;

                gameObject.transform.GetChild(0).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(1).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(2).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(3).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(4).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(5).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(6).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(7).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(8).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;
                gameObject.transform.GetChild(9).gameObject.GetComponent<HealthArmor>().Armor_Health = 70;

                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                gameObject.transform.GetChild(3).gameObject.SetActive(true);
                gameObject.transform.GetChild(4).gameObject.SetActive(true);
                gameObject.transform.GetChild(5).gameObject.SetActive(true);
                gameObject.transform.GetChild(6).gameObject.SetActive(true);
                gameObject.transform.GetChild(7).gameObject.SetActive(true);
                gameObject.transform.GetChild(8).gameObject.SetActive(true);
                gameObject.transform.GetChild(9).gameObject.SetActive(true);


            }
            //Debug.Log("Red");
            gameObject.GetComponent<MeshRenderer>().material = LowHealthShader;
            gameObject.transform.GetChild(10).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = LowHealthShader;
            healthSpeedMultiplier = 1.6f;
            AlmostDead = true;
        }
        else //Its green
        {
           //Debug.Log("Green");
            gameObject.GetComponent<MeshRenderer>().material = DefaultShader;
            gameObject.transform.GetChild(10).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = DefaultShellShader;
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