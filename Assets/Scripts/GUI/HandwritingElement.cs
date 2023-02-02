using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Element in the display that shows a clickable button on the page that is "handwritten". It will attempt to take
	/// the text in the button and scew it so it looks more natural.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public class HandwritingElement : Button {
		/// <summary>
		/// Create a new object
		/// </summary>
		public HandwritingElement() {
			//attach the callback (can be called in editor)
			if(Application.isPlaying) {
				RegisterCallback<AttachToPanelEvent>(OnSetup);
			}
		}

		/// <summary>When the button is attached to the screen start working on display</summary>
		private void OnSetup(AttachToPanelEvent evt) {
			var manager = UIManager.Instance;
			
			//generate an offset of the button in the range
			var posOffset = new Vector2(Random.Range(-manager.handwritingOffset.x, manager.handwritingOffset.x),
				Random.Range(-manager.handwritingOffset.y, manager.handwritingOffset.y));
			
			//generate a rotation in the range
			var rotOffset = Random.Range(-manager.handwritingRotationAmount, manager.handwritingRotationAmount);
				
			//rotate it
			style.rotate = new StyleRotate {
				value = new Rotate(new Angle(rotOffset))
			};

			//move it
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