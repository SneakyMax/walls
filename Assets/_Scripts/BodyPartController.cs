using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class BodyPartController : MonoBehaviour
    {
        public static BodyPartController Instance { get; private set; }

        [AssignedInUnity]
        public Head Head;

        [AssignedInUnity]
        public Hand LeftHand;

        [AssignedInUnity]
        public Hand RightHand;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
        }
    }
}