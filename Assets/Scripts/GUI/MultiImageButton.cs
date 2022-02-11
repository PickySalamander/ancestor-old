using UnityEngine;
using UnityEngine.UI;

namespace Potterblatt.GUI {
	/// <summary>
	/// A <see cref="Button"/> that works with <see cref="MultiImageButtonChild"/> to change other graphics instead of
	/// just one. Based off
	/// <a href="https://forum.unity.com/threads/tint-multiple-targets-with-single-button.279820/">this</a>.
	/// </summary>
	public class MultiImageButton : Button {
		[Tooltip("Children to change as well (only works for color tint)")]
		public MultiImageButtonChild[] children;
		
		protected override void DoStateTransition(SelectionState state, bool instant) {
			//do the changes for the default button
			base.DoStateTransition(state, instant);

			//go through each child
			if(children != null && children.Length > 0) {
				foreach(var other in children) {
					//skip nulls
					if(!other) { continue; }
					
					//get the new tint on the child
					var tintColor = state switch {
						SelectionState.Normal => other.colors.normalColor,
						SelectionState.Highlighted => other.colors.highlightedColor,
						SelectionState.Pressed => other.colors.pressedColor,
						SelectionState.Selected => other.colors.selectedColor,
						SelectionState.Disabled => other.colors.disabledColor,
						_ => Color.black
					};

					//start its tween
					other.StartColorTween(tintColor, instant);
				}
			}
		}
	}
}