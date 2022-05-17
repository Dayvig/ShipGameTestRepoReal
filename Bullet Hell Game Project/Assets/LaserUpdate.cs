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
    public float flashTime;
    public float flashCtr;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        flare = false;
        flashTime = duration / 4;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = toFollow.transform.position;
        LaserUpdate1();
        //LaserUpdate2();
        if (toFollow == null || toFollow.gameObject.activeSelf == false)
        {
            Destroy(gameObject);
        }
    }

    void LaserUpdate1()
    {
        currentTime += Time.deltaTime;
        if (currentTime > duration)
        {
            flare = true;
        }
        if (flare && currentTime > duration)
        {
            var rColor = Color.red;
            r.color = rColor;
            if (currentTime > duration + duration / 8)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            var rColor = r.color;
            rColor.a = (currentTime / duration) * 0.75f;
            r.color = rColor;
        }
    }

    void LaserUpdate2()
    {
        currentTime += Time.deltaTime;
        flashCtr += Time.deltaTime;
        if (flashCtr < flashTime)
        {
            var rColor = Color.red;
            rColor.a = 0.4f;
            r.color = rColor;
        }
        else if (flashCtr > flashTime && flashCtr < flashTime * 2)
        {
            var rColor = Color.clear;
            r.color = rColor;
        }
        else if (flashCtr > flashTime * 2)
        {
            flashCtr = 0;
            flashTime /= 2f;
        }

        if (currentTime >= duration * 9 / 10)
        {
            var rColor = Color.red;
            r.color = rColor;
        }
        
        if (currentTime > duration + duration/10)
        {
            Destroy(gameObject);
        }
    }
}
