using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class T4Enemy : AbstractEnemy
    {
        public T4Enemy()
        {
            hp = 100;
            damage = 5;
            moveSpeed = 20f;
            fireRate = 1f;
            isLeft = true;
            enemyName = "T4Enemy";
        }

        public T4Enemy(bool l)
        {
            hp = 100;
            damage = 5;
            moveSpeed = 20f;
            fireRate = 1f;
            isLeft = l;
            enemyName = "T4Enemy";
        }

        public T4Enemy(int h, int d, float m, float f, bool l)
        {
            hp = h;
            damage = d;
            moveSpeed = m;
            fireRate = f;
            isLeft = l;
            enemyName = "T4Enemy";
        }
    }
}