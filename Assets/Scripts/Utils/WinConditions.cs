using System;
using System.Collections;
using Potterblatt.GUI;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using UnityEngine;

namespace Potterblatt.Utils {
	public class WinConditions : MonoBehaviour {
		public Discovery[] discoveriesRequired;

		private void Start() {
			StartCoroutine(WaitForSaveState());
		}

		private IEnumerator WaitForSaveState() {
			yield return new WaitUntil(() => SaveState.IsSetup);

			SaveState.Instance.onDiscovery += OnNewDiscovery;
		}

		private void OnNewDiscovery(Person person, DiscoveryType type) {
			var saveState = SaveState.Instance;
			
			foreach(var discovery in discoveriesRequired) {
				var discoveredAlready = saveState[discovery.person];
				var result = discoveredAlready & discovery.type;
				if(result != discovery.type) {
					return;
				}
			}
			
			DialogManager.Instance.ShowDialog("All Done!", 
				"You discovered everything!\n\n" +
				"You can keep fooling around if you want, but there isn't anything left to discover. " +
				"Thanks for playing the demo, you've been a big help!");
		}
	}
}