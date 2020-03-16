using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS;
using System;
using Sirenix.OdinInspector;

namespace BSS.UI {
    /// <summary>
    /// 숫자 텍스트가 순차적으로 증가하는 UI
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class IncreaseText : MonoBehaviour {
        [HideInEditorMode]
        public bool isUpdate;
        
        public event Action<int> OnRequestAction;//Action<Value>
        public event Action OnUpdateAction;
        public event Action OnEndAction;

        public float rate {
            get { return (currentVal -realVal); }
        }
        public int difference {
            get { return Mathf.Abs(realVal - currentVal); }
        }

        [SerializeField]
        public string textFormat = "0";

        
        private int realVal;
        private int currentVal;

        private Text mText;

        private void Awake() {
            mText = GetComponent<Text>();
        }

        private void Start() {
            UpdateText();
        }


        public void SetNumber(int num,float time) {
            if (realVal == num) return;
            if (time<=0f) {
                SetNumberAtOnce(num);
                return;
            }
            isUpdate = true;
            StopAllCoroutines();
            currentVal = realVal;
            realVal = num;
            OnRequestAction?.Invoke(num);
            StartCoroutine(CoIncrease(GetAddAmount(time)));
        }
        public void SetNumberAtOnce(int num) {
            isUpdate = false;
            StopAllCoroutines();
            realVal = num;
            currentVal = realVal;
            UpdateText();
        }

        public int GetRealValue() {
            return realVal;
        }
        public void Skip() {
            isUpdate = false;
            if (currentVal == realVal) return;
            currentVal = realVal;
            UpdateText();
        }

        private void UpdateText() {
            mText.text = currentVal.ToString(textFormat);
        }

        IEnumerator CoIncrease(int addAmount) {
            while (true) {
                currentVal += addAmount;
                UpdateText();
                OnUpdateAction?.Invoke();
                if (IsCompare(addAmount)) {
                    currentVal = realVal;
                    UpdateText();
                    isUpdate = false;
                    OnEndAction?.Invoke();
                    yield break;
                }
                yield return null;
            }
        }

        private int GetAddAmount(float time) {
            bool isMinus = realVal < currentVal;

            int frameCount = (int)(time * 60f);
            int result= (int)((float)difference / (float)frameCount);
            if (result==0) {
                result = 1;
            }
            if (isMinus) {
                result = -result;
            }
            return result;
        }

        private bool IsCompare(int addAmount) {
            if (currentVal == realVal) return true;
            int dif = Mathf.Abs(realVal - currentVal);
            if (dif < Mathf.Abs(addAmount)+1) {
                return true;
            }
            return false;
        } 
    }
}
