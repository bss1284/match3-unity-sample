using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BSS;
using BSS.Extension;
using UnityEditor;

namespace BSS {
    /// <summary>
    /// 단축키 설정
    /// </summary>
    public class HotKeyEditor : MonoBehaviour  {
        /// <summary>
        /// Alt+n을 누르면 게임오브젝트를 생성합니다. 
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Create GameObject &n")]
        private static void CreateGameObject() {
            var obj = new GameObject();
            Undo.RegisterCreatedObjectUndo(obj, "Create GameObject");
            if (Selection.activeGameObject == null) return;
            if (IsPrefab(Selection.activeGameObject)) return;
            obj.transform.SetParent(Selection.activeGameObject.transform);
            if (obj.GetComponentInParent<Canvas>() != null) {
                var rect=obj.AddComponent<RectTransform>();
                obj.transform.localScale = Vector3.one;
                rect.anchoredPosition = Vector2.zero;
            }
            Selection.activeGameObject = obj;
        }


        /// <summary>
        /// Alt+q을 누르면 게임오브젝트를 삭제합니다.
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Delete GameObject &q")]
        private static void DeleteSelectObject() {
            if (Selection.activeGameObject == null) return;
            if (IsPrefab(Selection.activeGameObject)) return;
            var parent = Selection.activeGameObject.transform.parent;
            foreach (var it in Selection.gameObjects) {
                if (it == null) continue;
                if (IsPrefab(it)) continue;
                Undo.DestroyObjectImmediate(it);
            }
            if (parent != null) {
                Selection.activeGameObject = parent.gameObject;
            }
        }
        /// <summary>
        /// Alt+p을 누르면 게임오브젝트의 부모를 선택합니다.
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Select Parent &p")]
        private static void SelectParent () {
            if (Selection.activeGameObject == null) return;
            var parent=Selection.activeGameObject.transform.parent;
            if (parent == null) return;
            Selection.activeGameObject = parent.gameObject;
        }

        /// <summary>
        /// Alt+i을 누르면 이미지 컴포넌트를 추가합니다.
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Add Image Component &i")]
        private static void AddImageComponent() {
            if (Selection.activeGameObject == null) return;
            if (IsPrefab(Selection.activeGameObject)) return;
            if (Selection.activeGameObject.GetComponentInParent<Canvas>() == null) return;
            if (Selection.activeGameObject.GetComponent<Text>() != null) return;
            if (Selection.activeGameObject.GetComponent<Image>() == null) {
                Selection.activeGameObject.AddComponent<Image>();
            } else {
                GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<Image>());
            }
        }
        /// <summary>
        /// Alt+i을 누르면 텍스트 컴포넌트를 추가합니다.
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Add Text Component &t")]
        private static void AddTextComponent() {
            if (Selection.activeGameObject == null) return;
            if (IsPrefab(Selection.activeGameObject)) return;
            if (Selection.activeGameObject.GetComponentInParent<Canvas>() == null) return;
            if (Selection.activeGameObject.GetComponent<Image>() != null) return;
            if (Selection.activeGameObject.GetComponent<Text>() == null) {
                var newComp=Selection.activeGameObject.AddComponent<Text>();
                newComp.alignment = TextAnchor.MiddleCenter;
            } else {
                GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<Text>());
            }
        }

        /// <summary>
        /// Alt+b을 누르면 버튼 컴포넌트를 추가합니다.
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Add Button Component &b")]
        private static void AddButtonComponent() {
            if (Selection.activeGameObject == null) return;
            if (IsPrefab(Selection.activeGameObject)) return;
            if (Selection.activeGameObject.GetComponentInParent<Canvas>() == null) return;
            if (Selection.activeGameObject.GetComponent<Image>() == null) {
                AddImageComponent();
            }
            if (Selection.activeGameObject.GetComponent<Button>() == null) {
                Selection.activeGameObject.AddComponent<Button>();
            } else {
                GameObject.DestroyImmediate(Selection.activeGameObject.GetComponent<Button>());
            }
        }

        /// <summary>
        /// Alt+f을 누르면 RectTransform을 풀사이즈로 변경합니다.
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Rect Full Size &f")]
        private static void RectSetFullSize() {
            if (Selection.activeGameObject == null) return;
            if (IsPrefab(Selection.activeGameObject)) return;
            var rect = Selection.activeGameObject.GetComponent<RectTransform>();
            if (rect==null) return;
            rect.SetAnchorsStretch(0f, 1f);
            rect.SetOffset(0f, 0f, 0f, 0f);
        }
        /// <summary>
        /// Alt+c을 누르면 RectTransform의 앵커를 가운데로 변경합니다.
        /// </summary>
        [MenuItem("Tools/BSS/HotKey/Rect Center Position &c")]
        private static void RectSetCenterPostion() {
            if (Selection.activeGameObject == null) return;
            if (IsPrefab(Selection.activeGameObject)) return;
            var rect = Selection.activeGameObject.GetComponent<RectTransform>();
            if (rect == null) return;
            rect.anchoredPosition = Vector2.zero;
        }



        private static bool IsPrefab(GameObject obj) {
#if UNITY_2019_1_OR_NEWER
            var prefabType = PrefabUtility.GetPrefabAssetType(obj);
            if (prefabType != PrefabAssetType.NotAPrefab) return true;
            return false;
#else
            var prefabType = PrefabUtility.GetPrefabType(obj);
            if (prefabType == PrefabType.Prefab) return true;
            return false;
#endif
        }

    }
}
