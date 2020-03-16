using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace BSS {
	public class HoldTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		[SerializeField]
		public float holdTime = 1f;

        public UnityEvent onHolded = new UnityEvent();
        public UnityEvent onCanceled = new UnityEvent();

        private Coroutine coroutine;

        public HoldTrigger Init(float holdTime,UnityAction holdAct) {
            onHolded.AddListener(holdAct);
            return this;
        }

        public void OnPointerDown(PointerEventData eventData)
		{
            StopAllCoroutines();
            coroutine = StartCoroutine(coWaitLongPressed());
		}

        public void OnPointerUp(PointerEventData eventData)
		{
            if (coroutine!=null) {
                onCanceled.Invoke();
                StopAllCoroutines();
                coroutine = null;
            }
		}

        public void OnPointerExit(PointerEventData eventData)
		{
            if (coroutine != null) {
                onCanceled.Invoke();
                StopAllCoroutines();
                coroutine = null;
            }
        }


        IEnumerator coWaitLongPressed() {
            yield return new WaitForSeconds(holdTime);
            onHolded.Invoke();
            coroutine = null;
        }
        
	}
}

