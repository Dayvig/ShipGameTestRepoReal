using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    // Start is called before the first frame update

    public float whateverTimer;
    public float whateverInterval;
    
    void Start()
    {
        whateverInterval = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        whateverTimer += Time.deltaTime;
        if (whateverTimer > whateverInterval)
        {
            Destroy(this.gameObject);
        }
    }
}
