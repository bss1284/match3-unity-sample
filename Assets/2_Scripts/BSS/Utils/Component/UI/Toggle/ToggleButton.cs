using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BSS {
    [RequireComponent(typeof(Toggle))]
    public class ToggleButton : MonoBehaviour {
        public UnityEvent trueEvent;
        public UnityEvent falseEvent;

        private void Awake()
        {
            var toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((b) => {
                if (b) {
                    trueEvent.Invoke();
                } else {
                    falseEvent.Invoke();
                }
            });
        }
    }
}
