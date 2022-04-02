using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class T3Enemy_Behavior : Base_Enemy_Behavior
{
    private T3Enemy values;
    public static string BULLET_NAME = "T3Bullet";
    private float timer;
    private float chargeTimer;
    public float chargeTime;
    public bool charging = false;
    public float baseExplosionInterval;
    public float explosionInterval;
    public float explosionTimer;
    public int chargingState = 0;

    public override void MovementUpdate()
    {
        Vector3 toPos = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
        transform.position = toPos;
        
        if (Vector3.Distance(transform.position, nextWaypoint) < 1)
        {
            if (currentWaypointIndex != Waypoints.Count-1)
            {
                SetToNextWaypoint();
            }
        }
    }

    public override void ShootingUpdate()
    {
        if (currentWaypointIndex == Waypoints.Count-1){
            
        if (!charging)
        {
            shootTimer += Time.deltaTime;
            explosionInterval = baseExplosionInterval;
            
            if (shootTimer >= shootInterval)
            {
                charging = true;
                shootTimer -= shootInterval;
            }
        }
        else
        {
            chargeTimer += Time.deltaTime;
            explosionTimer += Time.deltaTime;
            if (chargeTimer >= chargeTime / 2)
            {
                if (chargingState == 0)
                {
                    explosionInterval /= 4;
                    chargingState = 1;
                }
            }
            else if (chargeTimer > (chargeTime * 3) / 4)
            {
                if (chargingState == 1)
                {
                    explosionInterval /= 8;
                    chargingState = 2;
                }
            }

            if (explosionTimer >= explosionInterval)
            {
                if (chargingState < 2)
                {
                    effects.MakeSmallExplosion(transform.position);
                }
                else
                {
                    effects.MakeExplosion(transform.position);
                }

                //Debug.Log("Boom");
                explosionTimer -= explosionInterval;
            }

            if (chargeTimer >= chargeTime)
            {
                FiringPattern();
                charging = false;
                chargeTimer -= chargeTime;
                chargingState = 0;
            }
            
            }
        }
    }

    public override void SetupEnemy()
    {
        values = GameObject.Find("Model").GetComponent<T3Enemy>();
        shootInterval = values.fireRate / gameModel.fireRateMultiplier;
        shootTimer = Random.Range(0, shootInterval / 2);
        hitPoints = (int)(values.hp * gameModel.healthMultiplier);
        moveSpeed = values.moveSpeed * gameModel.speedMultiplier;
        bulletSpeed = values.bulletSpeed * gameModel.bulletSpeedMultiplier;
        chargeTime = values.chargeTime / gameModel.fireRateMultiplier;
        explosionInterval = baseExplosionInterval = chargeTime / 10;
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
        
        bullets.FireBullet(transform.position + Vector3.left * 1, Vector3.back.normalized, BULLET_NAME, this);
        bullets.FireBullet(transform.position + Vector3.left * 2, Vector3.back.normalized, BULLET_NAME, this);
        bullets.FireBullet(transform.position + Vector3.left * 3, Vector3.back.normalized, BULLET_NAME, this);
        bullets.FireBullet(transform.position + Vector3.left * 4, Vector3.back.normalized, BULLET_NAME, this);
        bullets.FireBullet(transform.position + Vector3.left * 5, Vector3.back.normalized, BULLET_NAME, this);
        bullets.FireBullet(transform.position + Vector3.left * 6, Vector3.back.normalized, BULLET_NAME, this);

    }
}