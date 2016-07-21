using System.Collections.Generic;
using Assets._Scripts.Helpers;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class MultiWallDescription : MonoBehaviour, IWallDescription
    {
        [AssignedInUnity]
        public string Name;

        public IEnumerable<MeshInfo> GenerateSolidMeshes(float thickness)
        {
            var meshes = new List<MeshInfo>();

            foreach (var description in GetComponents<WallDescription>())
            {
                meshes.AddRange(description.GenerateSolidMeshes(thickness));
            }

            return meshes;
        }
    }
}