using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSS {
    public class BezierMover : MonoBehaviour {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public Vector3 curvePosition;

        
        
        public float value {
            get => _value;
            set {
                _value = value;
                ToBezier(_value);
            }
            
        }
        [SerializeField]
        [Range(0f,1f)]
        private float _value;

        private void OnValidate() {
            ToBezier(value);
        }

        private void ToBezier(float t) {
            Vector3 p1 = Vector3.Lerp(startPosition, curvePosition, t);
            Vector3 p2 = Vector3.Lerp(curvePosition, endPosition, t);
            Vector3 p3 = Vector3.Lerp(p1, p2, t);
            transform.position = p3;
        }
    }
}
