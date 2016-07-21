using UnityEngine;
using Valve.VR;

namespace Assets._Scripts
{
    [UnityComponent]
    public class CollisionController : MonoBehaviour
    {
        public static CollisionController Instance { get; private set; }

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
        }

        public void HeadCollision(Head head, Wall wall)
        {
            BodyPartController.Instance.LeftHand.TriggerCollisionVibration();
            BodyPartController.Instance.RightHand.TriggerCollisionVibration();

            DestroyWall(wall);
        }

        public void HandCollision(Hand hand, Wall wall)
        {
            hand.TriggerCollisionVibration();

            DestroyWall(wall);
        }

        private void DestroyWall(Wall wall)
        {
            WallController.Instance.RemoveWall(wall.gameObject);
        }
    }
}