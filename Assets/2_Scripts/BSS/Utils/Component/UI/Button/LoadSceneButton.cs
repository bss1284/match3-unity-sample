using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS.Extension;

namespace BSS.UI {
    [RequireComponent(typeof(Button))]
    public class LoadSceneButton : MonoBehaviour {
        public int sceneIndex;

        void Start() {
            GetComponent<Button>().Listen(() => {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
            });
        }
    }
}
