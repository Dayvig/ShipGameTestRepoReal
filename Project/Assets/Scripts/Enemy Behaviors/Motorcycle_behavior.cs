using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class Motorcycle_behavior : Base_Enemy_Behavior
{
    private MotorcycleEnemy values;
    private HogEnemy values2;
    public bool isLeft; 
    public static string BULLET_NAME = "MotorcycleBullet";
    
    public override void MovementUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, values.moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextWaypoint) < 1 && currentWaypointIndex != Waypoints.Count-1)
        {
            SetToNextWaypoint();
        }
        shootInterval = values.fireRate;
    }

    public override void SetupEnemy()
    {
        values = GameObject.Find("Model").GetComponent<MotorcycleEnemy>();
        values2 = GameObject.Find("Model").GetComponent<HogEnemy>();
        shootInterval = values.fireRate;
        shootTimer = Random.Range(0, shootInterval);
        hitPoints = values.hp;
        hitPoints = values2.hp;
    }

    public override bool Immune()
    {
        return false;
    }

    public override bool canShoot()
    {
        if (inScreen())
        {
            return true;
        }

        return false;
    }

    public override void UpdateVisuals()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rate);
    }

    public override void FiringPattern()
    {
        bullets.FireBullet(transform.position, (playerModel.ship.transform.position - transform.position).normalized, BULLET_NAME);
    }
}