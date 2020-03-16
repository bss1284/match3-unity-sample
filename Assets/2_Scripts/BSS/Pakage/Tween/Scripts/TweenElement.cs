using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BSS.Extension;
using UnityEngine.UI;

namespace BSS {
    [CreateAssetMenu(fileName = "NewTweenElement", menuName = "BSS/Tween Element",order =100)]
    public class TweenElement : ScriptableObject {
        [InfoBox("<b>Unit</b> : Y축의 1에 해당하는 값 \n<b>Freeze</b> : Tween효과에서 제외 \n[모든 Curve의 X축은 반드시 0과 1사이의 값이여야합니다.]\n ")]
        public float duration = 1f;


        [System.Serializable]
        public class AlphaTween {
            public bool isReset = true;
            public AnimationCurve curve;
        }
        [Header("Optional")]
        public bool isAlpha;
        [ShowIf("isAlpha")]
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
        [ShowIf("isScale")]
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
        [ShowIf("isPosition")]
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
        [ShowIf("isRotation")]
        public RotationTween rotationTween;
    }
}
