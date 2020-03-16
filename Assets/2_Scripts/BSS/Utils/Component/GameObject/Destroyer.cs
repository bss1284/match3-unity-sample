using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BSS {
    public class Destroyer : MonoBehaviour {
        public event Action OnDestoryed;

        private Func<bool> OnDestroyCondition;

        private bool isInit;
        public void Initialize(Func<bool> condition,Action destroyAct=null) {
            if (isInit) return;
            isInit = true;
            OnDestroyCondition = condition;
            if (destroyAct != null) {
                OnDestoryed += destroyAct;
            }
        }

        private void Update() {
            if (!isInit) return;
            if (OnDestroyCondition == null) {
                OnDestoryed?.Invoke();
                Destroy(gameObject);
                return;
            }
            if (OnDestroyCondition.Invoke()==true) {
                OnDestoryed?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}