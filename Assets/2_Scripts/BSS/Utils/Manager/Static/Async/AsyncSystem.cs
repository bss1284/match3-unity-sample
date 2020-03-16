using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BSS {
    /// <summary>
    /// 비동기 함수를 쉽게 사용하게 하는 클래스
    /// </summary>
    public class AsyncSystem : Singleton<AsyncSystem> {
        public static void ExcuteAfterSeconds(float seconds, Action act) {
            instance.StartCoroutine(instance.CoExcuteAfterSeconds(seconds, act)); ;
        }

        public static void ExcuteAfterCondition(Func<bool> condition,Action act) {
            instance.StartCoroutine(instance.CoExcuteAfterCondition(condition, act));
        }
        public static void ExcuteWhileCondition(Func<bool> condition, Action act) {
            instance.StartCoroutine(instance.CoExcuteWhileCondition(condition, act));
        }
        public static void ExcuteAfterFrame(int frameCount,Action act) {
            instance.StartCoroutine(instance.CoExcuteAfterFrame(frameCount, act));;
        }

        IEnumerator CoExcuteAfterCondition(Func<bool> condition, Action act) {
            yield return new WaitUntil(condition);
            act?.Invoke();
        }
        IEnumerator CoExcuteWhileCondition(Func<bool> condition, Action act) {
            while (true) {
                if (condition == null || !condition.Invoke()) yield break;
                act?.Invoke();
                yield return null;
            }
        }
        IEnumerator CoExcuteAfterFrame(int frameCount, Action act) {
            for (int i = 0; i < frameCount; i++) {
                yield return null;
            }
            act?.Invoke();
        }
        IEnumerator CoExcuteAfterSeconds(float seconds, Action act) {
            yield return new WaitForSeconds(seconds);
            act?.Invoke();
        }
    }
}
