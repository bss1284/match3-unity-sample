using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BSS.Extension {
#if SPINE
    using Spine.Unity;

    public static class SpineExtension {
        /// <summary>
        /// Spine의 Sorting Order를 변경합니다 (MeshRenderer의 SortingOrder) 
        /// </summary>
        public static void SetSortingOrder(this SkeletonAnimation spine, int order) {
            spine.GetComponent<MeshRenderer>().sortingOrder = order;
        }

        /// <summary>
        /// Spine의 애니메이션 이름을 가져옵니다.  
        /// </summary>
        public static string GetAnimationName(this SkeletonAnimation spine, int index) {
            var animations = spine.SkeletonDataAsset.GetSkeletonData(true).animations;
            return animations.Items[index].name;
        }

        /// <summary>
        /// Spine의 애니메이션 이름을 가져옵니다.  
        /// </summary>
        public static string GetAnimationName(this SkeletonGraphic spine, int index) {
            var animations = spine.SkeletonDataAsset.GetSkeletonData(true).animations;
            return animations.Items[index].name;
        }

        /// <summary>
        /// Spine의 애니메이션 길이를 가져옵니다. 
        /// </summary>
        public static float GetAnimationDuration(this SkeletonAnimation spine, int index) {
            var animations = spine.SkeletonDataAsset.GetSkeletonData(true).animations;
            return animations.Items[index].duration;
        }

        /// <summary>
        /// Spine의 애니메이션 길이를 가져옵니다. 
        /// </summary>
        public static float GetAnimationDuration(this SkeletonAnimation spine, string animName) {
            var animations = spine.SkeletonDataAsset.GetSkeletonData(true).animations;
            return animations.Find(x => x.name == animName).duration;
        }

        /// <summary>
        /// Spine의 애니메이션 길이를 가져옵니다. 
        /// </summary>
        public static float GetAnimationDuration(this SkeletonGraphic spine, int index) {
            var animations = spine.SkeletonDataAsset.GetSkeletonData(true).animations;
            return animations.Items[index].duration;
        }

        /// <summary>
        /// Spine의 애니메이션 길이를 가져옵니다. 
        /// </summary>
        public static float GetAnimationDuration(this SkeletonGraphic spine, string animName) {
            var animations = spine.SkeletonDataAsset.GetSkeletonData(true).animations;
            return animations.Find(x => x.name == animName).duration;
        }

        /// <summary>
        /// Spine을 특정 애니메이션으로 1회 재생합니다. 
        /// </summary>
        public static void PlayOnce(this SkeletonAnimation spine, string animName) {
            spine.AnimationState.SetAnimation(0, animName, false);
        }
        /// <summary>
        /// Spine을 특정 애니메이션으로 1회 재생합니다. 
        /// </summary>
        public static void PlayOnce(this SkeletonGraphic spine, string animName) {
            spine.AnimationState.SetAnimation(0, animName, false);
        }


        /// <summary>
        /// 특정 애니메이션이 완료 될 때 호출될 함수를 등록합니다. (Loop 애니메이션은 반복 호출) 
        /// </summary>
        public static void ListenComplete(this SkeletonAnimation spine, string animName, Action callback) {
            spine.AnimationState.Complete += (entry) => {
                if (entry.animation.name != animName) return;
                callback?.Invoke();
            };
        }
        /// <summary>
        /// 특정 애니메이션이 완료 될 때 호출될 함수를 등록합니다. (Loop 애니메이션은 반복 호출) 
        /// </summary>
        public static void ListenComplete(this SkeletonGraphic spine, string animName, Action callback) {
            spine.AnimationState.Complete += (entry) => {
                if (entry.animation.name != animName) return;
                callback?.Invoke();
            };
        }

    }
    public static class SpineEx {
    }
#endif
}


