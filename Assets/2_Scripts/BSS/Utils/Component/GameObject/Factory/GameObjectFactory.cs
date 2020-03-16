using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BSS.Extension;

namespace BSS {
    public class GameObjectFactory : MonoBehaviour {
        public enum StartPoint {
            LeftTop,LeftBottom,RightTop,RightBottom
        }
        public enum OrderType {
            WidthFirst,HeightFirst
        }
        public StartPoint startPoint;
        public OrderType orderType;
        public int width = 1;
        public int height = 1;
        public Vector2 initPosition;
        public Vector2 spacing;
        public bool isIndexer2;

        private GameObject child;

        [Button(ButtonSizes.Medium)]
        public void CreateChilds() {
            var childTr = transform.GetChild(0);
            if (childTr == null) {
                Debug.Log("Child is Null");
                return;
            }
            child = childTr.gameObject;

            if (orderType == OrderType.WidthFirst) {
                for (int i = 0; i < height; i++) {
                    for (int j = 0; j < width; j++) {
                        if (i == 0 && j == 0) {
                            child.transform.position = GetPosition(j, i);
                            if (isIndexer2) {child.GetOrAddComponent<Indexer2>().Init(j, i); }
                            continue;
                        }
                        var copy = Instantiate(child, transform);
                        copy.transform.position = GetPosition(j, i);
                        if (isIndexer2) { copy.GetOrAddComponent<Indexer2>().Init(j, i); }
                    }
                }
            } else {
                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < height; j++) {
                        if (i == 0 && j == 0) {
                            child.transform.position = GetPosition(i, j);
                            if (isIndexer2) { child.GetOrAddComponent<Indexer2>().Init(i, j); }
                            continue;
                        }
                        var copy = Instantiate(child, transform);
                        copy.transform.position = GetPosition(i, j);
                        if (isIndexer2) { copy.GetOrAddComponent<Indexer2>().Init(i, j); }
                    }
                }
            }
        }

        [Button(ButtonSizes.Medium)]
        public void ClearChilds() {
            for (int i = transform.childCount-1; i >= 1; i--) {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }


        private Vector2 GetPosition(int x,int y) {
            var initPos = (Vector2)transform.position + initPosition;
            int xSign=1;
            int ySign=1;
            if (startPoint == StartPoint.RightTop || startPoint == StartPoint.RightBottom) {
                xSign = -1;
            }
            if (startPoint == StartPoint.LeftTop || startPoint == StartPoint.RightTop) {
                ySign = -1;
            }
            return initPos + new Vector2(spacing.x * x* xSign, spacing.y * y*ySign);
        }
    }
}
