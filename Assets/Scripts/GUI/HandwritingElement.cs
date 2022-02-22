using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class HandwritingElement : Label {
		public HandwritingElement() {
			if(Application.isPlaying) {
				RegisterCallback<AttachToPanelEvent>(OnSetup);
			}
		}

		private void OnSetup(AttachToPanelEvent evt) {
			var manager = UIManager.Instance;
			
			var posOffset = new Vector2(Random.Range(-manager.handwritingOffset.x, manager.handwritingOffset.x),
				Random.Range(-manager.handwritingOffset.y, manager.handwritingOffset.y));
			
			var rotOffset = Random.Range(-manager.handwritingRotationAmount, manager.handwritingRotationAmount);
				
			var innerLabel = this.Q<Label>();
			innerLabel.style.rotate = new StyleRotate {
				value = new Rotate(new Angle(rotOffset))
			};

			innerLabel.style.translate = new StyleTranslate {
				value = new Translate {
					x = new Length(posOffset.x, LengthUnit.Pixel),
					y = new Length(posOffset.y, LengthUnit.Pixel)
				}
			};
		}

		public new class UxmlFactory : UnityEngine.UIElements.UxmlFactory<HandwritingElement, Label.UxmlTraits> { }

		public new class UxmlTraits : TextElement.UxmlTraits { }
	}
}