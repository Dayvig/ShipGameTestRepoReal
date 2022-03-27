using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Controller_EnemyBullets : MonoBehaviour
{
    public Model_Game gameModel;
    private List<GameObject> _bulletsInactive;
    private List<BulletTracker> _bulletsActive;
    private MotorcycleEnemy motorcycleEnemyValues;

    void Start()
    {
        Debug.Assert(gameModel != null, "Controller_EnemyBullets is looking for a reference to Model_Game, but none has been added in the Inspector!");
        motorcycleEnemyValues = GameObject.Find("Model").GetComponent<MotorcycleEnemy>();
        _bulletsInactive = new List<GameObject>();
        _bulletsActive = new List<BulletTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = _bulletsActive.Count - 1; i >= 0; i--)
        {
            var tracker = _bulletsActive[i];
            UpdateBullet(tracker, tracker.name);
            
            bool offScreen = false;
            if (tracker.bullet.transform.position.x > 20 || tracker.bullet.transform.position.x < -20) offScreen = true;
            if (tracker.bullet.transform.position.z > 20 || tracker.bullet.transform.position.z < -20) offScreen = true;

            if (offScreen)
            {
                _KillBullet(tracker);
            }
        }
    }
    
    //updates bullets based on type- is called every frame
    void UpdateBullet(BulletTracker thisBullet, string bulletName)
    {
        switch (bulletName)
        {
            case "MotorcycleBullet":
                thisBullet.bullet.transform.position += thisBullet.direction * Time.deltaTime * motorcycleEnemyValues.bulletSpeed;
                break; 
            default:
                thisBullet.bullet.transform.position += thisBullet.direction * Time.deltaTime * gameModel.enemyBulletSpeed1;
                break;
        }
    }

    public void FireBullet(Vector3 where, Vector3 direction, string type)
    {
        GameObject bullet;
        if (_bulletsInactive.Count > 0)
        {
            bullet = _bulletsInactive[0];
            _bulletsInactive.Remove(bullet);
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(gameModel.enemyBulletPrefab1);
        }

        bullet.transform.position = where;
        var tracker = new BulletTracker();
        tracker.bullet = bullet;
        tracker.direction = direction;
        tracker.name = type;
        _bulletsActive.Add(tracker);
    }
    
    public void FireBullet(Vector3 where, Vector3 direction)
    {
        GameObject bullet;
        if (_bulletsInactive.Count > 0)
        {
            bullet = _bulletsInactive[0];
            _bulletsInactive.Remove(bullet);
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(gameModel.enemyBulletPrefab1);
        }

        bullet.transform.position = where;
        var tracker = new BulletTracker();
        tracker.bullet = bullet;
        tracker.direction = direction;
        tracker.name = "defaultBullet";
        _bulletsActive.Add(tracker);
    }

    public void KillBullet(GameObject bullet)
    {
        BulletTracker tracker = null;
        foreach (var t in _bulletsActive)
        {
            if (t.bullet == bullet)
            {
                tracker = t;
                break;
            }
        }

        if (tracker != null)
            _KillBullet(tracker);
    }

    public void _KillBullet(BulletTracker tracker)
    {
        _bulletsActive.Remove(tracker);
        tracker.bullet.SetActive(false);
        _bulletsInactive.Add(tracker.bullet);
    }

    public class BulletTracker
    {
        public GameObject bullet;
        public Vector3 direction;
        public string name;
        //add more properties later maybe
    }
}
