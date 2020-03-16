using UnityEngine;
using UnityEngine.UI;
using System;

namespace BSS.Extension {
    public static class ButtonExtension {
        public static void Listen(this Button button,Action act) {
            button.onClick.AddListener(()=> {
                if (act != null) { act.Invoke(); }
            });
        }
    }

}