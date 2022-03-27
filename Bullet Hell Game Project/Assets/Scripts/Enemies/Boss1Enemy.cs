using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Boss1Enemy : AbstractEnemy
{
    // Start is called before the first frame update
    //0 - Drives downwards on the sides of the screen, shooting a 2-bullet spread left or right.

    public Boss1Enemy()
    {
        hp = 30;
        damage = 1;
        moveSpeed = 20f;
        fireRate = 1f;
        isLeft = true;
        enemyName = "Boss1";
    }

    public Boss1Enemy(bool l)
    {
        hp = 30;
        damage = 1;
        moveSpeed = 20f;
        fireRate = 1f;
        isLeft = l;
        enemyName = "Boss1";
    }

    public Boss1Enemy(int h, int d, float m, float f, bool l)
    {
        hp = h;
        damage = d;
        moveSpeed = m;
        fireRate = f;
        isLeft = l;
        enemyName = "Boss1";
    }


}
