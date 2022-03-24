using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    public abstract class Base_Collectible_Behavior : MonoBehaviour {
        
        public Model_Player playerModel;
        public int hitPoints;
        public float movementSpeed;
        public List<Vector3> Waypoints = new List<Vector3>();
        public Vector3 nextWaypoint;
        public int currentWaypointIndex;
        public int behaviorState = 0;
        public bool collectOnMoveInto = true;
        public ParticleSystem ps;
        private void Start()
        {
            playerModel = GameObject.Find("Model").GetComponent<Model_Player>();

            if (Waypoints.Count == 0)
            {
                Debug.Log("Empty Waypoints on collectible");
                Waypoints.Add(new Vector3(0, 0, 0));
            }
            SetupCollectible();
        }
        
        void Update()
        {
            UpdateVisuals();
            MovementUpdate();
            CollectionUpdate();
            
        }

        public abstract void MovementUpdate();
        public abstract void SetupCollectible();

        public void CollectionUpdate()
        {
            if (CollectionCondition())
            {
                Collect();
                Destroy(this.gameObject);
            }
        }

        public abstract bool CollectionCondition();
        public abstract void UpdateVisuals();
        public abstract void Collect();
    }
    
}