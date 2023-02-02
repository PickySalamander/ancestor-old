using System;
using System.Collections.Generic;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Singleton manager for handling opening popup dialogs.
	/// </summary>
	/// <seealso cref="Dialog"/>
	public class DialogManager : SingletonMonobehaviour<DialogManager> {
		/// <summary>Template for the dialog that is rendered</summary>
		public VisualTreeAsset dialogTemplate;

		/// <summary>Current dialog on the screen</summary>
		private VisualElement current;
		
		/// <summary>Dialog queue of dialogs waiting to be shown (if multiple)</summary>
		private readonly List<VisualElement> pending = new();

		/// <summary>
		/// Show a dialog on the page, by default the dialog will just show an "ok" button
		/// </summary>
		/// <param name="title">Title of the dialog</param>
		/// <param name="message">The message to show</param>
		/// <param name="ok">Yes or ok button to show</param>
		/// <param name="cancel">Optional cancel or no button to show</param>
		/// <returns>The dialog object to listen to for closure or success</returns>
		public Dialog ShowDialog(string title, string message, string ok = "Ok", string cancel = null) {
			//set the title and message
			var newTemplate = dialogTemplate.CloneTree();
			newTemplate.Q<Label>("title").text = title;
			newTemplate.Q<Label>("message").text = message;

			//set the ok button
			var okButton = newTemplate.Q<Button>("yes");
			okButton.text = ok;

			//set the cancel button if specified
			var cancelButton = newTemplate.Q<Button>("no");
			if(string.IsNullOrEmpty(cancel)) {
				cancelButton.style.visibility = new StyleEnum<Visibility>(Visibility.Hidden);
			}

			//add the dialog to the page or pend to the queue if there is something on the page
			if(current == null) {
				AddDialog(newTemplate);
			} else {
				pending.Add(newTemplate);
			}

			//return the dialog listener object
			return new Dialog(okButton, 
				string.IsNullOrWhiteSpace(cancel) ? null : cancelButton, 
				() => OnClose(newTemplate));
		}

		/// <summary>Called when the dialog is closed</summary>
		/// <param name="element">The rendered object to close</param>
		private void OnClose(VisualElement element) {
			//if this is the current display, remove it from the page
			if(current == element) {
				current = null;
				element.RemoveFromHierarchy();

				//if no pending hide the modal background
				if(pending.Count == 0) {
					UIManager.Instance.ModalRoot.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
				}
				
				//otherwise display the next in the queue
				else {
					var newDiag = pending[0];
					pending.RemoveAt(0);
					AddDialog(newDiag);
				}
			}
			
			//other wise remove from pending
			else {
				pending.Remove(element);
			}
		}

		/// <summary>
		/// Add a dialog's display to the page
		/// </summary>
		/// <param name="element">The visuals to add</param>
		private void AddDialog(VisualElement element) {
			//set the modal background to visible
			var modalRoot = UIManager.Instance.ModalRoot;
			modalRoot.style.display = new StyleEnum<DisplayStyle>(StyleKeyword.Undefined);

			//add this element to the modal and set the current
			modalRoot.Add(element);
			current = element;
		}

		/// <summary>
		/// Helper editor test script for testing the display.
		/// </summary>
		[ContextMenu("Sample Dialog")]
		private void SampleDialog() {
			ShowDialog("Test Title", $"This is a test message {DateTime.Now}");
		}
	}
}