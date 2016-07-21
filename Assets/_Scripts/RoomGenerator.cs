using Assets._Scripts.Helpers;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class RoomGenerator : MonoBehaviour
    {
        [AssignedInUnity]
        public float Width;

        [AssignedInUnity]
        public float Length;

        [AssignedInUnity]
        public float Height;

        [AssignedInUnity]
        public float PlatformHeight;

        [AssignedInUnity, Header("Existing Objects")]
        public Transform WallStartPosition;

        [AssignedInUnity]
        public Transform WallStopPosition;

        [AssignedInUnity, Header("Materials")]
        public Material WallMaterial;

        [AssignedInUnity]
        public Material EndMaterial;

        [AssignedInUnity]
        public Material FloorMaterial;

        [AssignedInUnity]
        public Material CeilingMaterial;

        [AssignedInUnity]
        public Material PlatformMaterial;

        private Transform root;
        private readonly Vector3 center = Vector3.zero;

        private float UnderFloorYPosition { get { return center.y - PlatformHeight; } }

        private readonly float wallThickness = 0.1f;

        [UnityMessage]
        public void Start()
        {
            var rootObj = new GameObject();
            root = rootObj.transform;

            GenerateFloor();
            GenerateWalls();
            GenerateCeiling();
            GenerateEnds();
            GeneratePlatform();
        }

        private void GenerateFloor()
        {
            var x = new Vector2(-Width / 2.0f, Width / 2.0f);
            var y = new Vector2(UnderFloorYPosition, UnderFloorYPosition - wallThickness);
            var z = new Vector2(-Length / 2.0f, Length / 2.0f);
            
            var mesh = MeshGenerator.Cube(x, y, z);

            CreateChildObject(mesh, FloorMaterial, "Floor");
        }

        private void GenerateWalls()
        {
            var y = new Vector2(UnderFloorYPosition, UnderFloorYPosition + Height);
            var z = new Vector2(-Length / 2.0f, Length / 2.0f);

            // Left
            var x = new Vector2(-Width / 2.0f, (-Width / 2.0f) - wallThickness);

            var mesh = MeshGenerator.Cube(x, y, z);
            CreateChildObject(mesh, WallMaterial, "Left Wall");

            // Right
            x = new Vector2(Width / 2.0f, (Width / 2.0f) + wallThickness);
            mesh = MeshGenerator.Cube(x, y, z);
            CreateChildObject(mesh, WallMaterial, "Right Wall");
        }

        private void GenerateCeiling()
        {
            var x = new Vector2(-Width / 2.0f, Width / 2.0f);
            var y = new Vector2(UnderFloorYPosition + Height, UnderFloorYPosition + Height + wallThickness);
            var z = new Vector2(-Length / 2.0f, Length / 2.0f);

            var mesh = MeshGenerator.Cube(x, y, z);
            CreateChildObject(mesh, CeilingMaterial, "Ceiling");
        }

        private void GenerateEnds()
        {
            var x = new Vector2(-Width / 2.0f, Width / 2.0f);
            var y = new Vector2(UnderFloorYPosition, UnderFloorYPosition + Height);

            // Front
            var z = new Vector2(Length / 2.0f, Length / 2.0f + wallThickness);
            var mesh = MeshGenerator.Cube(x, y, z);
            CreateChildObject(mesh, EndMaterial, "Front");

            // Back
            z = new Vector2(-Length / 2.0f, -Length / 2.0f - wallThickness);
            mesh = MeshGenerator.Cube(x, y, z);
            CreateChildObject(mesh, EndMaterial, "Back");
        }

        private void GeneratePlatform()
        {
            DimensionsHelper.OnReady(() =>
            {
                var x = new Vector2(DimensionsHelper.NegativeX, DimensionsHelper.PositiveX);
                var y = new Vector2(0, -PlatformHeight);
                var z = new Vector2(DimensionsHelper.ForwardZ, DimensionsHelper.BackZ);

                var mesh = MeshGenerator.Cube(x, y, z);
                CreateChildObject(mesh, PlatformMaterial, "Platform");
            });
        }

        private GameObject CreateChildObject(MeshInfo mesh, Material material, string name)
        {
            var obj = new GameObject();
            obj.transform.SetParent(root);

            var meshFilter = obj.AddComponent<MeshFilter>();
            var meshRenderer = obj.AddComponent<MeshRenderer>();

            meshFilter.mesh = mesh.Mesh;
            meshRenderer.material = material;

            obj.name = name;

            return obj;
        }
    }
}