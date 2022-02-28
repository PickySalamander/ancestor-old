using System;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class Dialog {
		public readonly Button ok;
		public readonly Button cancel;
		private readonly Action onClose;
		
		public event Action<bool> result;

		public Dialog(Button ok, Button cancel, Action onClose) {
			this.ok = ok;
			this.cancel = cancel;

			if(ok != null) {
				ok.clicked += () => OnClick(true);
			}

			if(cancel != null) {
				cancel.clicked += () => OnClick(false);
			}

			this.onClose = onClose;
		}

		private void OnClick(bool isOk) {
			onClose?.Invoke();
			result?.Invoke(isOk);
		}

		private void Close() {
			onClose?.Invoke();
		}
	}
}