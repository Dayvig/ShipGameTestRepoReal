using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Sound : MonoBehaviour
{
    public AudioSource boomSource;
    public List<AudioClip> BOOOM = new List<AudioClip>();

    private void Update()
    {
        
    }
    public void boom() {
        int r = Random.Range(0, BOOOM.Count);
        AudioClip clip = BOOOM[r];
        boomSource.clip = clip;
        boomSource.Play();
    }
}
