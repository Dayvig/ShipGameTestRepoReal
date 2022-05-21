using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneManager : MonoBehaviour
{
     
    void Start()
    {
        
    }

  
    public void LoadGame()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
