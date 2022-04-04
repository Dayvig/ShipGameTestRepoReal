using System;
using System.Collections;
using System.Collections.Generic;
using Collectibles;
using Enemies;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
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
    private T3Enemy values3;
    private T4Enemy values4;
    private Boss1Enemy boss1Value; //These need to be named the same as the enemy script placed in Application.Model
    private TrailEnemy trailValues;
    private string level = "1";
    private float prevDifficulty;


    void Start()
    {
        Debug.Assert(gameModel != null, "Controller_Enemies is looking for a reference to Model_Game, but none has been added in the Inspector!");
        waves = new List<Wave>();
        // Enemy values part 2, getting the prefab from Model
        values = GameObject.Find("Model").GetComponent<MotorcycleEnemy>();
        values2 = GameObject.Find("Model").GetComponent<HogEnemy>();
        values3 = GameObject.Find("Model").GetComponent<T3Enemy>();
        values4 = GameObject.Find("Model").GetComponent<T4Enemy>();
        boss1Value = GameObject.Find("Model").GetComponent<Boss1Enemy>(); //Gets the prefab
        trailValues = GameObject.Find("Model").GetComponent<TrailEnemy>();
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

        if (waveTimer >= gameModel.waveSpawn && waveIndex < gameModel.level1Waves.Count)
        {
            int numberToSpawn = gameModel.level1Waves[waveIndex];
            
            float turnOverTime = 10;
            if (waveTimer >= turnOverTime && gameModel.waveSpawn < gameModel.level1Waves.Count)
            {
                //Enemy requirements part 3
                GameObject EOP;
                GameObject H0G;
                GameObject T3;
                GameObject T4;
                GameObject BOSS1;
                GameObject TRAIL;
                //
                Wave newWave = new Wave();

                for (int i = 0; i < numberToSpawn; i++)
                {
                    Vector3 startPoint;
                    Vector3 stag = new Vector3();


                    switch (gameModel.level1EnemyTypes[waveIndex])
                    {
                        case "Motorcycle":
                            //Instantiates the prefab itself into the game
                            EOP = Instantiate(gameModel.motorCycleEnemyPrefab);
                            //Gets the behavior script of the prefab
                            Motorcycle_behavior thisEnemyBehavior = EOP.GetComponent<Motorcycle_behavior>();
                            //Figures out whether it's entering from the left, right, top, or bottom as according to the enemy class
                            stag = getEntrance(values);
                            //counts the enemies in the wave
                            enemycount++;
                            //Makes them start a little to the left or right for variety
                            float displace = Random.Range(-values.startDisplace, values.startDisplace);
                            
                            //Makes them either spawn from the left or right
                            if (Random.Range(0, 2) == 0)
                            {
                                //sets the startpoint
                                startPoint = new Vector3(values.startPos + displace, 0, 20);
                                //makes each subsequent enemy begin strafing a tiny bit lower than the previous one
                                Vector3 extraDisplace = new Vector3(0, 0, -(i * values.startStagger) / 16);
                                
                                //sets up the waypoints
                                thisEnemyBehavior.nextWaypoint = (values.Waypoints[0] + extraDisplace);
                                thisEnemyBehavior.Waypoints.Add(thisEnemyBehavior.nextWaypoint);
                                thisEnemyBehavior.Waypoints.Add(values.Waypoints[1] + extraDisplace);
                                
                                //tells the game that this enemy spawns on the left
                                thisEnemyBehavior.isLeft = true;
                                //puts the enemy in the right spot in the scene, staggering it with the other enemies so they enter one by one in a line
                                EOP.transform.position = startPoint + (stag * i * values.startStagger);
                            }
                            else
                            {
                                //same code, but for spawning on the right side
                                
                                startPoint = new Vector3(-values.startPos + displace, 0, 20);
                                Vector3 extraDisplace = new Vector3(0, 0, -(i * values.startStagger) / 16);
                                thisEnemyBehavior.nextWaypoint = (values.Waypoints[1] + extraDisplace);
                                thisEnemyBehavior.Waypoints.Add(thisEnemyBehavior.nextWaypoint);
                                thisEnemyBehavior.Waypoints.Add(values.Waypoints[0] + extraDisplace);
                                thisEnemyBehavior.isLeft = true;
                                EOP.transform.position = startPoint + (stag * i * values.startStagger);
                            }

                            break;

                        case "Hog":
                            H0G = Instantiate(gameModel.HogEnemyPrefab);
                            hogEnemy_Behavior HogBehavior = H0G.GetComponent<hogEnemy_Behavior>();
                            stag = getEntrance(values2);
                            float xhogdisplace = Random.Range(-12, 12);
                            HogBehavior.nextWaypoint = values2.Waypoints[0] + Vector3.left * xhogdisplace;
                            HogBehavior.Waypoints[0] = HogBehavior.nextWaypoint;
                            HogBehavior.Waypoints[1] = values2.Waypoints[1] + Vector3.left * xhogdisplace;
                            enemycount++;
                            H0G.transform.position = HogBehavior.nextWaypoint + (stag * i * values2.startStagger);
                            break;

                        case "T3Enemy":
                            T3 = Instantiate(gameModel.T3EnemyPrefab);
                            T3Enemy_Behavior thisT3EnemyBehavior = T3.GetComponent<T3Enemy_Behavior>();
                            stag = getEntrance(values3);
                            float xdisplace = Random.Range(-12, 12);
                            thisT3EnemyBehavior.nextWaypoint = values3.Waypoints[0] + Vector3.left * xdisplace;
                            thisT3EnemyBehavior.Waypoints[0] = thisT3EnemyBehavior.nextWaypoint;
                            thisT3EnemyBehavior.Waypoints[1] = values3.Waypoints[1] + Vector3.left * xdisplace;
                            enemycount++;
                            T3.transform.position = thisT3EnemyBehavior.nextWaypoint + (stag * i * values.startStagger);
                            break;
                        
/*                        case "T4Enemy":
                            T4 = Instantiate(gameModel.T4EnemyPrefab);
                            m = T4.GetComponent<Motorcycle_behavior>();
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

                            break;*/

                        case "Boss1":
                            BOSS1 = Instantiate(gameModel.Boss1Prefab); //Spawn the prefab in
                            Boss1_Behavior
                                Boss1mind = BOSS1.GetComponent<Boss1_Behavior>(); //Get its behavior inside its prefab
                            enemycount++; //Add 1? to enemy counter
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
                            
                            TRAIL = Instantiate(gameModel.TrailEnemyPrefab); //Spawn the prefab in
                            TrailEnemy_Behavior tbehavior = TRAIL.GetComponent<TrailEnemy_Behavior>(); //Get its behavior inside its prefab
                            stag = getEntrance(trailValues);
                            enemycount++;
                            tbehavior.nextWaypoint = trailValues.Waypoints[0];
                            tbehavior.Waypoints.Add(tbehavior.nextWaypoint);
                            tbehavior.Waypoints.Add(trailValues.Waypoints[1]);
                            tbehavior.Waypoints.Add(trailValues.Waypoints[2]);
                            tbehavior.Waypoints.Add(trailValues.Waypoints[3]);
                            tbehavior.Waypoints.Add(trailValues.Waypoints[4]);
                            TRAIL.transform.position = tbehavior.nextWaypoint + (stag * i * trailValues.startStagger);
                            break;
                        default:
                            EOP = Instantiate(gameModel.motorCycleEnemyPrefab);
                            thisEnemyBehavior = EOP.GetComponent<Motorcycle_behavior>();
                            enemycount++;
                            if ((int) Random.Range(0, 1) == 0)
                            {

                                startPoint = new Vector3(-17f, 0, 20);
                                thisEnemyBehavior.nextWaypoint = new Vector3(17f, 0, -20f);
                                thisEnemyBehavior.Waypoints.Add(thisEnemyBehavior.nextWaypoint);
                                thisEnemyBehavior.isLeft = true;
                            }
                            else
                            {
                                startPoint = new Vector3(17f, 0, 20);
                                thisEnemyBehavior.nextWaypoint = new Vector3(17f, 0, -20f);
                                thisEnemyBehavior.Waypoints.Add(thisEnemyBehavior.nextWaypoint);
                                thisEnemyBehavior.isLeft = false;
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

    private Vector3 getEntrance(AbstractEnemy a)
    {
        Vector3 toReturn = new Vector3();
        switch (a.enterFrom)
        {
            case AbstractEnemy.EnterDirections.Bottom:
                toReturn = new Vector3(0, 0, -1);
                break;
            case AbstractEnemy.EnterDirections.Top:
                toReturn = new Vector3(0, 0, 1);
                break;
            case AbstractEnemy.EnterDirections.Left:
                toReturn = new Vector3(-1, 0, 0);
                break;
            case AbstractEnemy.EnterDirections.Right:
                toReturn = new Vector3(1, 0, 0);
                break;
            default:
                toReturn = new Vector3(0, 0, 1);
                break;
        }

        return toReturn;
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