using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSS {
    /// <summary>
    /// 원형 리스트 (안전한 인덱스 접근)
    /// </summary>
    /// Example) Count가 [10]인 리스트일 경우
    /// Example) 접근Index:-1=> 적용인덱스:9 ([10]-1)
    /// Example) 접근Index:15=> 적용인덱스:5 (15-[10])
    [Serializable]
    public class CircularList<T> : ICollection<T> , IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyList<T> {
        [SerializeField]
        private List<T> innerList;

        public CircularList() {
            innerList = new List<T>();
        }
        public CircularList(int capacity) {
            innerList = new List<T>(capacity);
        }
        public CircularList(IEnumerable<T> collection) {
            innerList = new List<T>(collection);
        }

        public T this[int index] {
            get => innerList[ValidIndex(index)];
            set => innerList[ValidIndex(index)] = value;
        }

        public int Count => innerList.Count;

        public bool IsReadOnly => false;


        public void Add(T item) {
            innerList.Add(item);
        }

        public void Clear() {
            innerList.Clear();
        }

        public bool Contains(T item) {
            return innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            innerList.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index) {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() {
            return innerList.GetEnumerator();
        }

        public int IndexOf(T item) {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, T item) {
            innerList.Insert(ValidIndex(index),item);
        }

        public bool Remove(T item) {
            return innerList.Remove(item);
        }

        public void RemoveAt(int index) {
            innerList.RemoveAt(ValidIndex(index));
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return innerList.GetEnumerator();
        }

        private int ValidIndex(int index) {
            if (Count == 0) return index;
            if (index < 0) {
                return ValidIndex(Count + index);
            }
            if (index >= Count) {
                return ValidIndex(index - Count);
            }
            return index;
        }
    }

    public static class CircularListExtension {
        public static CircularList<T> ToCircularList<T>(this IEnumerable<T> list) {
            return new CircularList<T>(list);
        }
    }
}