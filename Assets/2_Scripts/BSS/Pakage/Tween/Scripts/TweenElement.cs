using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS.Extension;
using UnityEngine.UI;

namespace BSS {
    [CreateAssetMenu(fileName = "NewTweenElement", menuName = "BSS/Tween Element",order =100)]
    public class TweenElement : ScriptableObject {
        public float duration = 1f;


        [System.Serializable]
        public class AlphaTween {
            public bool isReset = true;
            public AnimationCurve curve;
        }
        [Header("Optional")]
        public bool isAlpha;
        public AlphaTween alphaTween = new AlphaTween();


        [System.Serializable]
        public class ScaleTween {
            public bool isReset = true;
            public AnimationCurve curve;
            public Vector3 unitScale = Vector3.one;
            public bool freezeX;
            public bool freezeY;
            public bool freezeZ;
        }
        public bool isScale;
        public ScaleTween scaleTween = new ScaleTween();


        [System.Serializable]
        public class PositionTween {
            public bool isReset = true;
            public AnimationCurve curve;
            public Vector3 uniPosition=Vector3.one;
            public bool freezeX;
            public bool freezeY;
            public bool freezeZ;
        }
        public bool isPosition;
        public PositionTween positionTween;

        [System.Serializable]
        public class RotationTween {
            public bool isReset = true;
            public AnimationCurve curve;
            public Vector3 unitRotation = Vector3.one*360f;
            public bool freezeX;
            public bool freezeY;
            public bool freezeZ;
        }
        public bool isRotation;
        public RotationTween rotationTween;
    }
}
