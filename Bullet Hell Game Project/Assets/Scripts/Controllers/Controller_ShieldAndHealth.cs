using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Controllers;
using UnityEngine;

public class Controller_ShieldAndHealth : MonoBehaviour
{
    public Model_Player player;
    public Controller_EnemyBullets bullets;
    public Controller_Fuel controllerFuel;

    private float shieldRegenTimer;
    private bool firstSpawn = true;
    public float invincibleTimer;
    public float fuelShieldDuration;
    public bool fuelShieldActive;

    public PlayerInfo savedInfo;
    
    void Start()
    {
        Debug.Assert(player != null, "Controller_ShieldAndHealth is looking for a reference to Model_Player, but none has been added in the Inspector!");
        controllerFuel = GameObject.Find("Controller").GetComponent<Controller_Fuel>();
        fuelShieldActive = false;
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
        _ShieldOnOff();
        if (fuelShieldActive)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer > fuelShieldDuration)
            {
                fuelShieldActive = false;
                player.invincible = false;
                invincibleTimer -= fuelShieldDuration;
            }
        }

        // Collision Detection
        float radius = 0;
        radius = player.unshieldedRadius;
        
        var colliders = Physics.OverlapSphere(player.ship.transform.position, radius);
        
        foreach (var c in colliders)
        {
            if (c.gameObject.tag == "Enemy" && !player.invincible)
            {
                if (!TriggerFuelShield())
                {
                    if (!player.lostLife)
                    {
                        player.livesCurrent--;
                        player.lostLife = true;
                    }

                    player.hitpointsCurrent = 0;
                }
            }
            else if (c.gameObject.tag == "EnemyBullet" && !player.invincible)
            {
                if (!TriggerFuelShield())
                {
                    if (!player.lostLife)
                    {
                        player.livesCurrent--;
                        player.lostLife = true;
                    }

                    player.hitpointsCurrent = 0;
                }
                    bullets.KillBullet(c.gameObject);
            }
        }
    }

    public bool TriggerFuelShield()
    {
        /*if (controllerFuel.currentFuel > (controllerFuel.FuelMax * 0.6f) && !player.invincible)
        {
            fuelShieldActive = true;
            player.invincible = true;
            controllerFuel.currentFuel -= controllerFuel.FuelMax * 0.4f;
        }
        return fuelShieldActive;*/
        return false;
    }

    private void _ShieldOnOff()
    {
        if (player.shieldActive)
            player.shield.SetActive(true);
        else
            player.shield.SetActive(false);
    }
}
