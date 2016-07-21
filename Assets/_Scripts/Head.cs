using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class Head : MonoBehaviour
    {
        [AssignedInUnity]
        public Transform HMD;

        private new Rigidbody rigidbody;

        [UnityMessage]
        public void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        [UnityMessage]
        public void FixedUpdate()
        {
            rigidbody.MovePosition(HMD.position);
        }

        [UnityMessage]
        public void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody.gameObject.CompareTag("Wall"))
            {
                var wall = other.attachedRigidbody.gameObject.GetComponent<Wall>();
                CollisionController.Instance.HeadCollision(this, wall);
            }
        }
    }
}