using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Player : MonoBehaviour
{
    public GameObject shipPrefab;
    [HideInInspector]public GameObject ship;
    public GameObject shieldPrefab;
    [HideInInspector] public GameObject shield;
    public GameObject bulletPrefab;

    [Header("Starting Values")]
    public Vector3 positionSpawnStart;
    public Vector3 positionSpawnFinish;
    public int shieldpointsBase;
    public int hitpointsBase;
    public float damageBase;
    public int livesBase;
    public float shieldedRadius;
    public float unshieldedRadius;
    public bool invincible;
    public bool lostLife;
    public int score;
    public int bulletGrazes;
    public int grazesForPowerup;
    public float grazePowerupInterval;
    public float shiftTurningFactor;

    [Header("Tuning Values")]
    [Range(0.01f, 30.0f)] public float smoothingFactor;

    public float shipSpeed;
    public float bulletSpeed;
    public float fireRate;
    public float limitHorz;
    public float limitVert;
    public float shieldRegenIntervalBase;
    public float shieldDurationBase;
    public float turnLimit;
    public float currentTurnLimit;
    public float rotationSpeed;
    public float vFactor;
    public float shieldCooldownBase;

    [Header("Controlled Variables")]
    public Vector3 positionTarget;
    public Vector3 positionCurrent;
    public float rotationTarget;
    public float rotationCurrent;
    public Vector3 actualRotation = new Vector3(0, 0, 0);
    public float shieldPointsMax;
    public float shielddPointsCurrent;
    public int hitpointsCurrent;
    public float damageCurrent;
    public int livesCurrent;
    public bool shieldActive;
    public float shieldRegenIntervalCurrent;
    public float shieldCooldownCurrent;
    public float shieldDurationCurrent;
}
