using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BSS.UI {
    [RequireComponent(typeof(Button))]
    public class ButtonEffect : MonoBehaviour {
        [Range(0f, 1f)]
        public float volume = 1f;
        public AudioClip clickSound;

        private AudioSource audioSource;

        // Use this for initialization
        void Awake() {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
        }
        private void Start() {
            GetComponent<Button>().onClick.AddListener(() => {
                if (clickSound != null) {
                    audioSource.PlayOneShot(clickSound);
                }
            });
        }
    }
}
