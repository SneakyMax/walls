using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class PositionRelativeToPlayArea : MonoBehaviour
    {
        [AssignedInUnity]
        public RelativeTo RelativeTo;

        [AssignedInUnity]
        public Vector3 Offset;

        private RelativeTo lastRelativeTo;

        private Vector3 lastOffset;
        
        [UnityMessage]
        public void Start()
        {
            DimensionsHelper.OnReady(RecalculatePosition);
        }

        [UnityMessage]
        public void Update()
        {
            if (Offset != lastOffset || RelativeTo != lastRelativeTo)
                RecalculatePosition();
        }

        private void RecalculatePosition()
        {
            var start = Vector3.zero;

            switch (RelativeTo)
            {
                case RelativeTo.FrontLeft:
                    start = new Vector3(DimensionsHelper.NegativeX, DimensionsHelper.Floor, DimensionsHelper.ForwardZ);
                    break;
                case RelativeTo.FrontRight:
                    start = new Vector3(DimensionsHelper.PositiveX, DimensionsHelper.Floor, DimensionsHelper.ForwardZ);
                    break;
                case RelativeTo.BackLeft:
                    start = new Vector3(DimensionsHelper.NegativeX, DimensionsHelper.Floor, DimensionsHelper.BackZ);
                    break;
                case RelativeTo.BackRight:
                    start = new Vector3(DimensionsHelper.PositiveX, DimensionsHelper.Floor, DimensionsHelper.BackZ);
                    break;
                case RelativeTo.Center:
                    start = new Vector3((DimensionsHelper.NegativeX + DimensionsHelper.PositiveX) / 2.0f, DimensionsHelper.Floor, (DimensionsHelper.ForwardZ + DimensionsHelper.BackZ) / 2.0f);
                    break;
                case RelativeTo.TopFrontLeft:
                    start = new Vector3(DimensionsHelper.NegativeX, DimensionsHelper.ZPlayAreaTop, DimensionsHelper.ForwardZ);
                    break;
                case RelativeTo.TopFrontRight:
                    start = new Vector3(DimensionsHelper.PositiveX, DimensionsHelper.ZPlayAreaTop, DimensionsHelper.ForwardZ);
                    break;
                case RelativeTo.TopBackLeft:
                    start = new Vector3(DimensionsHelper.NegativeX, DimensionsHelper.ZPlayAreaTop, DimensionsHelper.BackZ);
                    break;
                case RelativeTo.TopBackRight:
                    start = new Vector3(DimensionsHelper.PositiveX, DimensionsHelper.ZPlayAreaTop, DimensionsHelper.BackZ);
                    break;
                case RelativeTo.TopCenter:
                    start = new Vector3((DimensionsHelper.NegativeX + DimensionsHelper.PositiveX) / 2.0f, DimensionsHelper.ZPlayAreaTop, (DimensionsHelper.ForwardZ + DimensionsHelper.BackZ) / 2.0f);
                    break;
                case RelativeTo.DeadCenter:
                    start = new Vector3((DimensionsHelper.NegativeX + DimensionsHelper.PositiveX) / 2.0f, (DimensionsHelper.Floor + DimensionsHelper.ZPlayAreaTop) / 2.0f, (DimensionsHelper.ForwardZ + DimensionsHelper.BackZ) / 2.0f);
                    break;
            }

            transform.position = start + Offset;

            lastRelativeTo = RelativeTo;
            lastOffset = Offset;
        }
    }
}