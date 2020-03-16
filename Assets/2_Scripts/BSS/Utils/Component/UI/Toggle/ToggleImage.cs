using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BSS {
    public class ToggleImage : MonoBehaviour {
        public Toggle toggle;
        public Image targetImage;
        public Sprite isOffImage;
        public Sprite isOnImage;


        void Start() {
            if (toggle.isOn) {
                targetImage.sprite = isOnImage;
            } else {
                targetImage.sprite = isOffImage;
            }
            toggle.onValueChanged.AddListener((b) => {
                if (b) {
                    targetImage.sprite = isOnImage;
                } else {
                    targetImage.sprite = isOffImage;
                }
            });
        }

    }
}
