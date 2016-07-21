using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class DimensionsHelper : MonoBehaviour
    {
        public bool IsReady { get; private set; }

        [AssignedInUnity]
        public RoomGenerator RoomGenerator;

        [AssignedInUnity]
        public SteamVR_PlayArea PlayArea;

        public static DimensionsHelper Instance { get; private set; }

        /// <summary>Play area dimensions start at the front left and go to the back right.</summary>
        private Rect playAreaDimensions;

        private IList<Action> readyActions;

        public const float PlayAreaHeight = 2.032f; // 6 feet 8 inches

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
            get { return -Instance.RoomGenerator.Width / 2.0f; }
        }

        public static float PositiveX
        {
            get { return Instance.playAreaDimensions.x + Instance.playAreaDimensions.width; }
        }

        public static float PositiveXOutside
        {
            get
            {
                return Instance.RoomGenerator.Width / 2.0f;
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

        public static float ZPlayAreaTop { get { return PlayAreaHeight; } }

        public static float ZCeilingTop {get { return Instance.RoomGenerator.Height - Instance.RoomGenerator.PlatformHeight; } }

        public static float Floor { get { return 0; } }

        public static float UnderFloor
        {
            get { return -Instance.RoomGenerator.PlatformHeight; }
        }
    }
}