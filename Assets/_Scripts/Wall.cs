using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class Wall : MonoBehaviour
    {
        public IWallDescription Description { get; set; }

        [AssignedInUnity]
        public string DescriptionName;
    }
}