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
    private float waveTimer = 8f;
    public int waveIndex;
    public int enemycount;
    // Enemy Requirements part 1
    private MotorcycleEnemy values;
    private HogEnemy values2;
    private Boss1Enemy boss1Value; //These need to be named the same as the enemy script placed in Application.Model
    //
    private string level = "1";
    private float prevDifficulty;


    void Start()
    {
        Debug.Assert(gameModel != null, "Controller_Enemies is looking for a reference to Model_Game, but none has been added in the Inspector!");
        waves = new List<Wave>();
        // Enemy values part 2, getting the prefab from Model
        values = GameObject.Find("Model").GetComponent<MotorcycleEnemy>();
        values2 = GameObject.Find("Model").GetComponent<HogEnemy>();
        boss1Value = GameObject.Find("Model").GetComponent<Boss1Enemy>(); //Gets the prefab
        //
        prevDifficulty = gameModel.difficultyMultiplier;
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
        DifficultyUpdate();
        waveTimer += Time.deltaTime;
        Debug.Log(waveTimer);

        if (waveTimer >= gameModel.waveSpawn && waveIndex < gameModel.level1Waves.Count)
        {
            int numberToSpawn = gameModel.level1Waves[waveIndex];
            
            float turnOverTime = 10;
            if (waveTimer >= turnOverTime && gameModel.waveSpawn < gameModel.level1Waves.Count)
            {
                //Enemy requirements part 3
                GameObject EOP;
                GameObject H0G;
                GameObject BOSS1;
                GameObject TRAIL;
                //
                Wave newWave = new Wave();

                for (int i = 0; i < numberToSpawn; i++)
                {
                    Vector3 startPoint;
                    switch (gameModel.level1EnemyTypes[waveIndex])
                    {
                        case "Motorcycle":
                            EOP = Instantiate(gameModel.motorCycleEnemyPrefab);
                            Motorcycle_behavior m = EOP.GetComponent<Motorcycle_behavior>();
                            enemycount++;
                            float displace = Random.Range(-values.startDisplace, values.startDisplace);
                            if (Random.Range(0, 2) == 0)
                            {
                                Vector3 stag = new Vector3(0, 0, 1);
                                startPoint = new Vector3(-values.startPos + displace, 0, 20);
                                m.nextWaypoint = new Vector3(-values.startPos + displace, 0, 8f - (i * values.startStagger)/16);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.Waypoints.Add(new Vector3(values.startPos + displace, 0, 8f - (i * values.startStagger)/16));
                                m.isLeft = true;
                                EOP.transform.position = startPoint + (stag * i * values.startStagger);
                            }
                            else
                            {
                                startPoint = new Vector3(values.startPos + displace, 0, 20);
                                m.nextWaypoint = new Vector3(values.startPos + displace, 0, 8f - (i * values.startStagger)/16);
                                m.Waypoints.Add(m.nextWaypoint);
                                m.Waypoints.Add(new Vector3(-values.startPos + displace, 0, 8f - (i * values.startStagger)/16));
                                m.isLeft = false;
                                Vector3 stag = new Vector3(0, 0, 1);
                                EOP.transform.position = startPoint + (stag * i * values.startStagger);
                            }
                            break;

                        case "Hog":
                            H0G = Instantiate(gameModel.HogEnemyPrefab);
                            m = H0G.GetComponent<Motorcycle_behavior>();
                            enemycount++;
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

                        case "Boss1":
                            BOSS1 = Instantiate(gameModel.Boss1Prefab);     //Spawn the prefab in
                            Boss1_Behavior Boss1mind = BOSS1.GetComponent<Boss1_Behavior>();       //Get its behavior inside its prefab
                            enemycount++;                                   //Add 1? to enemy counter
                            displace = Random.Range(-boss1Value.startDisplace, boss1Value.startDisplace);
                            if (Random.Range(0, 2) == 0)
                            {
                                startPoint = new Vector3(-boss1Value.startPos + displace, 0, 20);
                                Boss1mind.nextWaypoint = new Vector3(-boss1Value.startPos + displace, 0, -20f);
                                Boss1mind.Waypoints.Add(Boss1mind.nextWaypoint);
                                Boss1mind.isLeft = true;
                            }
                            else
                            {
                                startPoint = new Vector3(boss1Value.startPos + displace, 0, 20);
                                Boss1mind.nextWaypoint = new Vector3(boss1Value.startPos + displace, 0, -20f);
                                Boss1mind.Waypoints.Add(Boss1mind.nextWaypoint);
                                Boss1mind.isLeft = false;
                            }
                            break;
                        
                        case "Trail":
                            Vector3 stag2 = new Vector3(-1, 0, 0);
                            TRAIL = Instantiate(gameModel.TrailEnemyPrefab);     //Spawn the prefab in
                            Debug.Log("Test" + i);
                            TrailEnemy_Behavior tbehavior = TRAIL.GetComponent<TrailEnemy_Behavior>();       //Get its behavior inside its prefab
                            enemycount++;                                   //Add 1? to enemy counter
                            startPoint = new Vector3(-20, 0, 0);
                            tbehavior.nextWaypoint = startPoint;
                            tbehavior.Waypoints.Add(tbehavior.nextWaypoint);
                            tbehavior.Waypoints.Add(new Vector3(2, 0, 10));
                            TRAIL.transform.position = startPoint + (stag2 * i * values.startStagger);
                            break;
                        default:
                            EOP = Instantiate(gameModel.motorCycleEnemyPrefab);
                            m = EOP.GetComponent<Motorcycle_behavior>();
                            enemycount++;
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
                    waveTimer = gameModel.waveCooldown[waveIndex];
                    waveIndex++;
            }


        }

    }

    private void DifficultyUpdate()
    {
        if (gameModel.difficultyMultiplier != prevDifficulty)
        {
            gameModel.speedMultiplier += (gameModel.difficultyMultiplier - prevDifficulty);
            gameModel.bulletSpeedMultiplier += (gameModel.difficultyMultiplier - prevDifficulty);
            gameModel.healthMultiplier += (gameModel.difficultyMultiplier - prevDifficulty);
            gameModel.fireRateMultiplier += (gameModel.difficultyMultiplier - prevDifficulty);
            prevDifficulty = gameModel.difficultyMultiplier;
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
            enemycount--;
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