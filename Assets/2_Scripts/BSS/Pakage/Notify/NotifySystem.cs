using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BSS {
    /// <summary>
    /// My event type.
    /// </summary>
    [System.Serializable]
    public class MyEvent : UnityEvent<GameObject, object> {

    }
    public class NotifySystem : Singleton<NotifySystem> {
        private Dictionary<string, MyEvent> eventDictionary = new Dictionary<string, MyEvent>();

        /// <summary>
        /// Start listening specified event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="listener">Listener.</param>
        public static void StartListening(string eventName, UnityAction<GameObject, object> listener) {
            MyEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new MyEvent();
                thisEvent.AddListener(listener);
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Stop listening specified event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="listener">Listener.</param>
        public static void StopListening(string eventName, UnityAction<GameObject, object> listener) {
            MyEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Trigger specified event.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="obj">Object.</param>
        /// <param name="param">Parameter.</param>
        public static void Trigger(string eventName, GameObject obj, object param) {
            MyEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.Invoke(obj, param);
            }
        }
    }
}
