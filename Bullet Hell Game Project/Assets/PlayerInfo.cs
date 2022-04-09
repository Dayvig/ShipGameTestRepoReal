using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public int lives;
    public Controller_ShieldAndHealth SHController;
    public void Start()
    {
        Debug.Log("New scene" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        SHController = GameObject.Find("Controller").GetComponent<Controller_ShieldAndHealth>();
        Debug.Assert(SHController != null, "WHAT");
        SHController.player.livesCurrent = lives;
        DontDestroyOnLoad(this);
    }

    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        Debug.Log("New scene" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        SHController = GameObject.Find("Controller").GetComponent<Controller_ShieldAndHealth>();
        Debug.Assert(SHController != null, "WHAT");
        SHController.player.livesCurrent = lives;
        DontDestroyOnLoad(this);
        
    }
    
}
