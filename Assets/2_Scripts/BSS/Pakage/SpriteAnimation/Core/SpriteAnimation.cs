using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BSS {
    /// <summary>
    /// Spirte Sequence 애니메이션 파일
    /// </summary>
    [CreateAssetMenu(fileName = "NewSpriteAnimation", menuName = "BSS/Sprite Animation", order = 100)]
    public class SpriteAnimation : ScriptableObject {
        public float realDuration => animFrames.Count == 0 ? 0f : GetRealDuration();

        

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


        [SerializeField]
        private List<Sprite> editFrames = new List<Sprite>();

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

        private void Clear() {
            animFrames.Clear();
        }

    }
}