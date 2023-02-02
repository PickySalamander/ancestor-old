using System;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Controller for a popup dialog on the page
	/// </summary>
	public class Dialog {
		/// <summary>Callback when the dialog is closed</summary>
		private readonly Action onClose;

		/// <summary>Callback for which button is pressed</summary>
		public event Action<bool> result;

		/// <summary>
		/// Construct with the starting buttons and the callback
		/// </summary>
		/// <param name="ok">The yes or ok button</param>
		/// <param name="cancel">The close or cancel button</param>
		/// <param name="onClose">Callback when the dialog is closed</param>
		public Dialog(Button ok, Button cancel, Action onClose) {
			if(ok != null) {
				ok.clicked += () => OnClick(true);
			}

			if(cancel != null) {
				cancel.clicked += () => OnClick(false);
			}

			this.onClose = onClose;
		}

		/// <summary>
		/// Called when a button is clicked
		/// </summary>
		/// <param name="isOk">if the button was the ok button</param>
		private void OnClick(bool isOk) {
			onClose?.Invoke();
			result?.Invoke(isOk);
		}
	}
}