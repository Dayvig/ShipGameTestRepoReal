using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFireEnemy_Behavior : Base_Enemy_Behavior
{
        private RapidEnemy values;
        public static string BULLET_NAME = "RapidBullet";
        public float volleyTimer = 0;
        public float volleyInterval;
        public int volleys;
        
        //Behavior 0 - Waiting to fire
        //Behavior 1 - Volleying
        
        public override void MovementUpdate()
        {
            Vector3 toPos = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
            transform.position = toPos;

            if (Vector3.Distance(transform.position, nextWaypoint) < 1)
            {
                if (currentWaypointIndex != Waypoints.Count - 1)
                {
                    SetToNextWaypoint();
                }
            }
        }

        public override void SetupEnemy()
        {
            values = GameObject.Find("Model").GetComponent<RapidEnemy>();
            shootInterval = values.fireRate / gameModel.fireRateMultiplier;
            shootTimer = Random.Range(-shootInterval / 2, 0);
            hitPoints = (int) (values.hp * gameModel.healthMultiplier);
            moveSpeed = values.moveSpeed * gameModel.speedMultiplier;
            bulletSpeed = values.bulletSpeed * gameModel.bulletSpeedMultiplier;
            volleyInterval = values.volleyInterval / gameModel.fireRateMultiplier;
            behaviorState = 0;
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

        public override void ShootingUpdate()
        {
            if (behaviorState == 0)
            {
                shootTimer += Time.deltaTime;

                if (shootTimer >= shootInterval)
                {
                    behaviorState = 1;
                    shootTimer -= shootInterval;
                    volleys = values.volleyBullets;
                }
            }
            else
            {
                volleyTimer += Time.deltaTime;

                if (volleyTimer >= volleyInterval)
                {
                    if (canShoot())
                    {
                        FiringPattern();
                    }

                    volleyTimer -= volleyInterval;
                    volleys--;
                    if (volleys <= 0)
                    {
                        behaviorState = 0;
                    }
                }
            }
        }

        public override void FiringPattern()
        {
            bullets.FireBullet(transform.position, Vector3.back.normalized, "Default", this); 
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