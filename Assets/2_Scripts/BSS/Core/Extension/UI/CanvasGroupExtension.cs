using UnityEngine;

namespace BSS.Extension {
    public static class CanvasGroupExtension 
    {
        public static bool IsOpen(this CanvasGroup canvasGroup) {
            return canvasGroup.alpha > 0.01f;
        }
        public static void Close(this CanvasGroup canvasGroup) {
            canvasGroup.alpha=0f;
            canvasGroup.blocksRaycasts=false;
            canvasGroup.interactable=false;
        }
        public static void Show(this CanvasGroup canvasGroup) {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        public static void Toggle(this CanvasGroup canvasGroup) {
            if (canvasGroup.IsOpen()) {
                canvasGroup.Close();
            } else {
                canvasGroup.Show();
            }
        }
        public static void Toggle(this CanvasGroup canvasGroup,bool forceOnOff) {
            if (forceOnOff) {
                canvasGroup.Show();
            } else {
                canvasGroup.Close();
            }
        }
    }

}