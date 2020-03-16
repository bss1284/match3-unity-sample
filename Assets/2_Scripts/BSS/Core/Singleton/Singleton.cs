using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSS {
    /// <summary>
    /// Scene 전환시 제거되는 싱글톤 (없을경우 생성)
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour {
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
    }

    
}
