using UnityEngine;
using System.Linq;

namespace BSS {
    /// <summary>
    /// 스프라이트 렌더러 애니메이터
    /// </summary>
    [AddComponentMenu("Spritedow/Sprite Animator")]
    [RequireComponent(typeof(SpriteRenderer))]
    [DisallowMultipleComponent]
    public class SpriteAnimator : BaseAnimator
    {
        public SpriteRenderer spriteRenderer {
            get { return GetComponent<SpriteRenderer>(); }
        }
        public int sortingOrder {
            get { return GetComponent<SpriteRenderer>().sortingOrder; }
            set { GetComponent<SpriteRenderer>().sortingOrder = value; }
        }
        public string sortingLayerName {
            get { return GetComponent<SpriteRenderer>().sortingLayerName; }
            set { GetComponent<SpriteRenderer>().sortingLayerName = value; }
        }
        public int sortingLayerID {
            get { return GetComponent<SpriteRenderer>().sortingLayerID; }
            set { GetComponent<SpriteRenderer>().sortingLayerID = value; }
        }



        protected override void ChangeFrame(Sprite frame)
        {
            spriteRenderer.sprite = frame;
        }


        

    }
}