using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class T3Enemy : AbstractEnemy
    {
        public T3Enemy()
        {
            hp = 50;
            damage = 3;
            moveSpeed = 20f;
            fireRate = 1f;
            isLeft = true;
            enemyName = "T3Enemy";
        }

        public T3Enemy(bool l)
        {
            hp = 50;
            damage = 3;
            moveSpeed = 20f;
            fireRate = 1f;
            isLeft = l;
            enemyName = "T3Enemy";
        }

        public T3Enemy(int h, int d, float m, float f, bool l)
        {
            hp = h;
            damage = d;
            moveSpeed = m;
            fireRate = f;
            isLeft = l;
            enemyName = "T3Enemy";
        }
    }
}