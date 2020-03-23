using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS.Extension;

namespace BSS {
    /// <summary>
    /// GameObject에 텍스트 컴포넌트를 등록하기위한 클래스
    /// </summary>
    public class WorldTextDrawer : MonoBehaviour {
        public Vector2 canvasScale=Vector2.one *0.01f;
        public Vector2 canvasSize = new Vector2(200f, 200f);
        public Vector2 relativePosition;
        public string sortingLayerName="Default";
        public int sortingOrder;

        public Font font;
        public int fontSize=10;
        public Color fontColor = Color.white;

        private Dictionary<GameObject, Canvas> canvasDics = new Dictionary<GameObject, Canvas>();


        public Canvas GetCanvas(GameObject obj) {
            return canvasDics[obj];
        }
        public Text GetText(GameObject obj) {
            return canvasDics[obj].transform.GetChild(0).GetComponent<Text>();
        }



        public void Regist(GameObject obj) {
            if (canvasDics.ContainsKey(obj)) {
                UnRegist(obj);
            }
            var canvas=GameObjectEx.Create<Canvas>("ReelText", obj.transform, relativePosition);
            canvas.transform.localScale = canvasScale;
            canvas.GetComponent<RectTransform>().SetSize(canvasSize);
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = sortingLayerName;
            canvas.sortingOrder = sortingOrder;
            var text = GameObjectEx.Create<Text>("Text", canvas.transform, Vector3.zero);
            text.font = font;
            text.fontSize = fontSize;
            text.color = fontColor;
            text.alignment = TextAnchor.MiddleCenter;
            text.GetComponent<RectTransform>().SetFullSize();
            text.raycastTarget = false;
            canvasDics[obj] = canvas;
        }

        public void UnRegist(GameObject obj) {
            if (!canvasDics.ContainsKey(obj)) return;
            if (canvasDics[obj] != null) {
                Destroy(canvasDics[obj].gameObject);
            }
            canvasDics.Remove(obj);
        }
    }
}
