using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	[RequireComponent(typeof(UIDocument))]
	public abstract class GamePage : MonoBehaviour {
		private UIDocument uiDocument;
		
		public UIDocument UIDocument {
			get {
				if(!uiDocument) {
					uiDocument = GetComponent<UIDocument>();
				}

				return uiDocument;
			}
		}

		public VisualElement RootElement => UIDocument.rootVisualElement;
	}
}