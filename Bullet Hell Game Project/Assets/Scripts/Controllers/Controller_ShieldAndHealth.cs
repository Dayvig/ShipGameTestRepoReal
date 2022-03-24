using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Controller_ShieldAndHealth : MonoBehaviour
{
    public Model_Player player;
    public Controller_EnemyBullets bullets;

    private float shieldRegenTimer;
    private bool firstSpawn = true;
    void Start()
    {
        Debug.Assert(player != null, "Controller_ShieldAndHealth is looking for a reference to Model_Player, but none has been added in the Inspector!");
    }

    public void OnSpawn()
    {
        Debug.Log("OnSpawn Called");
        player.hitpointsCurrent = player.hitpointsBase;
        player.shieldPointsMax = player.shielddPointsCurrent = player.shieldpointsBase;
        player.shieldRegenIntervalCurrent = player.shieldRegenIntervalBase;
        player.shieldDurationCurrent = player.shieldDurationBase;
        player.shieldCooldownCurrent = player.shieldCooldownBase;
        if (firstSpawn)
        {
            player.livesCurrent = player.livesBase;
            firstSpawn = false;
        }

        player.lostLife = false;
    }

    public void ShieldAndHealthUpdate()
    {
        // Inputs
        /*if (Input.GetKey(KeyCode.LeftShift) && !player.shieldActive && shieldRegenTimer == 0f)
        {
            player.shieldActive = true;
            player.shielddPointsCurrent = player.shieldDurationCurrent;
            shieldRegenTimer = player.shieldDurationCurrent + player.shieldCooldownCurrent;
        }
        if (player.shielddPointsCurrent <= 0)
        {
            player.shieldActive = false;
            player.shielddPointsCurrent = 0;
        }

        
        player.shielddPointsCurrent -= Time.deltaTime;
        shieldRegenTimer -= Time.deltaTime;
        
        if (shieldRegenTimer < 0f)
        {
            shieldRegenTimer = 0f;
        }*/
        
        // Update Model
        _ShieldOnOff();

        // Collision Detection
        float radius = 0;
        /*if (player.shieldActive)
            radius = player.shieldedRadius;
        else*/
            radius = player.unshieldedRadius;
        
        var colliders = Physics.OverlapSphere(player.ship.transform.position, radius);
        
        foreach (var c in colliders)
        {
            if (c.gameObject.tag == "Enemy" && !player.invincible)
            {
                /*if (player.shieldActive)
                {
                    player.shielddPointsCurrent -= 3;
                    player.shielddPointsCurrent = (int)Mathf.Max(player.shielddPointsCurrent, 0);
                    shieldRegenTimer = 0;
                }
                else*/
                if (!player.lostLife)
                {
                    player.livesCurrent--;
                    player.lostLife = true;
                }
                player.hitpointsCurrent = 0;

                    //Behavior_Enemy1 e = c.GetComponent<Behavior_Enemy1>();
                    //e.KillThisEnemy();
            }
            else if (c.gameObject.tag == "EnemyBullet" && !player.invincible)
            {
                /*if (player.shieldActive)
                {
                    player.shielddPointsCurrent--;
                    player.shielddPointsCurrent = (int)Mathf.Max(player.shielddPointsCurrent, 0);
                    shieldRegenTimer = 0;
                }
                else*/
                if (!player.lostLife)
                {
                    player.livesCurrent--;
                    player.lostLife = true;
                }
                player.hitpointsCurrent = 0;

                bullets.KillBullet(c.gameObject);
            }
        }
        /*
        if (player.shielddPointsCurrent < player.shieldPointsMax)
        {
            shieldRegenTimer += Time.deltaTime;
            if (shieldRegenTimer >= player.shieldRegenIntervalCurrent)
            {
                shieldRegenTimer = 0;
                player.shielddPointsCurrent++;
            }
        }*/
    }

    private void _ShieldOnOff()
    {
        if (player.shieldActive)
            player.shield.SetActive(true);
        else
            player.shield.SetActive(false);
    }
}
