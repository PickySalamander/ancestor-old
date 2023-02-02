using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Base class for <see cref="UnityEngine.UIElements.UIDocument"/>'s
	/// </summary>
	[RequireComponent(typeof(UIDocument))]
	public abstract class GamePage : MonoBehaviour {
		/// <summary>The <see cref="UnityEngine.UIElements.UIDocument"/> attached</summary>
		private UIDocument uiDocument;

		/// <summary>The <see cref="UnityEngine.UIElements.UIDocument"/> attached</summary>
		public UIDocument UIDocument {
			get {
				if(!uiDocument) {
					uiDocument = GetComponent<UIDocument>();
				}

				return uiDocument;
			}
		}

		/// <summary>
		/// The root visual element where the UI hierarchy starts in the <see cref="UnityEngine.UIElements.UIDocument"/>
		/// </summary>
		public VisualElement RootElement => UIDocument.rootVisualElement;
	}
}