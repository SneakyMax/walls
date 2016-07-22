using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Assets._Scripts.Helpers
{
    public class MeshInfo
    {
        public Mesh Mesh { get; set; }

        public Bounds Bounds { get; set; }
    }

    public static class MeshGenerator
    {
        public static MeshInfo Cube(Bounds bounds)
        {
            return Cube(
                new Vector2(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x),
                new Vector2(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y),
                new Vector2(bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z));
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        // http://wiki.unity3d.com/index.php/ProceduralPrimitives
        public static MeshInfo Cube(Vector2 xPositions, Vector2 yPositions, Vector2 zPositions)
        {
            /*Mesh mesh = new Mesh();

            float length = 1f;
            float width = 1f;
            float height = 1f;

            #region Vertices
            Vector3 p0 = new Vector3(-length * .5f, -width * .5f, height * .5f);
            Vector3 p1 = new Vector3(length * .5f, -width * .5f, height * .5f);
            Vector3 p2 = new Vector3(length * .5f, -width * .5f, -height * .5f);
            Vector3 p3 = new Vector3(-length * .5f, -width * .5f, -height * .5f);

            Vector3 p4 = new Vector3(-length * .5f, width * .5f, height * .5f);
            Vector3 p5 = new Vector3(length * .5f, width * .5f, height * .5f);
            Vector3 p6 = new Vector3(length * .5f, width * .5f, -height * .5f);
            Vector3 p7 = new Vector3(-length * .5f, width * .5f, -height * .5f);
            */
            
            var left = xPositions.x;
            var right = xPositions.y;

            if (left > right)
                Swap(ref left, ref right);

            var top = yPositions.x;
            var bottom = yPositions.y;

            if (bottom > top)
                Swap(ref bottom, ref top);

            var forward = zPositions.x;
            var backward = zPositions.y;

            if (backward > forward)
                Swap(ref backward, ref forward);

            var width = right - left;
            var height = top - bottom;
            var depth = forward - backward;

            var xCenter = (left + right) / 2.0f;
            var yCenter = (bottom + top) / 2.0f;
            var zCenter = (backward + forward) / 2.0f;

            var bounds = new Bounds(
                new Vector3(xCenter, yCenter, zCenter),
                new Vector3(width, height, depth));

            var mesh = new Mesh();
            var p0 = new Vector3(left, bottom, forward);
            var p1 = new Vector3(right, bottom, forward);
            var p2 = new Vector3(right, bottom, backward);
            var p3 = new Vector3(left, bottom, backward);
            var p4 = new Vector3(left, top, forward);
            var p5 = new Vector3(right, top, forward);
            var p6 = new Vector3(right, top, backward);
            var p7 = new Vector3(left, top, backward);


            Vector3[] vertices = 
            {
	            // Bottom
	            p0, p1, p2, p3,
 
	            // Left
	            p7, p4, p0, p3,
 
	            // Front
	            p4, p5, p1, p0,
 
	            // Back
	            p6, p7, p3, p2,
 
	            // Right
	            p5, p6, p2, p1,
 
	            // Top
	            p7, p6, p5, p4
            };
            
            var up = Vector3.up;
            var down = Vector3.down;
            var front = Vector3.forward;
            var back = Vector3.back;
            var vLeft = Vector3.left;
            var vRight = Vector3.right;

            Vector3[] normals = 
            {
	            // Bottom
	            down, down, down, down,
 
	            // Left
	            vLeft, vLeft, vLeft, vLeft,
 
	            // Front
	            front, front, front, front,
 
	            // Back
	            back, back, back, back,
 
	            // Right
	            vRight, vRight, vRight, vRight,
 
	            // Top
	            up, up, up, up
            };
            
            var uvs = GetCubeUVsTiling(width, height, depth);
            
            int[] triangles = 
            {
	            // Bottom
	            3, 1, 0,
                3, 2, 1,			
 
	            // Left
	            3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
                3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
 
	            // Front
	            3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
                3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
 
	            // Back
	            3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
                3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
 
	            // Right
	            3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
                3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
 
	            // Top
	            3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
                3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5
            };

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
            mesh.Optimize();

            return new MeshInfo
            {
                Mesh = mesh,
                Bounds = bounds
            };
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static Vector2[] GetCubeUVsStretched()
        {
            var _00 = new Vector2(0f, 0f);
            var _10 = new Vector2(1f, 0f);
            var _01 = new Vector2(0f, 1f);
            var _11 = new Vector2(1f, 1f);

            return new[]
            {
                // Bottom
	            _11, _01, _00, _10,
 
	            // Left
	            _11, _01, _00, _10,
 
	            // Front
	            _11, _01, _00, _10,
 
	            // Back
	            _11, _01, _00, _10,
 
	            // Right
	            _11, _01, _00, _10,
 
	            // Top
	            _11, _01, _00, _10,
            };
        }

        private static Vector2[] GetCubeUVsTiling(float width, float height, float depth)
        {
            width = Mathf.Abs(width);
            height = Mathf.Abs(height);
            depth = Mathf.Abs(depth);

            return new[]
            {
                // Bottom
                new Vector2(width, depth),
                new Vector2(0, depth),
                new Vector2(0, 0),
                new Vector2(width, 0),

                // Left
                new Vector2(depth, height),
                new Vector2(0, height),
                new Vector2(0, 0),
                new Vector2(depth, 0),

                // Front
                new Vector2(width, height),
                new Vector2(0, height),
                new Vector2(0, 0),
                new Vector2(width, 0),

                // Back
                new Vector2(width, height),
                new Vector2(0, height),
                new Vector2(0, 0),
                new Vector2(width, 0),

                // Right
                new Vector2(depth, height),
                new Vector2(0, height),
                new Vector2(0, 0),
                new Vector2(depth, 0),

                // Top
                new Vector2(width, depth),
                new Vector2(0, depth),
                new Vector2(0, 0),
                new Vector2(width, 0)
            };
        }
    }
}