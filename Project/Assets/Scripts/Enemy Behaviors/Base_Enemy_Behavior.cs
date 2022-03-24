using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public abstract class Base_Enemy_Behavior : MonoBehaviour
{
    public Model_Player playerModel;
    public Controller_Effects effects;
    public Controller_EnemyBullets bullets;
    public ParticleSystem ps;
    public float hitPoints = 20;
    public float limitHorz = 15;
    public float limitVert = 12;
    public int rate = 360;
    public List<Vector3> Waypoints = new List<Vector3>();
    public Vector3 nextWaypoint;
    public int currentWaypointIndex;

    public float shootInterval;
    public float shootTimer;
    public int behaviorState = 0;

    private void Start()
    {
        playerModel = GameObject.Find("Model").GetComponent<Model_Player>();
        effects = GameObject.Find("Controller").GetComponent<Controller_Effects>();
        bullets = GameObject.Find("Controller").GetComponent<Controller_EnemyBullets>();
        shootTimer = 0;
        if (Waypoints.Count == 0)
        {
            Debug.Log("Empty Waypoints on enemy");
            Waypoints.Add(new Vector3(0, 0, 0));
        }
        nextWaypoint = Waypoints[0];
        currentWaypointIndex = 0;
        SetupEnemy();
    }

    void Update()
    {
        ShootingUpdate();
        UpdateVisuals();
        MovementUpdate();

        var around = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider c in around)
        {
            if (c.gameObject.tag == "PlayerBullet" && !Immune())
            {
                c.gameObject.transform.position += Vector3.forward * 1000;
                hitPoints -= playerModel.damageCurrent;
                ps.Emit(40);
            }
        }
        if (hitPoints <= 0)
        {
            KillThisEnemy();
        }
    }

    //conditions for when an enemy can fire
    public abstract bool canShoot();

    public abstract void UpdateVisuals();

    public abstract void FiringPattern();

    public abstract void MovementUpdate();

    public abstract void SetupEnemy();

    public abstract bool Immune();
    public void ShootingUpdate()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            shootTimer -= shootInterval;
            if (canShoot())
            {
                FiringPattern();
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

    public void KillThisEnemy()
    {
        effects.MakeExplosion(transform.position);
        gameObject.SetActive(false);
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
}