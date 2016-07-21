using System;
using System.Collections.Generic;
using Assets._Scripts.Helpers;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class WallDescription : MonoBehaviour, IWallDescription
    {
        [AssignedInUnity]
        public string Name;

        [AssignedInUnity, Range(0, 1)]
        public float LeftPositionFromLeft;

        [AssignedInUnity, Range(0, 1)]
        public float RightPositionFromLeft = 1;

        [AssignedInUnity, Range(0, 1)]
        public float TopPositionFromTop;

        [AssignedInUnity, Range(0, 1)]
        public float BottomPositionFromTop = 1;

        [UnityMessage]
        public void Awake()
        {
            if (LeftPositionFromLeft > RightPositionFromLeft || TopPositionFromTop > BottomPositionFromTop)
            {
                throw new InvalidOperationException(Name + " Left must be less than right or top must be less than bottom.");
            }
        }

        public IEnumerable<MeshInfo> GenerateSolidMeshes(float thickness)
        {
            var halfWidth = thickness / 2.0f;

            var leftBound = DimensionsHelper.NegativeXOutside;
            var rightBound = DimensionsHelper.PositiveXOutside;

            var fullWidth = rightBound - leftBound;

            var topBound = DimensionsHelper.ZHeight;
            var bottomBound = DimensionsHelper.UnderFloor;

            var fullHeight = topBound - bottomBound;

            var left = leftBound + LeftPositionFromLeft * fullWidth;
            var right = leftBound + RightPositionFromLeft * fullWidth;

            var top = topBound - fullHeight * TopPositionFromTop;
            var bottom = topBound - fullHeight * BottomPositionFromTop;

            return new List<MeshInfo>
            {
                MeshGenerator.Cube(
                    new Vector2(left, right),
                    new Vector2(bottom, top),
                    new Vector2(-halfWidth, halfWidth))
            };
        }
    }
}