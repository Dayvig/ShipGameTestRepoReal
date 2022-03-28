using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Canister_spawner : MonoBehaviour
{
    public float elapsed = 0f;
    public GameObject canister;
    
    private void Update()
    {
        elapsed += Time.deltaTime;
        if (!(elapsed >= 5f)) return;
        elapsed %= 5f;
        MakeGas();
    }

    void MakeGas()
    {
        Debug.Log("HERE");
         Instantiate(canister, new Vector3(Random.Range(-10,10),gameObject.transform.position.y,transform.position.z), Quaternion.identity);
    }

}
