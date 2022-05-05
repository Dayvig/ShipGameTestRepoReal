using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Guardian_Behavior : Base_Enemy_Behavior
{
    private GuardianEnemy values;
        public static string BULLET_NAME = "GuardianBullet";

        public override void MovementUpdate()
        {
            Vector3 toPos = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
            transform.LookAt(toPos, Vector3.forward);
            transform.position = toPos;

            if (Vector3.Distance(transform.position, nextWaypoint) < 1)
            {
                if (currentWaypointIndex == 0)
                {
                    SetToWaypoint(1);
                }
                else
                {
                    SetToWaypoint(0);
                }
            }
        }

        public override void SetupEnemy()
        {
            values = GameObject.Find("Model").GetComponent<GuardianEnemy>();
            shootInterval = values.fireRate / gameModel.fireRateMultiplier;
            shootTimer = Random.Range(0, shootInterval / 2);
            hitPoints = (int) (values.hp * gameModel.healthMultiplier);
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
            bullets.FireBullet(transform.position, (playerModel.ship.transform.position - transform.position).normalized, "Default", this);
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
