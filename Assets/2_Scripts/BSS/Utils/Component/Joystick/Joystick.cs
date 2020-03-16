using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BSS {
    [RequireComponent(typeof(RectTransform))]
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
        public RectTransform bg;
        public RectTransform handle;
        public bool isDirectReturn;
        public Vector2 autoReturnSpeed = new Vector2(4.0f, 4.0f);
        public float radius = 40.0f;

        public Action<Vector2> MovementAct;
        public Action EndAct;

        private bool _returnHandle;
        private RectTransform _canvas;
        private List<Image> images = new List<Image>();

        public Vector2 Coordinates {
            get {
                if (handle.anchoredPosition.magnitude < radius)
                    return handle.anchoredPosition / radius;
                return handle.anchoredPosition.normalized;
            }
        }

        public void ImageOn() {
            images.ForEach(image => image.enabled = true);
        }
        public void ImageOff() {
            images.ForEach(image => image.enabled = false);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
            _returnHandle = false;
            var handleOffset = GetJoystickOffset(eventData);
            handle.anchoredPosition = handleOffset;
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            var handleOffset = GetJoystickOffset(eventData);
            handle.anchoredPosition = handleOffset;
            if (MovementAct != null) {
                MovementAct.Invoke(Coordinates);
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
            _returnHandle = true;
            if (EndAct != null) {
                EndAct.Invoke();
            }
        }

        private Vector2 GetJoystickOffset(PointerEventData eventData) {
            Vector3 globalHandle;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, eventData.position, eventData.pressEventCamera, out globalHandle))
                handle.position = globalHandle;
            var handleOffset = handle.anchoredPosition;
            if (handleOffset.magnitude > radius) {
                handleOffset = handleOffset.normalized * radius;
                handle.anchoredPosition = handleOffset;
            }
            return handleOffset;
        }
        void Awake() {
            images = bg.GetComponentsInChildren<Image>().ToList();
        }

        private void Start() {
            
            _returnHandle = true;
            var touchZone = GetComponent<RectTransform>();
            touchZone.pivot = Vector2.one * 0.5f;
            handle.transform.SetParent(transform);
            var curTransform = transform;
            do {
                if (curTransform.GetComponent<Canvas>() != null) {
                    _canvas = curTransform.GetComponent<RectTransform>();
                    break;
                }
                curTransform = transform.parent;
            }
            while (curTransform != null);
        }

        private void Update() {
            if (_returnHandle)
                if (isDirectReturn) {
                    handle.anchoredPosition = handle.anchoredPosition.normalized;
                    _returnHandle = false;
                    return;
                }
            if (handle.anchoredPosition.magnitude > Mathf.Epsilon)
                handle.anchoredPosition -= new Vector2(handle.anchoredPosition.x * autoReturnSpeed.x, handle.anchoredPosition.y * autoReturnSpeed.y) * Time.deltaTime;
            else
                _returnHandle = false;
        }
    }
}
