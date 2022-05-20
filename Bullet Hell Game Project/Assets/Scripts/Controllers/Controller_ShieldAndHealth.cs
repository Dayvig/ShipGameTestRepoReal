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
    public AudioClip Death;
    public AudioClip Shield;
    private bool soundIsOn;
    private bool shieldIsOn
    {
        get
        {
            return soundIsOn;
        }
        set
        {
            Debug.Log("Value is set to " + value + " and soundIsOn is set to " + soundIsOn + "." + "First spawn is set to " + firstSpawn);

            if (value != soundIsOn)
            {
                soundIsOn = value;
                if (soundIsOn == true && firstSpawn == false)
                {
                    Debug.Log("Audio clip of shield is being played.");
                    AudioSource.PlayClipAtPoint(Shield, transform.position);
                }
            }
        }
    }
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
        
        var colliders = Physics.OverlapCapsule(player.ship.transform.position + Vector3.forward * 1.3f,
            player.ship.transform.position + Vector3.forward * -0.8f , 0.1f);
        
        
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
                AudioSource.PlayClipAtPoint(Death, transform.position);
                bullets.KillBullet(c.gameObject);
            }
        }
    }

    public bool TriggerFuelShield()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("BossScene"))
        {
            if (Input.GetKeyDown(KeyCode.F) && controllerFuel.currentFuel > (controllerFuel.FuelMax * 0.3f) &&
                player.invincible == false)
            {
                fuelShieldActive = true;
                player.invincible = true;
                controllerFuel.currentFuel -= controllerFuel.FuelMax * 0.2f;
            }

            /*if (controllerFuel.currentFuel > (controllerFuel.FuelMax * 0.6f) && !player.invincible)
            {
                fuelShieldActive = true;
                player.invincible = true;
                controllerFuel.currentFuel -= controllerFuel.FuelMax * 0.4f;
            }
            */
            return fuelShieldActive;
        }
        else
        {
            return false;
        }
    }

    private void _ShieldOnOff()
    {
        if (player.shieldActive)
        {
            player.shield.SetActive(true);
            shieldIsOn = true;
            firstSpawn = false;
        }
        else
        {
            player.shield.SetActive(false);
            shieldIsOn = false;
        }
    }
}
