using UnityEngine;

namespace BSS.Extension {
    public static class InputExtension {
        
    }
    public static class InputEx {
        public static Vector2 GetMouseWorld() {
            var mp = Input.mousePosition;
            return  Camera.main.ScreenToWorldPoint(mp);
        }
        public static Vector2 GetMouseScreen() {
            return Input.mousePosition;
        }
        public static Vector2 GetMouseView() {
            var mp = Input.mousePosition;
            return Camera.main.ScreenToViewportPoint(mp);
        }
        public static int GetKeyDown() {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                return 0;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                return 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                return 2;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                return 3;
            }
            return -1;
        }
    }
}
