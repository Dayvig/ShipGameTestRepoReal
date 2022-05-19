using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy_Behaviors
{
    public class FastEnemy_Behavior : Base_Enemy_Behavior
    {
        private FastEnemy values;
        public static string BULLET_NAME = "FastBullet";
        public bool canMove;
        public GameObject thisIndicator;

        public override void MovementUpdate()
        {
            if (!canMove)
                return;
            
                Vector3 toPos = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
            transform.LookAt(toPos, Vector3.forward);
            transform.position = toPos;
            
            if (Vector3.Distance(transform.position, nextWaypoint) < 1)
            {
                if (currentWaypointIndex == Waypoints.Count - 1)
                {
                    KillThisEnemy();
                }
                else
                {
                    SetToNextWaypoint();
                }
            }
        }
        
        public override void ShootingUpdate()
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

        public override void SetupEnemy()
        {
            values = GameObject.Find("Model").GetComponent<FastEnemy>();
            shootInterval = values.fireRate / gameModel.fireRateMultiplier;
            shootTimer = Random.Range(0, shootInterval / 2);
            hitPoints = (int) (values.hp * gameModel.healthMultiplier);
            moveSpeed = values.moveSpeed * gameModel.speedMultiplier;
            bulletSpeed = values.bulletSpeed * gameModel.bulletSpeedMultiplier;
            canMove = false;
            Invoke("makeThisMove", 2f);
            SpawnIndicator();
        }

        public void makeThisMove()
        {
            canMove = true;
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
            bullets.FireBullet(transform.position, (playerModel.ship.transform.position - transform.position).normalized, "Default", this);
        }
        
        public override void SpawnIndicator()
        {
            if (behaviorState == 0)
            {
                Instantiate(gameModel.fastindicatorPrefab, new Vector3(15, 0, 8), Quaternion.Euler(90, 0, 0));
            }
            else
            {
                Instantiate(gameModel.fastindicatorPrefab, new Vector3(-15, 0, 8), Quaternion.Euler(90, 0, 0));
            }
        }

        public override void KillThisEnemy()
        {
            base.KillThisEnemy();
            if (inScreen())
            {
                playerModel.score += 1000;
            }
        }
    }
}