using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace BSS.Extension {
    public static class RandomExtension {
    }
    public static class RandomEx {
        public static int GetIndexWeighted(List<int> weightList) {
            int sum = 0;
            foreach (var it in weightList) {
                sum += it;
            }
            int r = Random.Range(0, sum);
            int checkVal = 0;
            for (int i = 0; i < weightList.Count; i++) {
                checkVal += weightList[i];
                if (r < checkVal) {
                    return i;
                }
            }
            return -1;
        }
        public static int GetIndexWeighted(List<float> weightList) {
            float sum = 0;
            foreach (var it in weightList) {
                sum += it;
            }
            float r = Random.Range(0, sum);
            float checkVal = 0;
            for (int i = 0; i < weightList.Count; i++) {
                checkVal += weightList[i];
                if (r < checkVal) {
                    return i;
                }
            }
            return -1;
        }
    }
}
