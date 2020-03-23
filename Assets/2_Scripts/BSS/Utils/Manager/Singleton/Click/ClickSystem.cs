using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BSS.Extension;

namespace BSS {
    public class ClickSystem : Singleton<ClickSystem> {
        public event Action<Clickable> OnOnceClickAction;
        public event Action<Clickable> OnDoubleClickAction;
        [SerializeField]
        private float doubleInterval = 0.2f;


        private float clickTime;
        private bool isWait;



        private void Update() {
            if (!Input.GetMouseButtonDown(0)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (isWait) return;
            StartCoroutine(CoWaitClick());
        }

        
        

        IEnumerator CoWaitClick() {
            isWait = true;
            clickTime = 0f;
            while (true) {
                yield return null;
                clickTime += Time.deltaTime;

                if (Input.GetMouseButtonDown(0)) {
                    var clickable = GetClickables(InputEx.GetMouseWorld()).FindAll(x => x.isDouble).PeekMax(x => x.priority);
                    if (clickable == null) break;
                    OnDoubleClickAction?.Invoke(clickable);
                    break;
                }

                if (clickTime > doubleInterval) {
                    var clickable = GetClickables(InputEx.GetMouseWorld()).FindAll(x => x.isOnce).PeekMax(x => x.priority);
                    if (clickable == null) break;
                    OnOnceClickAction?.Invoke(clickable);
                    break;
                }
            }
            isWait = false;
        }

        private List<Clickable> GetClickables(Vector3 pos) {
            var hits = Physics2D.RaycastAll(pos, Vector2.zero);
            return hits.Where(x => x.collider.gameObject.HasComponent<Clickable>()).ToList().ConvertAll(x => x.collider.gameObject.GetComponent<Clickable>());
        }
    }
}
