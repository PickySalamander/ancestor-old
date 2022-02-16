using System;
using Potterblatt.Storage;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class BirthIndex : GamePage {
		private VisualElement rowTemplate;

		private Label stateLabel;

		private void Awake() {
			stateLabel = RootElement.Q<Label>("state-label");
			
			Debug.Log("Here 1 " + stateLabel);
		}

		public void Setup(Person person, string state) {
			Debug.Log("Here 2 " + stateLabel);
			
			stateLabel.text = $"{state} State Board of Health".ToUpper();
		}
	}
}