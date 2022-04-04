using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class hogEnemy_Behavior : Base_Enemy_Behavior
{
    private HogEnemy values;
    public bool isLeft;
    public static string BULLET_NAME = "Nicebullet";
    public Controller_Enemies enemiescount;
    

    public override void MovementUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerModel.ship.transform.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, nextWaypoint) < 1)
        {
            if (currentWaypointIndex == 0)
            {
                SetToWaypoint(1);
            }
            else if(currentWaypointIndex == 1)
            {
                Destroy(gameObject);
            }
        }
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

    public override void SetupEnemy()
    {
        values = GameObject.Find("Model").GetComponent<HogEnemy>();
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
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}