using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public int index;
    public Image cr;
    public Sprite[] images;
    void Start()
    {
        index = 0;
        textComponent.text = String.Empty;
        cr.sprite = images[index];
        StartDialogue();
    }
    void Update()
    {
        if (index == 0)
        {
            cr.sprite = images[0];
        }
        else if (index > 0 && index < 8)
        {
            cr.sprite = images[1];
        }
        else if (index == 8)
        {
            cr.sprite = images[2];
        }
        else
        {
            cr.sprite = images[3];
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = String.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }
    }
   
}
