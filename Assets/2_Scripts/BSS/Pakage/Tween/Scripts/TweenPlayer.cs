using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS.UI;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using BSS.Extension;
using System;


namespace BSS {
    public class TweenPlayer : MonoBehaviour {
        public class PreviousData {
            public PreviousData(float _alpha, Vector3 _scale,Vector3 _position,Quaternion _rotation) {
                alpha = _alpha;
                scale = _scale;
                position = _position;
                rotation = _rotation;
            }

            public float alpha;
            public Vector3 scale;
            public Vector3 position;
            public Quaternion rotation;
        }
        public bool isPlaying { get; private set; }
        public bool isLoop = false;
        

        [SerializeField]
        private bool onStartPlaying;
        [SerializeField]
        [ShowIf("onStartPlaying")]
        private TweenElement startElement;

        private int playCount;
        private int targetCount;
        private PreviousData preData;

        private void Start() {
            if (onStartPlaying) {
                Play(startElement,1);
            }
        }

        public void Play(TweenElement element) {
            Play(element, 1, null);
        }

        public void Play(TweenElement element, Action endAction) {
            Play(element, 1, endAction);
        }
        public void Play(TweenElement element,int count) {
            Play(element, count, null);
        }

        public void Play(TweenElement element,int count,Action endAction) {
            if (isPlaying) return;
            if (count <= 0) return;
            if (element.duration <= 0f) return;

            playCount = 0;
            targetCount = count;
            preData = new PreviousData(GetAlpha(element), transform.localScale, transform.position, transform.rotation);
            StartCoroutine(CoPlayTween(element, endAction));
        }

        public void Stop() {
            if (preData == null || !isPlaying) return;
            StopAllCoroutines();
            ApplyAlpha(preData.alpha);
            transform.localScale = preData.scale;
            transform.position = preData.position;
            transform.rotation = preData.rotation;
            isPlaying = false;
            preData = null;
        }


        IEnumerator CoPlayTween(TweenElement element, Action endAction) {
            isPlaying = true;

            float duration = element.duration;
            
            float t = 0f;
            while (true) {
                if (element.isAlpha) {
                    AnimationCurve alphaCurve = element.alphaTween.curve;
                    float alphaValue = Mathf.Clamp01(alphaCurve.Evaluate(t / duration));
                    ApplyAlpha(alphaValue);
                }
                if (element.isScale) {
                    AnimationCurve scaleCurve = element.scaleTween.curve;
                    float scaleValue = scaleCurve.Evaluate(t / duration);
                    transform.localScale = element.scaleTween.unitScale * scaleValue;

                    if (element.scaleTween.freezeX) {
                        transform.localScale = transform.localScale.SetX(preData.scale.x);
                    }
                    if (element.scaleTween.freezeY) {
                        transform.localScale = transform.localScale.SetY(preData.scale.y);
                    }
                    if (element.scaleTween.freezeZ) {
                        transform.localScale = transform.localScale.SetZ(preData.scale.z);
                    }
                }
                if (element.isPosition) {
                    AnimationCurve posCurve = element.positionTween.curve;
                    float posValue = posCurve.Evaluate(t / duration);
                    transform.position = preData.position+(element.positionTween.uniPosition * posValue);

                    if (element.positionTween.freezeX) {
                        transform.position = transform.position.SetX(preData.position.x);
                    }
                    if (element.positionTween.freezeY) {
                        transform.position = transform.position.SetY(preData.position.y);
                    }
                    if (element.positionTween.freezeZ) {
                        transform.position = transform.position.SetZ(preData.position.z);
                    }
                }
                if (element.isRotation) {
                    AnimationCurve rotCurve = element.rotationTween.curve;
                    float rotValue = rotCurve.Evaluate(t / duration);
                    Vector3 sourceVector = element.rotationTween.unitRotation * rotValue;
                    
                    if (element.rotationTween.freezeX) {
                        sourceVector = sourceVector.SetX(preData.rotation.eulerAngles.x);
                    }
                    if (element.rotationTween.freezeY) {
                        sourceVector = sourceVector.SetY(preData.rotation.eulerAngles.y);
                    }
                    if (element.rotationTween.freezeZ) {
                        sourceVector = sourceVector.SetZ(preData.rotation.eulerAngles.z);
                    }
                    transform.rotation = Quaternion.Euler(sourceVector);
                }


                t += Time.deltaTime;
                if (t > element.duration) {
                    break;
                }
                yield return null;
            }

            if (element.isAlpha && element.alphaTween.isReset) {
                ApplyAlpha(preData.alpha);
            }
            if (element.isScale && element.scaleTween.isReset) {
                transform.localScale = preData.scale;
            }
            if (element.isPosition && element.positionTween.isReset) {
                transform.position = preData.position;
            }
            if (element.isRotation && element.rotationTween.isReset) {
                transform.rotation = preData.rotation;
            }
            playCount += 1;

            if (isLoop || playCount < targetCount) {
                StartCoroutine(CoPlayTween(element, endAction));
                yield break;
            }

            isPlaying = false;
            preData = null;
            endAction?.Invoke();
        }

        private float GetAlpha(TweenElement element) {
            var cg = GetComponent<CanvasGroup>();
            if (cg != null) {
                return cg.alpha;
            }
            var render = GetComponent<SpriteRenderer>();
            if (render != null) {
                return render.color.a;
            }
            var image = GetComponent<Image>();
            if (image != null) {
                return image.color.a;
            }
            return 0f;
        }

        private void ApplyAlpha(float value) {
            var cg = GetComponent<CanvasGroup>();
            if (cg != null) {
                cg.alpha = value;
                return;
            }
            var render = GetComponent<SpriteRenderer>();
            if (render != null) {
                render.color = new Color(render.color.r, render.color.g, render.color.b, value);
                return;
            }
            var image = GetComponent<Image>();
            if (image != null) {
                image.color = new Color(image.color.r, image.color.g, image.color.b, value);
                return;
            }
            var text = GetComponent<Text>();
            if (text != null) {
                text.color = new Color(text.color.r, text.color.g, text.color.b, value);
                return;
            }
        }
    }
}
