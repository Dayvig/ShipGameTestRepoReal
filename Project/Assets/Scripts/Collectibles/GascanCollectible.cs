namespace Collectibles
{
    public class GascanCollectible : AbstractCollectible
    {
        public GascanCollectible(string name, float range, float speed, int hp) : base(name, range, speed, hp)
        {
            collectibleName = name;
            collectionRange = range;
            movementSpeed = speed;
            hitPoints = hp;
        }

        public GascanCollectible(string name) : base(name)
        {
            collectibleName = name;
            collectionRange = 1f;
            movementSpeed = 0.1f;
            hitPoints = 1;
        }
    }
}