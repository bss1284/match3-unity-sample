using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSS {
    /// <summary>
    /// 프로그램이 종료 될 때 까지 존재하는 싱글톤 (없을경우 생성)
    /// </summary>
    public class UniqueSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        public static T instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null) {
                        var obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected void Awake() {
            if (instance != this) {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(gameObject);

            OnAwake();
        }

        protected virtual void OnAwake() {
        }
    }
}
