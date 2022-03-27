using System.Collections;
using UnityEngine;
namespace Enemies
{
    public class HogEnemy : AbstractEnemy
    {
        public HogEnemy()
        {
            hp = 25;
            damage = 2;
            moveSpeed = 10f;
            fireRate = 1f;
            isLeft = true;
            enemyName = "Hog";
        }

        public HogEnemy(bool l)
        {
            hp = 25;
            damage = 2;
            moveSpeed = 10f;
            fireRate = 1f;
            isLeft = l;
            enemyName = "Hog";
        }

        public HogEnemy(int h, int d, float m, float f, bool l)
        {
            hp = h;
            damage = d;
            moveSpeed = m;
            fireRate = f;
            isLeft = l;
            enemyName = "Hog";
        }
    }
}
