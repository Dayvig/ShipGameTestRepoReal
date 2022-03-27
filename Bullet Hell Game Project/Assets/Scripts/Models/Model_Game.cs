using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Model_Game : MonoBehaviour
{
    [Header("Game State Transition Timings")]
    public float introDuration;
    public float dialogueDuration;
    public float spawnDuration;
    public float spawnInvincibility;
    public float waveSpawn;
    public List<float> waveCooldown;

    [Header("Enemies")]
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyBulletPrefab1;
    public GameObject motorCycleEnemyPrefab;
    public GameObject HogEnemyPrefab;
    public GameObject GasCanisterPrefab;
    public GameObject PortalPrefab;
    public List<int> level1Waves;
    public List<int> Level2Waves;
    public float enemySpeed1;
    public float enemyBulletSpeed1;
    [Header("Effects")]
    public GameObject explosionPrefab1;
    public List<string> level1EnemyTypes;
    public List<string> level2EnemyTypes;
}
