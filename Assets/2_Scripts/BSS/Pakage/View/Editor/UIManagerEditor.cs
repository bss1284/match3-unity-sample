using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace BSS.UI {
	public class UIManagerEditor : EditorWindow
	{
		[MenuItem ("Tools/BSS/UI/Manager")]
		public static void ShowWindow ()
		{
			UIManagerEditor window = EditorWindow.GetWindow<UIManagerEditor> ("UI Manager");
			Vector2 size = new Vector2 (250f, 200f);
			window.minSize = size;
			window.wantsMouseMove = true;
		}

		[SerializeField]
		private string[] paths;
		[SerializeField]
		private BaseView[] targets;
		[SerializeField]
		private CanvasGroup[] canvasGroups;

		[SerializeField]
		private Vector2 scroll;
		[SerializeField]
		private int index = -1;

		private void OnEnable ()
		{
			FindWidgets ();

		}

		private void OnFocus ()
		{
			Repaint ();
		}

		private void OnGUI ()
		{
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			if (GUILayout.Button ("Refresh", EditorStyles.toolbarButton) || targets.Any (x => x == null)) {
				FindWidgets ();
				Focus ();
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();

			scroll = EditorGUILayout.BeginScrollView (scroll);
			for (int i = 0; i < targets.Length; i++) {
				GUIStyle style = new GUIStyle ("PopupCurveSwatchBackground") {
					padding = new RectOffset ()
				};
				if (i == index) {
					style = new GUIStyle ("MeTransitionSelectHead") {
						stretchHeight = false,

					};
					style.overflow = new RectOffset (-1, -2, -2, 2);
				}
				GUILayout.BeginVertical (style);
				GUILayout.BeginHorizontal ();
				GUILayout.Label (paths [i]);

				Rect elementRect = GUILayoutUtility.GetLastRect ();
				elementRect.width = Screen.width - 110f;
				Event ev = Event.current;
				switch (ev.rawType) {
				case EventType.MouseUp:
					if (elementRect.Contains (Event.current.mousePosition)) {
						if (Event.current.button == 0) {
							index = i;
							Selection.activeGameObject = targets [i].gameObject;
							Event.current.Use ();
						} 
					}
					break;
				}
				GUILayout.FlexibleSpace ();
				CanvasGroup canvasGroup = canvasGroups [i];
                if (canvasGroup.alpha > 0f) {
                    GUILayout.Label("[Show]", GUILayout.Width(80f));
                } else {
                    GUILayout.Label("[Hide]",GUILayout.Width(80f));
                }

                if (GUILayout.Button ("Show", GUILayout.Width (50f))) {
                    //Show
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }
				if (GUILayout.Button ("Hide", GUILayout.Width (50f))) {
						canvasGroup.alpha = 0f;
						canvasGroup.interactable = false;
						canvasGroup.blocksRaycasts = false;
				}
				GUILayout.EndHorizontal ();
				GUILayout.EndVertical ();

			}
			EditorGUILayout.EndScrollView ();
		}

        private int GetHierarchyIndexRecursive(Transform tr) {
            var power = 1;
            for (int i = 0; i < 4 -  tr.GetParentCount(); i++) {
                power = power * 10*10;
            }
            var root = tr.root;
            if (root == tr) {
                return tr.GetSiblingIndex()*power;
            }
            return GetHierarchyIndexRecursive(tr.parent)+ tr.GetSiblingIndex() * power; 
        }

		private void FindWidgets ()
		{
            var targetsList = FindInScene<BaseView>();
            targetsList.Sort((x, y) => GetHierarchyIndexRecursive(x.transform) - GetHierarchyIndexRecursive(y.transform));
            targets = targetsList.ToArray ();
            
			canvasGroups = targets.Select (x => x.gameObject.GetComponent<CanvasGroup> ()).ToArray ();
			List<string> widgetPaths = new List<string> ();
			for (int i = 0; i < targets.Length; i++) {
                string path = GetHierarchyPath(targets[i].transform);
                for (int j = 0; j < targets[i].transform.GetParentCount() - 1; j++) {
                    path = "-" + path;
                }
                widgetPaths.Add (path);
			}
			paths = widgetPaths.ToArray ();
		}

		public static string GetHierarchyPath (Transform transform)
		{
			string path = transform.name;
			while (transform.parent != null) {
				path = transform.parent.name + "/" + path;
				transform = transform.parent;
			}
			return path;
		}

		/// <summary>
		/// Finds components the in scene.
		/// </summary>
		/// <returns>The in scene.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> FindInScene<T> () where T : Component
		{
			T[] comps = Resources.FindObjectsOfTypeAll (typeof(T)) as T[];

			List<T> list = new List<T> ();

			foreach (T comp in comps) {
				if (comp.gameObject.hideFlags == 0) {
					string path = AssetDatabase.GetAssetPath (comp.gameObject);
					if (string.IsNullOrEmpty (path))
						list.Add (comp);
				}
			}
			return list;
		}
	}
}