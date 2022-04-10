using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller_Collectibles : MonoBehaviour
{
    public Model_Game gameModel;
    public List<string> level1Collectibles;
    public List<float> level1Timings;
    public float spawnTimer = 0f;
    public float spawnInterval;
    public int colIndex = 0;
    public Portal portalValues;
    public GuardianEnemy guardValues;
    GameObject GUARD;
    public int additionalGuardians = 0;
    
    
    void Start()
    {
        Debug.Assert(gameModel != null, "Controller_Collectibles is looking for a reference to Model_Game, but none has been added in the Inspector!");
        colIndex = 0;
        spawnTimer = level1Timings[colIndex];
        portalValues = GameObject.Find("Model").GetComponent<Portal>();
        guardValues = GameObject.Find("Model").GetComponent<GuardianEnemy>();
    }

    void Update()
    {
        CollectibleUpdate();
    }

    void SpawnGuardians(int toSpawn)
    {
        Vector3 stag;
        Vector3 dis;
        for (int i = 0; i < portalValues.guardiansToSpawn; i++)
        {
            GUARD = Instantiate(gameModel.GuardEnemyPrefab); //Spawn the prefab in
            Guardian_Behavior guardianBehavior = GUARD.GetComponent<Guardian_Behavior>(); //Get its behavior inside its prefab

            int rand = (int) Random.Range(0, 6);
            if (rand == 0)
            {
                stag = new Vector3(-1, 0, 0);
                dis = new Vector3(0, 0, Random.Range(-6, 6));
                guardianBehavior.nextWaypoint = guardValues.Waypoints[0] + dis;
                guardianBehavior.Waypoints.Add(guardianBehavior.nextWaypoint);
                guardianBehavior.Waypoints.Add(guardValues.Waypoints[1] + dis);
                GUARD.transform.position = guardianBehavior.nextWaypoint + (stag * i * guardValues.startStagger);
            }
            else if (rand == 1)
            {
                stag = new Vector3(1, 0, 0);
                dis = new Vector3(0, 0, Random.Range(-6, 6));
                guardianBehavior.nextWaypoint = guardValues.Waypoints[2] + dis;
                guardianBehavior.Waypoints.Add(guardianBehavior.nextWaypoint);
                guardianBehavior.Waypoints.Add(guardValues.Waypoints[3] + dis);
                GUARD.transform.position = guardianBehavior.nextWaypoint + (stag * i * guardValues.startStagger);
            }
            else if (rand == 2)
            {
                stag = new Vector3(0, 0, 1);
                dis = new Vector3(Random.Range(-6, 6), 0, 0);
                guardianBehavior.nextWaypoint = guardValues.Waypoints[4] + dis;
                guardianBehavior.Waypoints.Add(guardianBehavior.nextWaypoint);
                guardianBehavior.Waypoints.Add(guardValues.Waypoints[5] + dis);
                GUARD.transform.position = guardianBehavior.nextWaypoint + (stag * i * guardValues.startStagger);
            }
            else if (rand == 3)
            {
                stag = new Vector3(0, 0, -1);
                dis = new Vector3(Random.Range(-6, 6), 0, 0);
                guardianBehavior.nextWaypoint = guardValues.Waypoints[6] + dis;
                guardianBehavior.Waypoints.Add(guardianBehavior.nextWaypoint);
                guardianBehavior.Waypoints.Add(guardValues.Waypoints[7] + dis);
                GUARD.transform.position = guardianBehavior.nextWaypoint + (stag * i * guardValues.startStagger);
            }
            else if (rand == 4)
            { 
                stag = new Vector3(0, 0, -1);
                dis = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
                guardianBehavior.nextWaypoint = guardValues.Waypoints[8] + dis;
                guardianBehavior.Waypoints.Add(guardianBehavior.nextWaypoint);
                guardianBehavior.Waypoints.Add(guardValues.Waypoints[9] + dis);
                GUARD.transform.position = guardianBehavior.nextWaypoint + (stag * i * guardValues.startStagger);
            }
            else
            { 
                stag = new Vector3(0, 0, -1);
                dis = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
                guardianBehavior.nextWaypoint = guardValues.Waypoints[10] + dis;
                guardianBehavior.Waypoints.Add(guardianBehavior.nextWaypoint);
                guardianBehavior.Waypoints.Add(guardValues.Waypoints[11] + dis);
                GUARD.transform.position = guardianBehavior.nextWaypoint + (stag * i * guardValues.startStagger);
            }
        }
    }

    public void CollectibleUpdate()
    {
        if (gameModel.enemiesKilled >= gameModel.enemiesToSpawnPortal){
            Vector3 startPoint = new Vector3(0, 0, 8);
            GameObject COL = Instantiate(gameModel.PortalPrefab, startPoint, Quaternion.identity);
            PortalBehavior behavior = COL.GetComponent<PortalBehavior>();
            behavior.nextWaypoint = startPoint;
            behavior.Waypoints.Add(behavior.nextWaypoint);
            gameModel.enemiesKilled -= gameModel.enemiesToSpawnPortal;

            SpawnGuardians(portalValues.guardiansToSpawn + additionalGuardians);
            additionalGuardians += 2;
        }
    
        /*if (colIndex < level1Collectibles.Count)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0)
            {
                int numberToSpawn = 1;
                for (int i = 0; i < numberToSpawn; i++)
                {
                    GameObject COL;                   
                    Vector3 startPoint = new Vector3(0, 0, 8);
                    switch (level1Collectibles[colIndex])
                    {
                        case "Portal":
                            COL = Instantiate(gameModel.PortalPrefab, startPoint, Quaternion.identity);
                            PortalBehavior behavior = COL.GetComponent<PortalBehavior>();
                            behavior.nextWaypoint = startPoint;
                            behavior.Waypoints.Add(behavior.nextWaypoint);
                            break;
                    }

                    colIndex++;
                }

                spawnTimer = level1Timings[colIndex];
            }*/
        }
    }
        /*
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
                    Vector3 stagger = new Vector3(0, 0, 2);
                    EOP = new GameObject();
                    EOP.transform.position = startPoint + (stagger * i);
                    newWave.enemies.Add(EOP);
                    H0G = new GameObject();
                    H0G.transform.position = startPoint + (stagger * i);
                    newWave.enemies.Add(H0G);
                }

                waves.Add(newWave);

                waveTimer = 0;
                waveIndex++;
            }

            waveTimer = 0;
            waveIndex++;
        }*/
