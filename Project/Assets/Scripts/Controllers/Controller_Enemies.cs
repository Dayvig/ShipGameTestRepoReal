using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller_Enemies : MonoBehaviour
{
    public Model_Game gameModel;
    public List<Wave> waves;
    private float waveTimer = 1000;
    public int waveIndex;
    private MotorcycleEnemy values;
    public float waveInterval = 10f;
    private HogEnemy values2;


    void Start()
    {
        Debug.Assert(gameModel != null, "Controller_Enemies is looking for a reference to Model_Game, but none has been added in the Inspector!");
        waves = new List<Wave>();
        values = GameObject.Find("Model").GetComponent<MotorcycleEnemy>();
        values2 = GameObject.Find("Model").GetComponent<HogEnemy>();
    }

    void Update()
    {
        // Enemies Movement
        for (int i = waves.Count -1; i >= 0; i--)
        {
            Wave wave = waves[i];
            bool anyLeft = false;
            foreach (var enemy in wave.enemies)
            {
                //checking wave to see if any are left
                //this allows us to simply set an enemy's gameObject to not be active to effectively kill it
                //to allow for effects, we handle this with the enemey's behavior
                //  this does two things: 
                //      - this allows us to manage resources in the controller more cleanly
                //      - this allows us stop the enemey's behavior for continuing to run after it is dead
                if (enemy.transform.gameObject.activeSelf && enemy.transform.position.z > -16)
                    anyLeft = true;
            }

            if (!anyLeft)
            {
                CleanUpWave(wave);
            }
        }
    }

    public void EnemyUpdate()
    {
        // Making waves for the level according to model specifications
        
        waveTimer += Time.deltaTime;

        if (waveTimer >= waveInterval && waveIndex < gameModel.level1Waves.Count)
        {
            int numberToSpawn = gameModel.level1Waves[waveIndex];
        
        float turnOverTime = 10;
            if (waveTimer >= turnOverTime && waveIndex < gameModel.level1Waves.Count)
            {
                GameObject EOP;
                GameObject H0G;
                Wave newWave = new Wave();

                for (int i = 0; i < numberToSpawn; i++)
                {
                    Vector3 startPoint;
                    switch (gameModel.level1EnemyTypes[waveIndex])
                    {
                        case "Motorcycle":
                            EOP = Instantiate(gameModel.motorCycleEnemyPrefab);
                            Motorcycle_behavior m = EOP.GetComponent<Motorcycle_behavior>();
                            float displace = Random.Range(-values.startDisplace, values.startDisplace);
                            if (Random.Range(0, 2) == 0)
                            {
                                startPoint = new Vector3(-values.startPos + displace, 0, 20);
                                m.nextWaypoint = new Vector3(-values.startPos + displace, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = true;
                            }
                            else
                            {
                                startPoint = new Vector3(values.startPos + displace, 0, 20);
                                m.nextWaypoint = new Vector3(values.startPos + displace, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = false;
                            }
                            break;

                        case "Hog":
                            H0G = Instantiate(gameModel.HogEnemyPrefab);
                            m = H0G.GetComponent<Motorcycle_behavior>();
                            displace = Random.Range(-values2.startDisplace, values2.startDisplace);
                            if (Random.Range(0, 2) == 0)
                            {
                                startPoint = new Vector3(-values2.startPos + displace, 0, 20);
                                m.nextWaypoint = new Vector3(-values2.startPos + displace, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = true;
                            }
                            else
                            {
                                startPoint = new Vector3(values2.startPos + displace, 0, 20);
                                m.nextWaypoint = new Vector3(values2.startPos + displace, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = false;
                            }
                            break;

                        default:
                            EOP = Instantiate(gameModel.motorCycleEnemyPrefab);
                            m = EOP.GetComponent<Motorcycle_behavior>();
                            if ((int)Random.Range(0, 1) == 0)
                            {

                                startPoint = new Vector3(-17f, 0, 20);
                                m.nextWaypoint = new Vector3(17f, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = true;
                            }
                            else
                            {
                                startPoint = new Vector3(17f, 0, 20);
                                m.nextWaypoint = new Vector3(17f, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = false;
                            }

                            H0G = Instantiate(gameModel.HogEnemyPrefab);
                            m = H0G.GetComponent<Motorcycle_behavior>();
                            if ((int)Random.Range(0, 1) == 0)
                            {

                                startPoint = new Vector3(-17f, 0, 20);
                                m.nextWaypoint = new Vector3(17f, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = true;
                            }
                            else
                            {
                                startPoint = new Vector3(17f, 0, 20);
                                m.nextWaypoint = new Vector3(17f, 0, -20f);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.isLeft = false;
                            }
                            break;


                    }
                }

                waves.Add(newWave);

                waveTimer = 0;
                waveIndex++;
            }

            waveTimer = 0;
            waveIndex++;
        }

    }
    
    

    private void CleanUpWave(Wave wave)
    {
        for (int j = wave.enemies.Count - 1; j >= 0; j--)
        {
            var EOP = wave.enemies[j];
            wave.enemies.Remove(EOP);
            Destroy(EOP.transform.gameObject);

            var H0G = wave.enemies[j];
            wave.enemies.Remove(H0G);
            Destroy(H0G.transform.gameObject);
        }
        waves.Remove(wave);
    }

    [System.Serializable]
    public class Wave
    {
        public List<GameObject> enemies;
        public List<Vector3> waypoints;
        public List<AbstractEnemy> enemyType;

        public Wave()
        {
            enemies = new List<GameObject>();
            waypoints = new List<Vector3>();
            enemyType = new List<AbstractEnemy>();
        }
    }

    [System.Serializable]
    public class EnemyOnPath
    {
        public Transform transform;
        public int waypointIndex;
        public AbstractEnemy enemyT;
    }
}
