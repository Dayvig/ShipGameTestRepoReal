using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class Guardian_Behavior : Base_Enemy_Behavior
{
    private GuardianEnemy values;
        public static string BULLET_NAME = "GuardianBullet";
        public GameObject thisIndicator;

        public override void MovementUpdate()
        {
            Vector3 toPos = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
            transform.LookAt(toPos, Vector3.forward);
            transform.position = toPos;
            if (thisIndicator != null)
            UpdateIndicator();

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
        
        public override void SpawnIndicator()
        {
            /* Don't
             
            
            Vector3 direction = (playerModel.ship.transform.position - transform.position).normalized;
            if (direction.z < 0)
            {
                thisIndicator =
                    Instantiate(gameModel.indicatorPrefab, transform.position,Quaternion.Euler(90, 0, 90 + Mathf.Acos(direction.z) * 180/Mathf.PI));
                thisIndicator.GetComponent<LaserUpdate>().toFollow = transform.gameObject;
                thisIndicator.GetComponent<LaserUpdate>().duration = shootInterval / 2;
                UpdateIndicator();
            }
            else
            {
                thisIndicator =
                    Instantiate(gameModel.indicatorPrefab, transform.position,Quaternion.Euler(90, 0, 270 + Mathf.Acos(direction.z) * 180/Mathf.PI));
                thisIndicator.GetComponent<LaserUpdate>().toFollow = transform.gameObject;
                thisIndicator.GetComponent<LaserUpdate>().duration = shootInterval / 2;
                UpdateIndicator();
            }

            */
        }

        public void UpdateIndicator()
        {
            
            Vector3 direction = (playerModel.ship.transform.position - transform.position).normalized;
            if (direction.z < 0)
            {
                thisIndicator.transform.rotation = Quaternion.Euler(90, 0, 90 + Mathf.Acos(direction.z) * 180/Mathf.PI);
            }
            else
            {
                thisIndicator.transform.rotation = Quaternion.Euler(90, 0, 180 + Mathf.Acos(direction.z) * 180/Mathf.PI);
            }
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
            base.KillThisEnemy();
            if (inScreen())
            {
                playerModel.score += 1000;
            }
        }
    }
