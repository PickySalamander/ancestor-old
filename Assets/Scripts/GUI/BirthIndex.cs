using Potterblatt.Storage;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class BirthIndex : AncestorVisualElement {
		private BirthIndexRow rowTemplate;

		private Label stateLabel;

		protected override void DoInit() {
			stateLabel = this.Q<Label>("state-label");
			
			Debug.Log("Here 1 " + stateLabel);
		}

		public void Setup(Person person, string state) {
			Debug.Log("Here 2 " + stateLabel);
			
			stateLabel.text = $"{state} State Board of Health".ToUpper();
		}

		public new class UxmlFactory : UxmlFactory<BirthIndex> {}
	}
}