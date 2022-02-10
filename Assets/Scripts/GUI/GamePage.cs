using System;
using Pixelplacement;
using UnityEngine;

namespace Potterblatt.GUI {
	[RequireComponent(typeof(RectTransform))]
	public class GamePage : MonoBehaviour {
		private void OnEnable() {
			
		}

		public void Show() {
			
			//TODO just come off right side of page and transition the other off the left
			var rectTransform = (RectTransform) transform;

			Debug.Log(PageManager.Instance.canvas.pixelRect);

			var canvasRect = ((RectTransform) Manager.canvas.transform).rect;
			var startlocation = new Vector3(canvasRect.width / 2 + Manager.newPageLocation.x,
				-canvasRect.height / 2 + Manager.newPageLocation.y);
			
			Debug.Log(startlocation);

			Tween.LocalPosition(rectTransform, startlocation, 
				rectTransform.localPosition, 1, 1f);
		}

		public void Hide() {
			
		}

		public PageManager Manager => PageManager.Instance;
	}
}