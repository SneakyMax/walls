using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Helpers;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class WallGenerator : MonoBehaviour
    {
        [AssignedInUnity]
        public GameObject EmptyWallPrefab;

        [AssignedInUnity]
        public Material WallMaterial;

        [AssignedInUnity]
        public GameObject WallDescriptionsContainer;

        [AssignedInUnity]
        public Transform WallStartPosition;

        [AssignedInUnity]
        public float Rate = 2;

        [AssignedInUnity, Range(0.1f, 1f)]
        public float MinThickness;

        [AssignedInUnity, Range(0.1f, 3)]
        public float MaxThickness;

        private IList<IWallDescription> descriptions;

        private IWallDescription lastWallDescription;

        private Coroutine generatorCoroutine;

        [UnityMessage]
        public void Awake()
        {
            descriptions = new List<IWallDescription>();

            LoadDescriptions();
        }

        private void LoadDescriptions()
        {
            var singleDescriptions = WallDescriptionsContainer.GetComponentsInChildren<WallDescription>()
                .Where(x => x.GetComponent<MultiWallDescription>() == null);

            var multiDescriptions = WallDescriptionsContainer.GetComponentsInChildren<MultiWallDescription>();
            
            foreach (var singleDescription in singleDescriptions)
            {
                descriptions.Add(singleDescription);
            }

            foreach (var multiDescription in multiDescriptions)
            {
                descriptions.Add(multiDescription);
            }

        }

        [UnityMessage]
        public void Start()
        {
            StartGenerating();
        }

        public void StartGenerating()
        {
            generatorCoroutine = StartCoroutine(MakeWalls());
        }

        public void StopGenerating()
        {
            StopCoroutine(generatorCoroutine);
            generatorCoroutine = null;
        }

        private IEnumerator MakeWalls()
        {
            while (true)
            {
                yield return new WaitForSeconds(Rate);

                IWallDescription description = null;
                while (description == null || description == lastWallDescription)
                    description = ChooseDescription();

                lastWallDescription = description;

                var thickness = Random.Range(MinThickness, MaxThickness);

                var meshes = description.GenerateSolidMeshes(thickness);
                var wall = InstantiateWall(meshes, description);

                wall.DescriptionName = description.DescriptionName;
                wall.tag = "Wall";

                WallController.Instance.AddWall(wall);
            }
            // ReSharper disable once FunctionNeverReturns
        } 

        private Wall InstantiateWall(IEnumerable<MeshInfo> meshes, IWallDescription description)
        {
            var instance = (GameObject)Instantiate(EmptyWallPrefab, WallStartPosition.position, Quaternion.identity);

            var i = 0;
            foreach (var mesh in meshes)
            {
                var child = new GameObject();
                child.transform.SetParent(instance.transform, false);

                var filter = child.AddComponent<MeshFilter>();
                filter.mesh = mesh.Mesh;

                var childRenderer = child.AddComponent<MeshRenderer>();
                childRenderer.material = WallMaterial;

                var boxCollider = child.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
                boxCollider.center = mesh.Bounds.center;
                boxCollider.size = mesh.Bounds.size;

                child.name = "Mesh " + i;

                i++;
            }

            var wall = instance.GetComponent<Wall>();
            wall.Description = description;

            return wall;
        }

        private IWallDescription ChooseDescription()
        {
            var index = Random.Range(0, descriptions.Count);

            return descriptions[index];
        }
    }
}