using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class HandwritingElement : Button {
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
				
			style.rotate = new StyleRotate {
				value = new Rotate(new Angle(rotOffset))
			};

			style.translate = new StyleTranslate {
				value = new Translate {
					x = new Length(posOffset.x, LengthUnit.Pixel),
					y = new Length(posOffset.y, LengthUnit.Pixel)
				}
			};
		}
		
		public new class UxmlFactory : UxmlFactory<HandwritingElement, Label.UxmlTraits> { }

		public new class UxmlTraits : TextElement.UxmlTraits { }
	}
}