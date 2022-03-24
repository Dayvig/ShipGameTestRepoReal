using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_FadeFromBlack : MonoBehaviour
{
    public SpriteRenderer fader;
    float target;
    Color fadeColor;

    private void Start()
    {
        target = 1;
        fadeColor = Color.black;
        fadeColor.a = target;
    }

    void Update()
    {
        target -= Time.deltaTime;
        target = Mathf.Clamp01(target);

        fadeColor.a = target;
        fader.color = fadeColor;
    }

    public void FadeToBlack()
    {
        target = 1;
    }
    public void FadeFromBlack()
    {
        target = 0;
    }
}
