using UnityEngine;

namespace Models
{
    public class BulletProperties : MonoBehaviour
    {
        public Vector3 Direction;
        
        public BulletProperties()
        {
            Direction = Vector3.forward;
        }

        public BulletProperties(Vector3 dir)
        {
            Direction = dir;
        }

        public void SetDirection(Vector3 d)
        {
            Direction = d;
        }

        public Vector3 GetDirection()
        {
            return Direction;
        }

    }
}