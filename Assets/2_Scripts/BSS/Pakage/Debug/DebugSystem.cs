using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS.Extension;

namespace BSS {
    /// <summary>
    /// 에디터나 개발 모드에서만 로그를 남기는 시스템 (빌드시 제외)
    /// </summary>
    public class DebugSystem {
        public static bool IsDebugMode=true;

        public static void Log(object message) {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (IsDebugMode) {
                Debug.Log(message);
            }
            #endif
        }
        public static void Log(object message,Color color) {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (IsDebugMode) {
                Debug.Log($"<color={color.ToHex()}>{message}</color>");
            }
            #endif
        }

        public static void LogError(object message) {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (IsDebugMode) {
                Debug.LogError(message);
            }
            #endif
        }

        public static void LogWarning(object message) {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (IsDebugMode) {
                Debug.LogWarning(message);
            }
            #endif
        }
    }
}
