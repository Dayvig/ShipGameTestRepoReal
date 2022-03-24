using System;
using UnityEngine;

namespace Enemies
{
    public class MotorcycleEnemy : AbstractEnemy
    {
        //0 - Drives downwards on the sides of the screen, shooting a 2-bullet spread left or right.

        public MotorcycleEnemy()
        {
            hp = 10;
            damage = 1;
            moveSpeed = 20f;
            fireRate = 1f;
            isLeft = true;
            enemyName = "Motorcycle";
        }
        
        public MotorcycleEnemy(bool l)
        {
            hp = 10;
            damage = 1;
            moveSpeed = 20f;
            fireRate = 1f;
            isLeft = l;
            enemyName = "Motorcycle";
        }

        public MotorcycleEnemy(int h, int d, float m, float f, bool l)
        {
            hp = h;
            damage = d;
            moveSpeed = m;
            fireRate = f;
            isLeft = l;
            enemyName = "Motorcycle";
        }
    }
}