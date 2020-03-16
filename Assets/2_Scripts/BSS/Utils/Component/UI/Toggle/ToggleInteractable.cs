using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BSS {
	[RequireComponent(typeof(Toggle))]
	public class ToggleInteractable : MonoBehaviour
	{
		public ToggleGroup toggles;
		private Toggle toggle;
		// Use this for initialization
		void Awake ()
		{
			toggle = GetComponent<Toggle> ();
		}
		
		public void reverseInteractable(bool _bool) {
			toggle.interactable = !_bool;
		}
		public void groupInteractable(bool _bool) {
			foreach (var it in toggles.ActiveToggles()) {
				it.interactable = _bool;
			}
		}
	}
}

