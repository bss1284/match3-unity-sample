using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS.Extension;

namespace BSS {
    [RequireComponent(typeof(Button))]
    public class NotifyButton : MonoBehaviour {
        [SerializeField]
        private string eventName;

        public void Init(string _eventName) {
            eventName = _eventName;
        }

        void Awake() {
            GetComponent<Button>().Listen(() => {
                NotifySystem.Trigger("Button", gameObject, "");
                NotifySystem.Trigger(eventName, gameObject,null);
            });
        }
    }
}
