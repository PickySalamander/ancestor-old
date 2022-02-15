using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public abstract class AncestorVisualElement : VisualElement {
		public AncestorVisualElement() {
			if(Application.isPlaying) {
				RegisterCallback<AttachToPanelEvent>(OnAttach);
			}
		}

		private void OnAttach(AttachToPanelEvent evt) {
			DoInit();
			UnregisterCallback<AttachToPanelEvent>(OnAttach);
		}

		protected abstract void DoInit();
	}
}