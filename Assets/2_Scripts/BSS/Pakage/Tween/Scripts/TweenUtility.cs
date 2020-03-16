using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSS {
    public static class TweenUtility {
        /// <summary>
        /// TweenPlayer 컴포넌트를 가져와 횟수만큼 재생합니다.
        /// 컴포넌트가 없었다면 생성하여 재생 후 제거합니다.
        /// </summary>
        public static TweenPlayer SetTween(GameObject obj, TweenElement tweenElement,int count, System.Action endAction = null) {
            var tweenPlayer = obj.GetComponent<TweenPlayer>();
            if (tweenPlayer != null) {
                tweenPlayer.Play(tweenElement,count, endAction);
            } else {
                tweenPlayer = obj.AddComponent<TweenPlayer>();
                tweenPlayer.Play(tweenElement, count, () => {
                    if (tweenPlayer!=null) {
                        endAction?.Invoke();
                        GameObject.Destroy(tweenPlayer);
                    }
                });
            }
            return tweenPlayer;
        }
    }
}
