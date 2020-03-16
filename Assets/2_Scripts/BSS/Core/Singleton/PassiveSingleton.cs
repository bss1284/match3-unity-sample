using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BSS {
    /// <summary>
    /// Scene 전환시 제거되는 싱글톤 (없을경우 생성하지 않음)
    /// </summary>
    public class PassiveSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        public static T instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null) {
                        throw new Exception(typeof(T).Name + "is not exist.");
                    }
                }
                return _instance;
            }
        }
    }
}
