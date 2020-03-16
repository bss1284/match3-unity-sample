using System;
using UnityEngine;

namespace BSS {
    [Serializable]
    public class AnimationFrame
    {
        public Sprite frame;
        public int duration=1;

        public AnimationFrame() { }
        public AnimationFrame(Sprite frame, int duration)
        {
            this.duration = duration;
            this.frame = frame;
        }
    }
}