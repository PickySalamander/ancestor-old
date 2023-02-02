using Potterblatt.GUI;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Potterblatt {
	/// <summary>
	/// Manager of the whole game!
	/// </summary>
	public class GameManager : SingletonMonobehaviour<MonoBehaviour> {
		[Tooltip("The exit button action")]
		public InputActionReference quitBtn;

		/// <summary>Is the app currently exiting?</summary>
		private bool isExiting;

		private void Start() {
			//enable the quit button
			quitBtn.action.Enable();
			quitBtn.action.performed += OnExit;
		}

		/// <summary>
		/// Called when the user pushed the exit button
		/// </summary>
		/// <param name="callbackContext"></param>
		private void OnExit(InputAction.CallbackContext callbackContext) {
			//only exit once
			if(isExiting) {
				return;
			}

			isExiting = true;

			//ask the user if they want to exit and then do it if they want
			DialogManager.Instance.ShowDialog("Quit?",
					"Are you sure you want to quit?", "Yes", "No").result +=
				result => {
					isExiting = false;

					if(result) {
#if UNITY_EDITOR
						EditorApplication.isPlaying = false;
#else
						Application.Quit();
#endif
					}
				};
		}
	}
}