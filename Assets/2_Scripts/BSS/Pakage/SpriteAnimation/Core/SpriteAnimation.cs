using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace BSS {
    /// <summary>
    /// Spirte Sequence 애니메이션 파일
    /// </summary>
    [CreateAssetMenu(fileName = "NewSpriteAnimation", menuName = "BSS/Sprite Animation", order = 100)]
    public class SpriteAnimation : ScriptableObject {
        [ShowInInspector]
        public float realDuration => animFrames.Count == 0 ? 0f : GetRealDuration();

        

        [ReadOnly]
        public List<AnimationFrame> animFrames = new List<AnimationFrame>();

        [SerializeField]
        public int fps = 30;
        //Property
        public int FPS { get { return fps; } set { fps = value; } }
        public int FramesCount { get { return animFrames.Count; } }


        private float GetRealDuration() {
            int frameSum = animFrames.Sum(x => x.duration);
            return ((float)frameSum / (float)FPS);
        }


        [BoxGroup("Edit Mode")]
        [SerializeField]
        private List<Sprite> editFrames = new List<Sprite>();

        [BoxGroup("Edit Mode")]
        [Button(ButtonSizes.Medium)]
        private void Apply() {
            if (editFrames.Count == 0) return;
            editFrames.Sort((x,y) => {
                return x.name.CompareTo(y.name);
            });
            animFrames.Clear();
            for (int i = 0; i < editFrames.Count; i++) {
                animFrames.Add(new AnimationFrame(editFrames[i], 1));
            }
            editFrames.Clear();
        }

        [BoxGroup("Edit Mode")]
        [Button(ButtonSizes.Medium)]
        private void Clear() {
            animFrames.Clear();
        }

    }
}