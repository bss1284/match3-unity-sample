using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BSS {
    public static class SpriteAnimationUtility  {
        /// <summary>
        /// Animator를 게임오브젝트를 생성해서 재생 후 파괴합니다.
        /// </summary>
        public static SpriteAnimator CreateAnimation(SpriteAnimation targetAnim, Vector3 pos,int sortOrder,int count,Action endAction=null) {
            if (count<=0) throw new Exception($"Count parameter is invalid");
            GameObject obj = new GameObject();
            obj.transform.position = pos;
            var animator = obj.AddComponent<SpriteAnimator>();
            animator.animations.Add(targetAnim);
            animator.spriteRenderer.sortingOrder = sortOrder;
            animator.PlayIndex(0,count, () => {
                if (animator == null) return;
                endAction?.Invoke();
                GameObject.Destroy(animator.gameObject);
            });
            return animator;
        }
        /// <summary>
        /// 애니메이터 컴포넌트를 가져와 재생합니다.
        /// 컴포넌트가 없었다면 컴포넌트 생성 후 끝나면 제거합니다.
        /// </summary>
        public static SpriteAnimator SetAnimation(SpriteRenderer renderer, SpriteAnimation targetAnim, int count, Action endAction = null) {
            if (count <= 0) throw new Exception($"Count parameter is invalid");
            var animator=renderer.GetComponent<SpriteAnimator>();
            Sprite preSprite = renderer.sprite;

            if (animator != null) {
                animator.Play(targetAnim, count, () => {
                    if (renderer != null) {
                        renderer.sprite = preSprite;
                    }
                    endAction?.Invoke();
                });
            } else {
                animator=renderer.gameObject.AddComponent<SpriteAnimator>();
                animator.Play(targetAnim, count, () => {
                    if (renderer != null) {
                        renderer.sprite = preSprite;
                    }
                    if (animator != null) {
                        endAction?.Invoke();
                        GameObject.Destroy(animator);
                    }
                });
            }
            return animator;
        }
    }
}
