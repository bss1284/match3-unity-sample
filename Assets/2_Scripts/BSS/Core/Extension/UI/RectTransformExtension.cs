using UnityEngine;
using UnityEngine.UI;

namespace BSS.Extension {
    public static class RectTransformExtension {
        public static bool IsStretch(this RectTransform rect) {
            return !(rect.anchorMin.x.Equals(rect.anchorMax.x) && rect.anchorMin.y.Equals(rect.anchorMax.y));
        }
        public static RectTransform SetFullSize(this RectTransform rect) {
            rect.sizeDelta = new Vector2(0.0f, 0.0f);
            rect.anchorMin = new Vector2(0.0f, 0.0f);
            rect.anchorMax = new Vector2(1.0f, 1.0f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return rect;
        }
        public static RectTransform SetSize(this RectTransform rect, Vector2 newSize) {
            var pivot = rect.pivot;
            var dist = newSize - rect.rect.size;
            rect.offsetMin = rect.offsetMin - new Vector2(dist.x * pivot.x, dist.y * pivot.y);
            rect.offsetMax = rect.offsetMax + new Vector2(dist.x * (1f - pivot.x), dist.y * (1f - pivot.y));
            return rect;
        }
        public static void SetAnchorsFix(this RectTransform rect,float x,float y) {
            rect.anchorMin = new Vector2(x, y);
            rect.anchorMax = new Vector2(x, y);
        }
        public static void SetAnchorsStretch(this RectTransform rect,float min,float max) {
            rect.anchorMin = new Vector2(min, min);
            rect.anchorMax = new Vector2(max, max);
        }
        public static Rect GetScreenRect(this RectTransform self) {
            var rect = new Rect();
            var canvas = self.GetComponentInParent<Canvas>();
            var camera = canvas.worldCamera;
            if (camera != null) {
                var corners = new Vector3[4];
                self.GetWorldCorners(corners);
                rect.min = camera.WorldToScreenPoint(corners[0]);
                rect.max = camera.WorldToScreenPoint(corners[2]);
            }
            return rect;
        }
        public static Rect GetWorldRect(this RectTransform self) {
            var rect = new Rect();
            var corners = new Vector3[4];
            self.GetWorldCorners(corners);
            rect.min = corners[0];
            rect.max = corners[2];
            return rect;
        }

        //Only Stretch Mode
        public static void SetOffset(this RectTransform rect, float left, float right, float top, float bottom) {
            if (!rect.IsStretch()) return;
            rect.offsetMin = new Vector2(left, top);
            rect.offsetMax = new Vector2(-right, -bottom);
        }
        public static void AddOffset(this RectTransform rect, float left, float right, float top, float bottom) {
            if (!rect.IsStretch()) return;
            rect.offsetMin = new Vector2(left, top);
            rect.offsetMax = new Vector2(-right, -bottom);
        }
        //Only Fix Mode
        public static void SetPosition(this RectTransform rect, Vector2 viewPort) {
            if (rect.IsStretch()) return;
            var parent = rect.parent.GetComponent<RectTransform>();
            Vector2 parentSize = parent.rect.size;
            rect.anchoredPosition = parentSize * (viewPort - rect.anchorMin );
        }
        //Only Fix Mode
        public static Vector2 GetViewport(this RectTransform rect) {
            var parent = rect.parent.GetComponent<RectTransform>();
            Vector2 parentSize = parent.rect.size;
            return (rect.anchoredPosition/parentSize)+rect.anchorMin;
        }
    }
    public static class RectTransformEx {
        public static RectTransform Create(string name,RectTransform parent) {
            var obj = new GameObject(name);
            var rect= obj.AddComponent<RectTransform>();
            rect.SetParent(parent,false);
            return rect;
        }

    }
}
