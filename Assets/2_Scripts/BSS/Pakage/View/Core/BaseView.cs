using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using BSS.Extension;
using System.Linq;

namespace BSS.UI {
	[RequireComponent (typeof(CanvasGroup))]
	public class BaseView : MonoBehaviour
	{
		[SerializeField]
		private string id;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string ID {
			get{ return id; }
			set{ id = value; }
		}

        [SerializeField]
        public Button closeButton;

        public AudioClip showSound;
        public TweenElement showTween;

        public AudioClip closeSound;
        public TweenElement closeTween;

        public event Action OnShowAction;
        public event Action OnCloseAction;

        public bool IsVisible { 
			get { 
				return canvasGroup.alpha == 1f; 
			} 
		}

		protected RectTransform rectTransform;
		protected CanvasGroup canvasGroup {
            get { return GetComponent<CanvasGroup>(); }
        }



        private void Awake ()
		{
			rectTransform = GetComponent<RectTransform> ();
            if (closeButton != null)  closeButton.Listen(Close);
            OnAwake();
		}

		protected virtual void OnAwake ()
		{
		}

		private void Start ()
		{
			OnStart ();
		}

		protected virtual void OnStart ()
		{
		}

		public virtual void Show ()
		{
            SoundSystem.PlayOnce (showSound, 1.0f);
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
            if (showTween == null || !showTween.isAlpha) {
                canvasGroup.alpha = 1f;
            } else {
                TweenUtility.SetTween(gameObject, showTween,1, () => {
                    if (canvasGroup != null) {
                        canvasGroup.alpha = 1f;
                    }
                });
            }

            if (OnShowAction != null) {
                OnShowAction.Invoke ();
			}
		}
        public void ShowByForce() {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

		public virtual void Close ()
		{
            SoundSystem.PlayOnce (closeSound, 1.0f);
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
            if (closeTween == null || !closeTween.isAlpha) {
                canvasGroup.alpha = 0f;
            } else {
                TweenUtility.SetTween(gameObject, closeTween,1, () => {
                    if (canvasGroup != null) {
                        canvasGroup.alpha = 0f;
                    }
                });
            }

            if (OnCloseAction != null) {
                OnCloseAction.Invoke ();
			}
		}
        public void CloseByForce() {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public virtual void Toggle ()
		{
			if (!IsVisible) {
				Show ();
			} else {
				Close ();
			}
		}

        public T NextView<T>(bool selfClose=false) where T : BaseView {
            var view = UISystem.Find<T>();
            view.Show();
            if (selfClose) {
                Close();
            }
            return view;
        }

		public virtual void Focus ()
		{
			rectTransform.SetAsLastSibling ();
		}
        
    }
}