using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS.Extension;

namespace BSS.UI {
    /// <summary>
    /// View가 Show가 된 후 일정 시간 후 자동으로 Close 시켜주는 컴포넌트
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BaseView))]
    public class AutoClose : MonoBehaviour {
        public float autoCloseTime;

        
        private BaseView view;

        private void Awake()
        {
            view = GetComponent<BaseView>();
            view.OnShowAction += () => {
                StopAllCoroutines();
                StartCoroutine(CoAutoClose());
            };
        }

        /// <summary>
        /// 해당 View에 AutoClose 컴포넌트를 붙이고 적용합니다.
        /// </summary>
        public static AutoClose Apply(BaseView view, float time) {
            var autoClose=view.gameObject.GetOrAddComponent<AutoClose>();
            autoClose.autoCloseTime = time;
            return autoClose;
        }

        IEnumerator CoAutoClose() {
            yield return new WaitForSeconds(autoCloseTime);
            if (view.IsVisible) { view.Close(); }
        }
    }
}
