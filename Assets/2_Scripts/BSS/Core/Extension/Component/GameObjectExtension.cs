using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace BSS.Extension {
    public static class GameObjectExtension {
        public static bool HasComponent<T>(this GameObject obj) where T : Component {
            return obj.GetComponent<T>() != null;
        }
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }
        public static Component CopyComponent(this GameObject destination, Component original) {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            for (int x = 0; x < fields.Length; x++) {
                System.Reflection.FieldInfo field = fields[x];

                if (field.IsDefined(typeof(SerializeField), false)) {
                    field.SetValue(copy, field.GetValue(original));
                }
            }
            return copy;
        }

        public static void SetLayer(this GameObject obj, int layer, bool includeChildren = true) {
            obj.layer = layer;
            if (includeChildren) {
                var children = obj.transform.GetComponentsInChildren<Transform>(true);
                for (var c = 0; c < children.Length; ++c) {
                    children[c].gameObject.layer = layer;
                }
            }
        }

        public static void DestroyAll(this IList<GameObject> objs){
            for (int i = objs.Count - 1; i >= 0; i--) {
                GameObject.Destroy(objs[i]);
            }
        }
    }

    public static class GameObjectEx {
        public static T Create<T>(string name) where T : Component {
            var type = typeof(T);
            var result = new GameObject(name, type).GetComponent<T>();
            return result;
        }
        public static T Create<T>(string name,Vector3 position) where T : Component {
            var result = Create<T>(name);
            result.transform.position = position;
            return result;
        }
        public static T Create<T>(string name,Transform parent, Vector3 localPosition) where T : Component {
            var result = Create<T>(name);
            result.transform.SetParent(parent, false);
            result.transform.localPosition = localPosition;
            return result;
        }
    }
}
