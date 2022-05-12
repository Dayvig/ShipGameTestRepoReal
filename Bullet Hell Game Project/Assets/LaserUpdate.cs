using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LaserUpdate : MonoBehaviour
{

    public float duration;
    public float currentTime;
    public SpriteRenderer r;
    public Color alphaColor;
    public bool flare;
    public GameObject toFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        flare = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = toFollow.transform.position;
        currentTime += Time.deltaTime;
        if (currentTime > duration)
        {
            flare = true;
        }
        if (flare && currentTime > duration)
        {
            var rColor = Color.yellow;
            r.color = rColor;
            if (currentTime > duration + duration / 8)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            var rColor = r.color;
            rColor.a = currentTime / duration;
            r.color = rColor;
        }
    }
}
