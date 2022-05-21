using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpdate : MonoBehaviour
{
    public Sprite[] images;
    private int index;
    public Image cr;


    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        cr.sprite = images[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
        
        if (index > images.Length-1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            index++;
            if (index <= images.Length-1)
            cr.sprite = images[index];
        }
    }
}
