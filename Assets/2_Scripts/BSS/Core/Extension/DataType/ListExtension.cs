using System.Collections.Generic;
using System;
using System.Linq;


namespace BSS.Extension {
    public static class ListExtension {
        public static bool IsAllow<T>(this IList<T> list, int index) {
            if (index < 0) return false;
            return list.Count > index;
        }
        public static bool AddUnique<T>(this IList<T> list, T item) {
            if (list.Contains(item)) {
                return false;
            }
            list.Add(item);
            return true;
        }
        public static T RandomPeek<T>(this IList<T> list) {
            if (list.Count == 0) return default(T);
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        public static T RandomPop<T>(this IList<T> list) {
            if (list.Count == 0) return default(T);
            T item = list[UnityEngine.Random.Range(0, list.Count)];
            list.Remove(item);
            return item;
        }
        public static T Pop<T>(this IList<T> list) {
            if (list.Count == 0) return default(T);
            T item = list[list.Count-1];
            list.Remove(item);
            return item;
        }
        public static T Peek<T>(this IList<T> list) {
            if (list.Count == 0) return default(T);
            return list[list.Count - 1];
        }
        public static T PeekMax<T>(this IList<T> list,Converter<T,int> converter) {
            if (list.Count == 0) return default(T);
            int maxValue = Int32.MinValue;
            int index = -1;
            for (int i = 0; i < list.Count; i++) {
                int value = converter(list[i]);
                if (maxValue < value) {
                    maxValue = value;
                    index = i;
                }
            }
            if (index<0) return default(T);
            return list[index];
        }
        public static T PeekMin<T>(this IList<T> list, Converter<T, int> converter) {
            if (list.Count == 0) return default(T);
            int minValue = Int32.MaxValue;
            int index = -1;
            for (int i = 0; i < list.Count; i++) {
                int value = converter(list[i]);
                if (minValue > value) {
                    minValue = value;
                    index = i;
                }
            }
            if (index < 0) return default(T);
            return list[index];
        }
        public static int Index<T>(this List<T> list,T item) {
            return list.FindIndex(x => x.Equals(item));
        }
        public static int NextIndex<T>(this List<T> list, T item) {
            int index=list.Index(item);
            if (index == -1) return -1;
            if (index == list.Count - 1) return 0;
            return index + 1;
        }

        public static IList<T> Shuffle<T>(this IList<T> list) {
            for (int i = 0; i < list.Count; i++) {
                T temp = list[i];
                int randomIndex = UnityEngine.Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
            return list;
        }
        public static void ExtractAdd<T>(this IList<T> list, List<T> otherList,params int[] indexes) {
            foreach (var index in indexes) {
                list.Add(otherList[index]);
            }
        }

        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB) {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }
    }
}
