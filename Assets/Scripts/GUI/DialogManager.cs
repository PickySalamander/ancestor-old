using System;
using System.Collections.Generic;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class DialogManager : SingletonMonobehaviour<DialogManager> {
		public VisualTreeAsset dialogTemplate;

		private List<VisualElement> dialogs = new();

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

			AddDialog(newTemplate);

			return new Dialog(okButton, 
				string.IsNullOrWhiteSpace(cancel) ? null : cancelButton, 
				() => OnClose(newTemplate));
		}

		private void OnClose(VisualElement element) {
			dialogs.Remove(element);
			element.RemoveFromHierarchy();

			if(dialogs.Count == 0) { 
				UIManager.Instance.ModalRoot.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
			}
		}

		private void AddDialog(VisualElement element) {
			var modalRoot = UIManager.Instance.ModalRoot;

			if(dialogs.Count == 0) {
				modalRoot.style.display = new StyleEnum<DisplayStyle>(StyleKeyword.Undefined);
			}
			
			modalRoot.Add(element);
			dialogs.Add(element);
		}

		[ContextMenu("Sample Dialog")]
		private void SampleDialog() {
			ShowDialog("Test Title", "This is a test message", "Ok");
		}
	}
}