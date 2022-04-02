using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        [TextArea]
        public string Notes = "";
        public int hp;
        public int damage;
        public float moveSpeed;
        public float fireRate;
        public string enemyName;
        public bool isLeft;
        public float bulletSpeed;
        public int[] bulletAngles;
        public float startPos;
        public List<Vector3> Waypoints = new List<Vector3>();
        public float startDisplace;
        public float startStagger;
        public EnterDirections enterFrom;
        public enum EnterDirections { Left, Right, Top, Bottom }
    }
}