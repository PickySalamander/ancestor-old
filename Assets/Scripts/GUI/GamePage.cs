using System;
using Pixelplacement;
using UnityEngine;

namespace Potterblatt.GUI {
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasGroup))]
	public class GamePage : MonoBehaviour {
		private CanvasGroup canvasGroup;
		
		private void OnEnable() {
			
		}

		public PageManager Manager => PageManager.Instance;

		public CanvasGroup CanvasGroup {
			get {
				if(!canvasGroup) {
					canvasGroup = GetComponent<CanvasGroup>();
				}
				
				return canvasGroup;
			}
		}
	}
}