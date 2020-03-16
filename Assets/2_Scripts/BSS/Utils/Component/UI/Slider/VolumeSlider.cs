using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BSS {
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        void Start() {
            var slider = GetComponent<Slider>();
            slider.value = AudioListener.volume;
            slider.onValueChanged.AddListener((val) => {
                AudioListener.volume = val;
            });
        }
    }
}