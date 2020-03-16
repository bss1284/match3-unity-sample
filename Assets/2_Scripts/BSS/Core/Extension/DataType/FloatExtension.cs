using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BSS.Extension {
    public static class FloatExtension {
        public static bool IsPositive(this float val) {
            return val >= 0f;
        }
        public static bool IsNegative(this float val) {
            return val < 0f;
        }
        public static float Round(this float val,int num) {
            return Mathf.Round(val * Mathf.Pow(10f, num)) * Mathf.Pow(10f, -num);
        }
        public static TimeSpan ToTimeSpan(this float val) {
            return TimeSpan.FromSeconds(val);
        }
    }
}
