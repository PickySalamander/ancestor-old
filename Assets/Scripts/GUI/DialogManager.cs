using System;
using System.Collections.Generic;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class DialogManager : SingletonMonobehaviour<DialogManager> {
		public VisualTreeAsset dialogTemplate;

		private VisualElement current;
		private readonly List<VisualElement> pending = new();

		public Dialog ShowDialog(string title, string message, string ok = "Ok", string cancel = null) {
			var newTemplate = dialogTemplate.CloneTree();
			newTemplate.Q<Label>("title").text = title;
			newTemplate.Q<Label>("message").text = message;

			var okButton = newTemplate.Q<Button>("yes");
			okButton.text = ok;

			var cancelButton = newTemplate.Q<Button>("no");
			if(string.IsNullOrEmpty(cancel)) {
				cancelButton.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
			}

			if(current == null) {
				AddDialog(newTemplate);
			} else {
				pending.Add(newTemplate);
			}

			return new Dialog(okButton, 
				string.IsNullOrWhiteSpace(cancel) ? null : cancelButton, 
				() => OnClose(newTemplate));
		}

		private void OnClose(VisualElement element) {
			if(current == element) {
				current = null;
				element.RemoveFromHierarchy();

				if(pending.Count == 0) {
					UIManager.Instance.ModalRoot.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
				}
				else {
					var newDiag = pending[0];
					pending.RemoveAt(0);
					AddDialog(newDiag);
				}
			}
			else {
				pending.Remove(element);
			}
		}

		private void AddDialog(VisualElement element) {
			var modalRoot = UIManager.Instance.ModalRoot;
			modalRoot.style.display = new StyleEnum<DisplayStyle>(StyleKeyword.Undefined);

			modalRoot.Add(element);
			current = element;
		}

		[ContextMenu("Sample Dialog")]
		private void SampleDialog() {
			ShowDialog("Test Title", $"This is a test message {DateTime.Now}");
		}
	}
}