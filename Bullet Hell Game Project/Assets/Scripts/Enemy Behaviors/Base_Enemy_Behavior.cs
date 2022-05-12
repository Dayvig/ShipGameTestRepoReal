using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Controllers;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public abstract class Base_Enemy_Behavior : MonoBehaviour
{
    public Model_Player playerModel;
    public Model_Game gameModel;
    public Controller_Effects effects;
    public Controller_EnemyBullets bullets;
    public Controller_Fuel controllerFuel;
    public Controller_Sound boom;
    public Canister_spawner c;
    public ParticleSystem ps;
    public float hitPoints;
    public float limitHorz = 15;
    public float limitVert = 12;
    public int rate = 360;
    public List<Vector3> Waypoints = new List<Vector3>();
    public Vector3 nextWaypoint;
    public int currentWaypointIndex;
    public SphereCollider hitbox;

    public float shootInterval;
    public float shootTimer;
    public int behaviorState = 0;
    public float moveSpeed;
    public float bulletSpeed;
    public bool switchedWaypoint = false;
    public bool indicatorSpawned;

    private void Start()
    {
        playerModel = GameObject.Find("Model").GetComponent<Model_Player>();
        gameModel =  GameObject.Find("Model").GetComponent<Model_Game>();
        effects = GameObject.Find("Controller").GetComponent<Controller_Effects>();
        bullets = GameObject.Find("Controller").GetComponent<Controller_EnemyBullets>();
        controllerFuel = GameObject.Find("Controller").GetComponent<Controller_Fuel>();
        boom = GameObject.Find("Controller").GetComponent<Controller_Sound>();
        c = GameObject.Find("Controller").GetComponent<Canister_spawner>();
        hitbox = GetComponent<SphereCollider>();
        shootTimer = 0;
        if (Waypoints.Count == 0)
        {
            Debug.Log("Empty Waypoints on enemy");
            Waypoints.Add(new Vector3(0, 0, 0));
        }
        nextWaypoint = Waypoints[0];
        currentWaypointIndex = 0;
        indicatorSpawned = false;
        SetupEnemy();
    }

    void Update()
    {
        ShootingUpdate();
        UpdateVisuals();
        MovementUpdate();
        var sizeCalc = (((gameObject.transform.localScale.x + gameObject.transform.localScale.z)/2)/2); //Calculates how big the hitbox should be
            //^ Right now it gets the X and Z size of the object, averages them. then makes a radius.
            //Debug.Log(sizeCalc);
            //var around = Physics.OverlapSphere(transform.position, sizeCalc); //Creates the hitbox (Sphere) of a enemy

            var around = Physics.OverlapSphere(transform.position, 1); //Creates the hitbox (Sphere) of a enemy

            if (gameObject.name.Contains("Boss1"))
            {
                around = Physics.OverlapSphere(transform.position, sizeCalc); //Creates the hitbox (Sphere) of a enemy
            }

            foreach (Collider c in around)
        {
            if (c.gameObject.tag == "PlayerBullet" && !Immune())
            {
                effects.MakeSmallExplosion(transform.position);
                c.gameObject.transform.position += Vector3.forward * 1000;
                hitPoints -= playerModel.damageCurrent;
            }
        }
        if (hitPoints <= 0)
        {
            if (gameObject.name == "Boss1(Clone)") //If something specific died then do something
            {
                KillThisEnemy();

                Debug.Log("Level 1 Complete, Changing Scene");

                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
            else
            {
                //Debug.Log(gameObject.name+" died");
                boom.boom();
                KillThisEnemy();
            }
            
        }
    }

    //conditions for when an enemy can fire
    public abstract bool canShoot();

    public abstract void UpdateVisuals();

    public abstract void FiringPattern();

    public abstract void MovementUpdate();

    public abstract void SetupEnemy();

    public abstract bool Immune();
    public virtual void ShootingUpdate()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval / 2 && !indicatorSpawned)
        {
            indicatorSpawned = true;
            SpawnIndicator();
        }
        if (shootTimer >= shootInterval)
        {
            shootTimer -= shootInterval;
            if (canShoot())
            {
                FiringPattern();
                indicatorSpawned = false;
            }
        }
    }

    public void SetToNextWaypoint()
    {
        currentWaypointIndex++;
        if (Waypoints.Count > currentWaypointIndex)
        {
            nextWaypoint = Waypoints[currentWaypointIndex];
        }
        else
        {
            Debug.Log("Tried to set to incorrect waypoint");
        }
    }

    public void SetToWaypoint(int toWayPoint)
    {
        currentWaypointIndex = toWayPoint;
        if (Waypoints.Count > currentWaypointIndex)
        {
            nextWaypoint = Waypoints[currentWaypointIndex];
        }
        else
        {
            Debug.Log("Tried to set to incorrect waypoint");
        }
    }

    public virtual void KillThisEnemy()
    {
        gameObject.SetActive(false);
        if (inScreen())
        {
            effects.MakeExplosion(transform.position);
            gameModel.enemiesKilled++;
            if (controllerFuel.spawnGas)
            {
                Instantiate(c.canister, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.identity);
                controllerFuel.spawnGas = false;
            }
        }
    }

    public virtual void SpawnIndicator()
    {
        GameObject thisIndicator = Instantiate(gameModel.indicatorPrefab, transform.position, Quaternion.Euler(90, 0, 0));
        thisIndicator.GetComponent<LaserUpdate>().toFollow = transform.gameObject;
        thisIndicator.GetComponent<LaserUpdate>().duration = shootInterval / 2;
    }
    
    public bool inScreen()
    {
        if ((gameObject.transform.position.x < limitHorz) &&
            (gameObject.transform.position.x > -limitHorz) &&
            (gameObject.transform.position.z < limitVert) &&
            (gameObject.transform.position.z > -limitVert))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpreadPattern(string name, float startAngle, float endAngle, int numBullets)
    {

        float angleStep = (endAngle - startAngle) / numBullets;
        float currentAngle = startAngle;

        for (int i = 0; i < numBullets; i++)
        {
            Vector3 position = transform.position;
            float projectileDirXPosition =
                Mathf.Sin(currentAngle * (float) (Math.PI / 180));
            float projectileDirYPosition =
                Mathf.Cos(currentAngle * (float) (Math.PI / 180));

            bullets.FireBullet(position,
                (new Vector3(projectileDirXPosition, 0, projectileDirYPosition)).normalized, name, this);

            currentAngle += angleStep;
        }
    }
}