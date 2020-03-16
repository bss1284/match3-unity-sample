using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace BSS.Extension {
    public static class TaskExtensions {
        public static IEnumerator AsIEnumerator(this Task task) {
            while (!task.IsCompleted) {
                yield return null;
            }

            if (task.IsFaulted) {
                throw task.Exception;
            }
        }
    }
}