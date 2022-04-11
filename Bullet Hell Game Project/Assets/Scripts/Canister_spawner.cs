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
        if (!(elapsed >= 10f)) return;
        elapsed %= 10f;
        MakeGas();
    }

    void MakeGas()
    {
         Instantiate(canister, new Vector3(Random.Range(-10,10),gameObject.transform.position.y,transform.position.z+11), Quaternion.identity);
    }

}
