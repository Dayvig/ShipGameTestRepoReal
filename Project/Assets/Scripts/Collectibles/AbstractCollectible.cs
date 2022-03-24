using Enemies;
using UnityEngine;

namespace Collectibles
{
    public class AbstractCollectible : MonoBehaviour
    {

        public string collectibleName;
        public float collectionRange;
        public float movementSpeed;
        public int hitPoints;

        public AbstractCollectible(string name, float range, float speed, int hp)
        {
            collectibleName = name;
            collectionRange = range;
            movementSpeed = speed;
            hitPoints = hp;
        }

        public AbstractCollectible(string name)
        {
            collectibleName = name;
            collectionRange = 10f;
            movementSpeed = 2f;
            hitPoints = 1;
        }

    }
}