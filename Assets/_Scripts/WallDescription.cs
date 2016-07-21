using System;
using System.Collections.Generic;
using Assets._Scripts.Helpers;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class WallDescription : MonoBehaviour, IWallDescription
    {
        public string DescriptionName {get { return Name; } }

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

            var leftOuterBound = DimensionsHelper.NegativeXOutside;
            var rightOuterBound = DimensionsHelper.PositiveXOutside;

            var leftBound = DimensionsHelper.NegativeX;
            var rightBound = DimensionsHelper.PositiveX;

            var fullWidth = rightOuterBound - leftOuterBound;
            var innerWidth = rightBound - leftBound;

            var topCeilingBound = DimensionsHelper.ZCeilingTop;
            var topPlayAreaBound = DimensionsHelper.ZPlayAreaTop;
            
            var bottomBound = DimensionsHelper.UnderFloor;
            var bottomBoundPlayArea = 0;

            var fullHeight = topCeilingBound - bottomBound;
            var innerHeight = topPlayAreaBound - bottomBoundPlayArea;

            var left = LeftPositionFromLeft < 0.01f ?
                leftOuterBound :
                leftBound + LeftPositionFromLeft * innerWidth;

            var right = RightPositionFromLeft > 0.99f ?
                rightOuterBound :
                leftBound + RightPositionFromLeft * innerWidth;

            var top = TopPositionFromTop < 0.01f ?
                topCeilingBound :
                topPlayAreaBound - TopPositionFromTop * innerHeight;

            var bottom = BottomPositionFromTop > 0.99 ?
                bottomBound :
                topPlayAreaBound - BottomPositionFromTop * innerHeight;

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