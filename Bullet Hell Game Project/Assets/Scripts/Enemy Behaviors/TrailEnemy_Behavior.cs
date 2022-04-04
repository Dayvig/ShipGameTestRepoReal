using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrailEnemy_Behavior : Base_Enemy_Behavior
{
    private TrailEnemy values;
    public static string BULLET_NAME = "TrailBullet";
    private float timer;
    public float circleWidth = 3;
    public float circleHeight = 3;
    
    public override void MovementUpdate()
    {
        /*timer += Time.deltaTime;

        float horiRotation = (Mathf.Cos(timer)*circleWidth)+transform.position.x;
        float vertRotation = (Mathf.Sin(timer)*circleHeight)+transform.position.z;
        
        Debug.Log(horiRotation + "x "+vertRotation + "z ");

        toPos.x += horiRotation;
        toPos.z += vertRotation;*/
        
        Vector3 toPos = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
        transform.position = toPos;
        
        if (Vector3.Distance(transform.position, nextWaypoint) < 1)
        {
            if (currentWaypointIndex == Waypoints.Count-1)
            {
                KillThisEnemy();
            }
            else
            {
                SetToNextWaypoint();
            }
        }
    }

    public override void SetupEnemy()
    {
        values = GameObject.Find("Model").GetComponent<TrailEnemy>();
        shootInterval = values.fireRate / gameModel.fireRateMultiplier;
        shootTimer = Random.Range(0, shootInterval/2);
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
        bullets.FireBullet(transform.position, Vector3.back.normalized, BULLET_NAME, this);
    }

    public override void KillThisEnemy()
    {
        if (inScreen())
        {
            effects.MakeExplosion(transform.position);
            gameModel.enemiesKilled++;
            playerModel.score += 1000;
        }
        gameObject.SetActive(false);
    }
    
}