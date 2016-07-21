using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class WallController : MonoBehaviour
    {
        public static WallController Instance { get; private set; }

        [AssignedInUnity]
        public float WallSpeed;

        [AssignedInUnity]
        public Transform SpawnPosition;

        [AssignedInUnity]
        public Transform DespawnPosition;

        private IList<GameObject> walls;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            walls = new List<GameObject>();
        }

        public void AddWall(Wall wall)
        {
            walls.Add(wall.gameObject);
        }

        [UnityMessage]
        public void FixedUpdate()
        {
            var remove = new List<GameObject>();

            foreach (var wall in walls)
            {
                var movement = new Vector3(0, 0, -WallSpeed) * Time.fixedDeltaTime;
                wall.GetComponent<Rigidbody>().MovePosition(wall.transform.position + movement);

                if (wall.transform.position.z < DespawnPosition.position.z)
                {
                    remove.Add(wall);
                }
            }

            foreach (var toRemove in remove)
                RemoveWall(toRemove);
        }

        private void RemoveWall(GameObject wall)
        {
            walls.Remove(wall);

            Destroy(wall);
        }
    }
}