using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BSS {
    public class ObservableData<T> {
        public T data { get; private set; }
        public event Action<T> OnChanged;
        public void Change(T _data) {
            data = _data;
            OnChanged?.Invoke(data);
        }
    }
}
