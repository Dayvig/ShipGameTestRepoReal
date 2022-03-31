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
    private T3Enemy values3;
    private T4Enemy values4;
    public bool isLeft; 
    public static string BULLET_NAME = "MotorcycleBullet";
    
    public override void MovementUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextWaypoint) < 1)
        {
            if (currentWaypointIndex == 0)
                {
                    SetToWaypoint(1);
                }
                else if (currentWaypointIndex == 1)
                {
                    SetToWaypoint(0);
                }
        }
    }

    public override void SetupEnemy()
    {
        values = GameObject.Find("Model").GetComponent<MotorcycleEnemy>();
        values2 = GameObject.Find("Model").GetComponent<HogEnemy>();
        values3 = GameObject.Find("Model").GetComponent<T3Enemy>();
        values4 = GameObject.Find("Model").GetComponent<T4Enemy>();
        shootInterval = values.fireRate / gameModel.fireRateMultiplier;
        shootTimer = 0f;
        hitPoints = (int)(values.hp * gameModel.healthMultiplier);
        moveSpeed = values.moveSpeed * gameModel.speedMultiplier;
        bulletSpeed = values.bulletSpeed * gameModel.bulletSpeedMultiplier;
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
        bullets.FireBullet(transform.position, (playerModel.ship.transform.position - transform.position).normalized, BULLET_NAME, this);
    }
}