using System.Collections;
using System.Collections.Generic;
using Potterblatt.GUI;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Potterblatt {
	public class GameManager : SingletonMonobehaviour<MonoBehaviour> {
		public InputActionReference quitBtn;

		private bool isExiting = false;

		private void Start() {
			quitBtn.action.Enable();
			quitBtn.action.performed += OnExit;
		}

		private void OnExit(InputAction.CallbackContext callbackContext) {
			if(isExiting) { return; }
			
			isExiting = true;
			
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