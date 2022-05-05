using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Collectibles;
using Controllers;
using UnityEngine;

public class Gas_Canister : Base_Collectible_Behavior
{
    public float elapsed = 0f;
    private float startTime;
    public Color primary, secondary;
    private Color currentColor;

    private MeshRenderer canisterRenderer;
    
    public Controller_Fuel controllerFuel;
    // Start is called before the first frame update
    private bool collected = false;
    // Update is called once per frame
    public GascanCollectible values;
    public override void MovementUpdate()
    {
        gameObject.transform.position -= new Vector3(0,0,values.movementSpeed) * Time.deltaTime;
        if (transform.position.z <= -11)
        {
            Destroy(this.gameObject);
        }    
    }

    public override void SetupCollectible()
    {
        controllerFuel = GameObject.Find("Controller").GetComponent<Controller_Fuel>();
        values = GameObject.Find("Model").GetComponent<GascanCollectible>();
    }

    public override bool CollectionCondition()
    {
        return collected;
    }

    public override void UpdateVisuals()
    {
        float t = (Mathf.Sin(2*(Time.time - GameObject.Find("Controller").GetComponent<Controller_Fuel>().startTime)));
        GetComponent<MeshRenderer>().material.color = Color.Lerp(primary, secondary, t);
        
    }

    public override void Collect()
    {
        controllerFuel.SetFuel(controllerFuel.currentFuel+values.fuelRestore);
        if (controllerFuel.currentFuel > controllerFuel.FuelMax)
        {
            controllerFuel.SetFuel(controllerFuel.FuelMax);
        }
        controllerFuel.UpdateColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        collected = true;
    }
}
