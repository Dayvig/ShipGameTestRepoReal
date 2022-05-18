using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fastIndicatorUpdate : MonoBehaviour
{

    public float flickerTimer;
    public float flickerInterval;
    public float totalTimer;
    public SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        flickerTimer = 0;
        totalTimer = 0;
        flickerInterval = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        flickerTimer += Time.deltaTime;
        totalTimer += Time.deltaTime;
        if (flickerTimer > flickerInterval)
        {
            sr.enabled = !sr.enabled;
            flickerTimer -= flickerInterval;
        }

        if (totalTimer > 2f)
        {
            Destroy(this.gameObject);
        }
    }
}
