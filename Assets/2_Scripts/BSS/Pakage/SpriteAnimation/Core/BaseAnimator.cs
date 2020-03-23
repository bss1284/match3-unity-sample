using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace BSS {
    
    public abstract class BaseAnimator : MonoBehaviour
    {
        public enum PlayOnAwake {
            None,Once,Loop
        }
        public class PlayListener {
            public SpriteAnimation targetAnimation;
            public int count;
            public Action endAction;
        }

        public List<SpriteAnimation> animations = new List<SpriteAnimation>();

        
        [SerializeField]
        public SpriteAnimation baseAnimation { get; private set; }
        
        public bool isPlaying { get { return playListener != null; } }

        [SerializeField]
        private PlayOnAwake playOnAwake = PlayOnAwake.None;



        private PlayListener playListener;
        private int baseIndex;
        private int playCount;
        private int playIndex;
        private IEnumerator playCoroutine;


        protected virtual void Awake() {
            if (playOnAwake == PlayOnAwake.Once) {
                PlayIndex(0);
            } else if (playOnAwake == PlayOnAwake.Loop) {
                SetBaseAnimation(0);
            }
            
        }

        private void OnEnable() {
            StartCoroutine(CoUpdateLoop());
            if (playCoroutine != null) {
                StartCoroutine(playCoroutine);
            }
        }

        public void PlayIndex(int animIndex) {
            Play(animations[animIndex], 1, null);
        }
        public void PlayIndex(int animIndex, Action endAction) {
            Play(animations[animIndex], 1, endAction);
        }
        public void PlayIndex(int animIndex,int count) {
            if (count <= 0) throw new Exception($"Count parameter is invalid");
            Play(animations[animIndex], count, null);
        }
        public void PlayIndex(int animIndex, int count,Action endAction) {
            if (count <= 0) throw new Exception($"Count parameter is invalid");
            Play(animations[animIndex], count, endAction);
        }

        public void Play(SpriteAnimation animation) {
            Play(animation, 1, null);
        }
        public void Play(SpriteAnimation animation, Action endAction) {
            Play(animation, 1, endAction);
        }
        public void Play(SpriteAnimation animation, int count) {
            if (count <= 0) throw new Exception($"Count parameter is invalid");
            Play(animation, count, null);
        }
        public void Play(SpriteAnimation animation, int count,Action endAction) {
            if (count <= 0) throw new Exception($"Count parameter is invalid");
            if (playCoroutine != null) {
                Stop();
            }
            playListener = new PlayListener();
            playListener.count = count;
            playListener.targetAnimation = animation;
            playListener.endAction = endAction;
            playCount = 0;
            playIndex = 0;
            playCoroutine = CoPlay();
            StartCoroutine(playCoroutine);
        }

        public void Stop() {
            if (playCoroutine == null) return;
            StopCoroutine(playCoroutine);
            if (playListener != null && playListener.endAction!=null) {
                playListener.endAction.Invoke();
            }
            playListener = null;
            playCoroutine = null;
        }

        public void SetBaseAnimation(SpriteAnimation animation) {
            if (baseAnimation == animation) return;
            baseIndex = 0;
            baseAnimation = animation;
        }
        public void SetBaseAnimation(int animIndex) {
            SetBaseAnimation(animations[animIndex]);
        }
        public void ClearBaseAnimation() {
            baseIndex = 0;
            baseAnimation = null;
        }

        public float GetDuration(int animIndex) {
            return animations[animIndex].realDuration;
        }
        public float GetDuration(string animName) {
            return animations.Find(x => x.name == animName).realDuration;
        }

        


        IEnumerator CoPlay() {
            var animation = playListener.targetAnimation;
            while (true) {
                ChangeFrame(animation.animFrames[playIndex].frame);
                float duration = (float)animation.animFrames[playIndex].duration * (1f / (float)animation.FPS); ;
                yield return new WaitForSeconds(duration);
                playIndex++;
                if (playIndex > animation.FramesCount - 1) {
                    playCount++;
                    if (playCount >= playListener.count) {
                        Stop();
                        yield return null;
                    } else {
                        playIndex = 0;
                    }
                }
            }
        }

        IEnumerator CoUpdateLoop() {
            while (true) {
                yield return null;
                if (playListener != null) continue;
                if (baseAnimation == null) continue;

                float duration = 1f / (float)baseAnimation.FPS;
                var sprite = baseAnimation.animFrames[baseIndex].frame;
                float realDuration = (float)baseAnimation.animFrames[baseIndex].duration * duration;
                ChangeFrame(sprite);
                baseIndex = (baseIndex + 1) % baseAnimation.FramesCount;
                yield return new WaitForSeconds(realDuration);
            }
        }


        protected virtual void ChangeFrame(Sprite frame) { }
    }
}