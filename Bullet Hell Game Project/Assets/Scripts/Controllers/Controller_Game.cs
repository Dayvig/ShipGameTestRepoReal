using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class Controller_Game : MonoBehaviour
{
    [Header("Model References")]
    public Model_Player playerModel;
    public Model_Game gameModel;
    [Header("Controller References")]
    public Controller_PlayerShip shipController;
    public Controller_PlayerGuns gunController;
    public Controller_Enemies enemyController;
    public Controller_Fuel controllerFuel;
    public Controller_ShieldAndHealth shieldAndHealthController;
    [Header("View References")]
    public View_FadeFromBlack fadeView;
    public View_IntroNarrative narrative;
    [Header("Current State")]
    [SerializeField] string stateReadout;

    private float spawnInvincibilityTimer;
    private bool SpawnInvincibility = true;
    
    //---------------------------------------------------------
    // NOTE: Game will start in the following sequence:
    // 1) Intro
    // 2) Dialogue
    // 3) Spawn
    // NOTE: The game will loop through the following sequence
    //  until Game Over or Win
    // 4) Play
    // 5) Die
    // 6) Respawn
    // NOTE: The game will end when either Win or Game Over
    // 7*) Win
    // 7*) Game Over
    //---------------------------------------------------------

    private GameStates currentState = GameStates.Standby;
    internal void SetGameState(GameStates newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (newState)
        {
            case GameStates.Standby:
                stateReadout = newState.ToString();
                break;
            case GameStates.Intro:
                stateReadout = newState.ToString();
                _OnIntro();
                break;
            case GameStates.Dialogue:
                stateReadout = newState.ToString();
                _OnDialogue();
                break;
            case GameStates.Spawn:
                stateReadout = newState.ToString();
                _OnSpawn();
                break;
            case GameStates.Respawn:
                stateReadout = newState.ToString();
                _OnRespawn();
                break;
            case GameStates.Play:
                stateReadout = newState.ToString();
                _OnPlay();
                break;
            case GameStates.Die:
                stateReadout = newState.ToString();
                _OnDie();
                break;
            case GameStates.Win:
                stateReadout = newState.ToString();
                _OnWin();
                break;
            case GameStates.GameOver:
                stateReadout = newState.ToString();
                _OnGameOver();
                break;
            default:
                Debug.Log("Task error:" + currentState);
                break;
        }
    }

    void Awake()
    {
        Debug.Assert(playerModel != null, "Controller_Game is looking for a reference to Model_Player, but none has been added in the Inspector!");
        Debug.Assert(gameModel != null, "Controller_Game is looking for a reference to Model_Game, but none has been added in the Inspector!");
        Debug.Assert(shipController != null, "Controller_Game is looking for a reference to Controller_PlayerShip, but none has been added in the Inspector!");
        Debug.Assert(gunController != null, "Controller_Game is looking for a reference to Controller_PlayerGun, but none has been added in the Inspector!");
        Debug.Assert(shieldAndHealthController != null, "Controller_Game is looking for a reference to Controller_ShieldAndHealth, but none has been added in the Inspector!");
        Debug.Assert(fadeView != null, "Controller_Game is looking for a reference to View_FadeFromBlack, but none has been added in the Inspector!");
        Debug.Assert(controllerFuel != null, "Controller_Game is looking for a reference to View_FadeFromBlack, but none has been added in the Inspector!");


        playerModel.damageCurrent = playerModel.damageBase;

        playerModel.positionTarget = playerModel.positionCurrent = playerModel.positionSpawnStart;

        playerModel.ship = GameObject.Instantiate(playerModel.shipPrefab);
        playerModel.ship.transform.position = playerModel.positionCurrent;
        
        playerModel.shield = GameObject.Instantiate(playerModel.shieldPrefab);
        playerModel.shield.transform.position = playerModel.positionCurrent;

        currentState = GameStates.Spawn;
        _OnSpawn();
        
    }

    void Update()
    {
        if (currentState == GameStates.Intro)
            _IntroUpdate();
        else if (currentState == GameStates.Dialogue)
            _DialogueUpdate();
        else if (currentState == GameStates.Spawn)
            _SpawnUpdate();
        else if (currentState == GameStates.Respawn)
            _RespawnUpdate();
        else if (currentState == GameStates.Play)
            _PlayUpdate();
        else if (currentState == GameStates.Die)
            _DieUpdate();
        else if (currentState == GameStates.Win)
            _WinUpdate();
        else if (currentState == GameStates.GameOver)
            _GameOverUpdate();
    }

    #region On State Change
    private void _OnIntro()
    {
        fadeView.FadeFromBlack();
    }
    private void _OnDialogue()
    {
        narrative.StartDialogue();
    }
    private void _OnSpawn()
    {
        narrative.CleanupNarrative();
        shieldAndHealthController.OnSpawn();
        shipController.ForceShipPos(playerModel.positionSpawnStart);
        spawnTimer = 0;
    }
    private void _OnRespawn()
    {
        shieldAndHealthController.OnSpawn();
        shipController.ForceShipPos(playerModel.positionSpawnStart);
        spawnTimer = 0;
        playerModel.hitpointsCurrent = playerModel.hitpointsBase;
    }
    private void _OnPlay()
    {

    }
    private void _OnDie()
    {
        if (playerModel.livesCurrent < 1)
        {
            SetGameState(GameStates.GameOver);
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        }
    }
    private void _OnWin()
    {

    }
    private void _OnGameOver()
    {

    }
    #endregion

    #region State Mains
    private float introTimer;
    private void _IntroUpdate()
    {
        introTimer += Time.deltaTime / gameModel.introDuration;
        if (introTimer >= 1)
            SetGameState(GameStates.Dialogue);
    }

    private void _DialogueUpdate()
    {
        bool done = narrative.UpdateFromGameController();

        if (done) SetGameState(GameStates.Spawn);
    }
    
    private float spawnTimer;
    private void _SpawnUpdate()
    {
        spawnTimer += Time.deltaTime / gameModel.spawnDuration;
        
        playerModel.positionTarget = Vector3.Lerp(
            playerModel.positionSpawnStart,
            playerModel.positionSpawnFinish,
            spawnTimer);

        if (spawnTimer >= 1)
            SetGameState(GameStates.Play);
    }
    private void _RespawnUpdate()
    {
        spawnInvincibilityTimer = gameModel.spawnInvincibility;
        spawnTimer += Time.deltaTime;
        playerModel.invincible = true;

        playerModel.positionTarget = Vector3.Lerp(
            playerModel.positionSpawnStart,
            playerModel.positionSpawnFinish,
            spawnTimer);

        // activate a bool for particles/grazing enable
        //
       
        playerModel.currentlyDead = false;
        

        if (spawnTimer >= gameModel.spawnDuration)
        {
            SetGameState(GameStates.Play);
            SpawnInvincibility = true;
            controllerFuel.currentFuel = controllerFuel.FuelMax;
        }
    }
    private void _PlayUpdate()
    {
        shipController.ShipUpdate();
        gunController.GunsUpdate();
        enemyController.EnemyUpdate();
        shieldAndHealthController.ShieldAndHealthUpdate();

        if (playerModel.hitpointsCurrent <= 0)
            SetGameState(GameStates.Die);
        if (SpawnInvincibility)
        {
            if (spawnInvincibilityTimer > 0)
            {
                shipController.playerModel.invincible = true;
                spawnInvincibilityTimer -= Time.deltaTime;
            }
            else
            {
                shipController.playerModel.invincible = false;
                SpawnInvincibility = false;
            }
        }
        
        
    }
    public float deathTimer;
    private void _DieUpdate()
    {
        //print test here
        // activate a bool for particles/grazing disable
        //
        playerModel.currentlyDead = true;
        shipController.playerModel.positionTarget =
            shipController.playerModel.ship.transform.position + Vector3.back * deathTimer * 10;
        //loop disable particles?z
        //
        deathTimer += Time.deltaTime;
        if (deathTimer >= 3)
        {
            SetGameState(GameStates.Respawn);
        }
    }
    private void _WinUpdate()
    {

    }
    private void _GameOverUpdate()
    {
        
    }
    #endregion
} 

public enum GameStates { Standby, Intro, Dialogue, Spawn, Respawn, Play, Die, Win, GameOver }