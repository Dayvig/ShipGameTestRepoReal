using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class T4Enemy_Behavior : Base_Enemy_Behavior
{
    private T4Enemy values;
    public static string BULLET_NAME = "T4Bullet";
    private T4SpawnEnemy spawnValues;

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


    public override void SetupEnemy()
    {
        values = GameObject.Find("Model").GetComponent<T4Enemy>();
        spawnValues = GameObject.Find("Model").GetComponent<T4SpawnEnemy>();
        shootInterval = values.fireRate / gameModel.fireRateMultiplier;
        shootTimer = Random.Range(0, shootInterval / 2);
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
        bullets.FireBullet(transform.position, (playerModel.ship.transform.position - transform.position).normalized , BULLET_NAME, this);
    }
    
    public override void KillThisEnemy()
    {
        effects.MakeExplosion(transform.position);
        gameModel.enemiesKilled++;
        playerModel.score += 1000;
        gameObject.SetActive(false);
    
        SpawnEnemy(values.enemiesToSpawn);
    }

    private void SpawnEnemy(int numToSpawn)
    {
        GameObject Spawn;
        for (int i = 0; i < numToSpawn; i++)
        {
            Spawn = Instantiate(gameModel.T4EnemySpawnPrefab); //Spawn the prefab in
            T4Spawn_Behavior tsbehavior = Spawn.GetComponent<T4Spawn_Behavior>(); //Get its behavior inside its prefab
            Vector3 spawnPoint = transform.position;
            float angle = Random.Range(0, 360);
            float radius = values.radius;
            Vector3 toGoTo = spawnPoint + new Vector3(Mathf.Sin(angle) * (float)(Math.PI / 180), 0f, Mathf.Cos(angle) * (float)(Math.PI / 180)).normalized * radius;
            tsbehavior.nextWaypoint = toGoTo;
            tsbehavior.Waypoints[0] = (tsbehavior.nextWaypoint);
            Spawn.transform.position = spawnPoint;
        }
    }

}