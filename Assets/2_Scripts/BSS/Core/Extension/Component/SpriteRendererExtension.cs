using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSS.Extension {
    public static class SpriteRendererExtension  {
        public static void SetScaleLikeRect(this SpriteRenderer sprRenderer,RectTransform target, Vector2 scale) {
            if (sprRenderer.sprite == null) {
                Debug.Log("Sprite is Null!");
                sprRenderer.transform.localScale = scale;
                return;
            }
            var worldRect = target.GetWorldRect();
            sprRenderer.transform.localScale = (worldRect.size / sprRenderer.sprite.bounds.size) * scale;
        }
        public static void SetAlpha(this SpriteRenderer sprRenderer,float a) {
            sprRenderer.color = new Color(sprRenderer.color.r, sprRenderer.color.g, sprRenderer.color.b, a);
        }
    }
}
