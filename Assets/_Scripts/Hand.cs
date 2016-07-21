using System;
using System.Collections;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class Hand : MonoBehaviour
    {
        [AssignedInUnity]
        public SteamVR_TrackedObject Controller;

        [AssignedInUnity]
        public bool IsLeft;

        private new Rigidbody rigidbody;

        [UnityMessage]
        public void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            if (Controller == null)
                throw new InvalidOperationException("No controller.");
        }

        [UnityMessage]
        public void FixedUpdate()
        {
            rigidbody.MovePosition(Controller.transform.position);
        }

        [UnityMessage]
        public void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody.gameObject.CompareTag("Wall"))
            {
                var wall = other.attachedRigidbody.gameObject.GetComponent<Wall>();
                CollisionController.Instance.HandCollision(this, wall);
            }
        }

        public void TriggerCollisionVibration()
        {
            LongVibration(0.3f, 0.2f);
        }

        private void LongVibration(float length, float strength)
        {
            if (Controller.index != SteamVR_TrackedObject.EIndex.None)
            {
                StartCoroutine(LongVibrationCoroutine(length, strength));
            }
        }

        private IEnumerator LongVibrationCoroutine(float length, float strength)
        {
            for (float i = 0; i < length; i += Time.deltaTime)
            {
                var index = SteamVR_Controller.GetDeviceIndex(IsLeft ? SteamVR_Controller.DeviceRelation.Leftmost : SteamVR_Controller.DeviceRelation.Rightmost);
                SteamVR_Controller.Input(index).TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
                yield return null;
            }
        }
    }
}