using UnityEngine;
using UnityEngine.UI;

namespace Potterblatt.GUI {
	/// <summary>
	/// Child of <see cref="MultiImageButton"/> that can be notified of state changes and also change its color.
	/// </summary>
	public class MultiImageButtonChild : MonoBehaviour {
		[Tooltip("The graphic's whose color changes")]
		public Graphic graphic;

		[Header("Colors")]
		[Tooltip("The color tint to change the graphic to")]
		public ColorBlock colors = ColorBlock.defaultColorBlock;
		
		private void Reset() {
			graphic = GetComponent<Graphic>();
		}
		
		/// <summary>
		/// Tween the color change
		/// </summary>
		/// <param name="targetColor">The color to change to</param>
		/// <param name="instant">Should the change be instant?</param>
		public void StartColorTween(Color targetColor, bool instant) {
			if (graphic == null)
				return;

			graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
		}
	}
}