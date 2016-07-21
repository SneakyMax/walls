using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class DimensionsHelper : MonoBehaviour
    {
        [AssignedInUnity]
        public float Height = 3.5f;

        [AssignedInUnity]
        public float FloorDepth = 0.5f;

        [AssignedInUnity]
        public float WallDistanceFromSides = 1.0f;

        public bool IsReady { get; private set; }

        [AssignedInUnity]
        public SteamVR_PlayArea PlayArea;

        public static DimensionsHelper Instance { get; private set; }

        /// <summary>Play area dimensions start at the front left and go to the back right.</summary>
        private Rect playAreaDimensions;

        private IList<Action> readyActions;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            readyActions = new List<Action>();
            playAreaDimensions = new Rect();
        }

        [UnityMessage]
        public void Start()
        {
            StartCoroutine(WaitForDimensions());
        }

        public static void OnReady(Action action)
        {
            if (Instance.IsReady)
            {
                action();
                return;
            }

            Instance.readyActions.Add(action);
        }

        private IEnumerator WaitForDimensions()
        {
            while (PlayArea.vertices == null)
            {
                yield return null;
            }

            var verts = PlayArea.vertices;
            
            playAreaDimensions = new Rect(
                new Vector2(verts[2].x, verts[2].z),
                new Vector2(verts[0].x, verts[0].z) - new Vector2(verts[2].x, verts[2].z));

            SetReady();
        }

        private void SetReady()
        {
            IsReady = true;
            foreach (var readyThing in readyActions)
            {
                readyThing();
            }
        }

        public static float NegativeX
        {
            get { return Instance.playAreaDimensions.x; }
        }

        public static float NegativeXOutside
        {
            get { return NegativeX - Instance.WallDistanceFromSides; }
        }

        public static float PositiveX
        {
            get { return Instance.playAreaDimensions.x + Instance.playAreaDimensions.width; }
        }

        public static float PositiveXOutside
        {
            get
            {
                return PositiveX + Instance.WallDistanceFromSides;
            }
        }

        public static float ForwardZ
        {
            get { return Instance.playAreaDimensions.y; }
        }

        public static float BackZ
        {
            get { return Instance.playAreaDimensions.y + Instance.playAreaDimensions.height; }
        }

        public static Vector2 ZCoordinates
        {
            get { return new Vector2(ForwardZ, BackZ); }
        }

        public static Vector2 XCoordinates
        {
            get { return new Vector2(NegativeX, PositiveX); }
        }

        public static Vector4 FullCoordinates
        {
            get { return new Vector4(XCoordinates.x, XCoordinates.y, ZCoordinates.x, ZCoordinates.y); }
        }

        public static float ZHeight
        {
            get { return Instance.Height; }
        }

        public static float Floor { get { return 0; } }

        public static float UnderFloor
        {
            get { return -Instance.FloorDepth; }
        }
    }
}