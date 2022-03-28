using System.Collections;
using System.Collections.Generic;
using Collectibles;
using UnityEngine;

public class Portal : AbstractCollectible
{
    public Portal(string name, float range, float speed, int hp) : base(name, range, speed, hp)
    {
        
    }

    public Portal(string name) : base(name)
    {
    }
}
