using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_PlayerShip : MonoBehaviour
{
    public Model_Player playerModel;
    public Material invincibleColor;
    public Material normalColor;

    public MeshRenderer[] playerShipObjects = new MeshRenderer[3];

    private bool shiftHeld;
    private void Start()
    {
        Debug.Assert(playerModel != null, "Controller_PlayerShip is looking for a reference to Model_Player, but none has been added in the Inspector!");
        playerShipObjects[0] = GameObject.Find("Cockpit").GetComponent<MeshRenderer>();
        playerShipObjects[1] = GameObject.Find("Cube").GetComponent<MeshRenderer>();
        playerShipObjects[2] = GameObject.Find("Fuselage").GetComponent<MeshRenderer>();
        playerModel.hitpointsCurrent = playerModel.hitpointsBase;
        //playerModel.livesCurrent = playerModel.livesBase;
    }

    private void Update()
    {
        _ApplySmoothingToMotion();
        if (playerModel.invincible)
        {
            //playerModel.
            playerShipObjects[0].material = invincibleColor;
            playerShipObjects[1].material = invincibleColor;
            playerShipObjects[2].material = invincibleColor;
            playerModel.shieldActive = true;
        }
        else
        {
            playerShipObjects[0].material = normalColor;
            playerShipObjects[1].material = normalColor;
            playerShipObjects[2].material = normalColor;
            playerModel.shieldActive = false;

        }
    }

    public void ShipUpdate()
    {
        _TakeInputs();
        _LimitToScreen();
    }

    public void ForceShipPos(Vector3 where)
    {
        playerModel.positionCurrent = playerModel.positionTarget = where;
    }
    public void ForceShipRot(float angle)
    {
        playerModel.rotationCurrent = playerModel.rotationTarget = angle;
    }

    private void _TakeInputs()
    {
        float shiftSlowDown;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftHeld = true;
            shiftSlowDown = 1 / playerModel.shiftTurningFactor;
        }
        else
        {
            shiftHeld = false;
            shiftSlowDown = 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            playerModel.positionTarget +=
                Vector3.forward * Time.deltaTime * (playerModel.shipSpeed * (1 - playerModel.vFactor)) * shiftSlowDown;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerModel.positionTarget -=
                Vector3.forward * Time.deltaTime * (playerModel.shipSpeed * (1 + playerModel.vFactor)) * shiftSlowDown;
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerModel.positionTarget -= Vector3.right * Time.deltaTime * playerModel.shipSpeed * shiftSlowDown;
            playerModel.rotationCurrent = setRotation(false, shiftHeld);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerModel.positionTarget += Vector3.right * Time.deltaTime * playerModel.shipSpeed * shiftSlowDown;
            playerModel.rotationCurrent = setRotation(true, shiftHeld);
        }

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            if (!shiftHeld)
            {
                playerModel.rotationCurrent = stabilizeRotation();
            }
        }
        
        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
}

    private float setRotation(bool r, bool s)
    {
        float toTurn = playerModel.rotationSpeed;
        if (s)
        {
        toTurn /= (playerModel.shiftTurningFactor/2);
        }
        if (r)
        {
            if (playerModel.rotationCurrent < playerModel.turnLimit)
            {
                return playerModel.rotationCurrent += toTurn;
            }
            return playerModel.turnLimit;
        }
        
        if (playerModel.rotationCurrent > -playerModel.turnLimit)
            {
                return playerModel.rotationCurrent -= toTurn;
            }
        return -playerModel.turnLimit;
    }

    private float stabilizeRotation()
    {
        if (playerModel.rotationCurrent < 5 && playerModel.rotationCurrent > -5)
        {
            ForceShipRot(0);
            return 0;
        }
        if (playerModel.rotationCurrent > 0)
        {
            return playerModel.rotationCurrent -= playerModel.rotationSpeed/2;
        }
        else
        {
            return playerModel.rotationCurrent += playerModel.rotationSpeed/2;
        }
    }

    private void _LimitToScreen()
    {
        if (playerModel.positionTarget.x < -playerModel.limitHorz)
            playerModel.positionTarget.x = -playerModel.limitHorz;
        if (playerModel.positionTarget.x > playerModel.limitHorz)
            playerModel.positionTarget.x = playerModel.limitHorz;

        if (playerModel.positionTarget.z < -playerModel.limitVert)
            playerModel.positionTarget.z = -playerModel.limitVert;
        if (playerModel.positionTarget.z > playerModel.limitVert)
            playerModel.positionTarget.z = playerModel.limitVert;
    }
    private void _ApplySmoothingToMotion()
    {
        
        playerModel.positionCurrent = Vector3.Lerp(
            playerModel.positionCurrent, 
            playerModel.positionTarget, 
            Time.deltaTime * playerModel.smoothingFactor);

        
        
        playerModel.actualRotation = Vector3.Lerp(
            new Vector3(0, playerModel.rotationCurrent, 0), 
            new Vector3(0, playerModel.rotationTarget, 0),
            Time.deltaTime * playerModel.smoothingFactor);
        
        playerModel.ship.transform.position = playerModel.positionCurrent;
        playerModel.shield.transform.position = playerModel.ship.transform.position;
        playerModel.ship.transform.rotation = Quaternion.Euler(playerModel.actualRotation);
    }

    public void UpgradeGuns()
    {
        playerModel.fireRate /= 2;
    }
}
