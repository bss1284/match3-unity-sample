using UnityEngine;

namespace BSS.Extension {
    public static class ColorExtension {
        public static string ToHex(this Color color) {
            return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(color.r), ToByte(color.g), ToByte(color.b));
        }
        public static Color SetAlpha(this Color color, float a) {
            return new Color(color.r, color.g, color.b, a);
        }

        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
    }
}
