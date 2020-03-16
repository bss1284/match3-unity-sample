using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BSS {
    [RequireComponent(typeof(Collider2D))]
    public class Clickable : MonoBehaviour {
        public int priority;
        public bool isOnce;
        public bool isDouble;

        public event Action<GameObject> OnOnceClickAction;
        public event Action<GameObject> OnDoubleClickAction;

        private void Start() {
            ClickSystem.instance.OnOnceClickAction += (clickable) => {
                if (clickable != this) return;
                OnOnceClickAction?.Invoke(gameObject);
            };
            ClickSystem.instance.OnDoubleClickAction += (clickable) => {
                if (clickable != this) return;
                OnDoubleClickAction?.Invoke(gameObject);
            };
        }
    }
    
}
