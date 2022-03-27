using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        public int hp;
        public int damage;
        public float moveSpeed;
        public float fireRate;
        public string enemyName;
        public bool isLeft;
        public float bulletSpeed;
        public int[] bulletAngles;
        public float startPos;
        public float startDisplace;
        public float startStagger;
        
    }
}