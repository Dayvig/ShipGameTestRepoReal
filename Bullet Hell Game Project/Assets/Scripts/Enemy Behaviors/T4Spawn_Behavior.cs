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
        for (int i = 0; i < numToSpawn; i++){
            
            Spawn = Instantiate(gameModel.TrailEnemyPrefab); //Spawn the prefab in
        T4Spawn_Behavior tbehavior = TRAIL.GetComponent<TrailEnemy_Behavior>(); //Get its behavior inside its prefab
        stag = getEntrance(trailValues);
        enemycount++;
        tbehavior.nextWaypoint = trailValues.Waypoints[0];
        tbehavior.Waypoints.Add(tbehavior.nextWaypoint);
        tbehavior.Waypoints.Add(trailValues.Waypoints[1]);
        tbehavior.Waypoints.Add(trailValues.Waypoints[2]);
        tbehavior.Waypoints.Add(trailValues.Waypoints[3]);
        tbehavior.Waypoints.Add(trailValues.Waypoints[4]);
        TRAIL.transform.position = tbehavior.nextWaypoint + (stag * i * trailValues.startStagger);
        
    }
    
}