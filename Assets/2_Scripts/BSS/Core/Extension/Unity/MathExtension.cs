namespace BSS.Extension {
    public static class MathExtension {
        public static bool IsBetween(int val, int min, int max,bool equal=true) {
            if (equal) {
                return min <= val && val <= max;
            } else {
                return min < val && val < max;
            }
        }
        public static bool IsBetween(float val, float min, float max, bool equal = true) {
            if (equal) {
                return (min < val || min.Equals(val)) && (val < max || max.Equals(val));
            } else {
                return min < val && val < max;
            }
        }
    }
    public static class MathEx {
        public static float Percent(int child, int parent) {
            return ((float)child / (float)parent);
        }
        public static float Percent100(int child, int parent) {
            return ((float)child / (float)parent) * 100f;
        }
    }
}