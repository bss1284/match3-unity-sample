using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using BSS.Extension;

namespace BSS {
    public class LineDrawer : Singleton<LineDrawer> {
        public bool isWorldSpace = true;
        public int poolCount = 5;
        public LineRenderer rendererPrefab;

        private List<LineRenderer> lineRenderers = new List<LineRenderer>();


        private void Awake() {
            for (int i = 0; i < poolCount; i++) {
                var render = Instantiate(rendererPrefab, transform);
                render.useWorldSpace = isWorldSpace;
                lineRenderers.Add(render);
                render.gameObject.SetActive(false);
            }
        }

        public static int Draw(List<Vector3> lines) {
            var index = instance.lineRenderers.FindIndex(x => !x.gameObject.activeSelf);
            var render = instance.lineRenderers[index];
            render.gameObject.SetActive(true);
            render.positionCount = lines.Count;
            render.SetPositions(lines.ToArray());
            return index;
        }
        public static int Draw(List<Vector2> lines) {
            return Draw(lines.ConvertAll(x => (Vector3)x));
        }

        public static void Erase(int index) {
            var render = instance.lineRenderers[index];
            render.gameObject.SetActive(false);
        }
        public static void Clear() {
            instance.lineRenderers.ForEach(x => x.gameObject.SetActive(false));
        }
    }
}
