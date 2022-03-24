using System;
using System.Collections;
using System.Collections.Generic;
using Collectibles;
using Controllers;
using UnityEngine;

public class Gas_Canister : Base_Collectible_Behavior
{
    public Controller_Fuel controllerFuel;
    // Start is called before the first frame update
    private bool collected = false;
    // Update is called once per frame
    public GascanCollectible values;
    public override void MovementUpdate()
    {
        gameObject.transform.position -= new Vector3(0,0,0.01f);
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
    }

    public override void Collect()
    {
        controllerFuel.SetFuel(controllerFuel.currentFuel+5);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        collected = true;
    }
}
