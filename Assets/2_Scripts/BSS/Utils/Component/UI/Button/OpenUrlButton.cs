using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS.Extension;

namespace BSS.UI {
    [RequireComponent(typeof(Button))]
    public class OpenUrlButton : MonoBehaviour {
        public string url;

        void Start() {
            GetComponent<Button>().Listen(() => {
                Application.OpenURL(url);
            });
        }
    }
}