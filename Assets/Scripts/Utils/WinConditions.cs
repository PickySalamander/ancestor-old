using System;
using System.Collections;
using Potterblatt.GUI;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using UnityEngine;

namespace Potterblatt.Utils {
	/// <summary>
	/// Controller for determining if the user discovered everything
	/// </summary>
	public class WinConditions : MonoBehaviour {
		[Tooltip("The discoveries that the user needs to make in order to win")]
		public Discovery[] discoveriesRequired;

		private void Start() {
			StartCoroutine(WaitForSaveState());
		}

		/// <summary>
		/// <see cref="Coroutine"/> for waiting for the save state then monitoring discoveries
		/// </summary>
		private IEnumerator WaitForSaveState() {
			yield return new WaitUntil(() => SaveState.IsSetup);

			//wait for discoveries
			SaveState.Instance.onDiscovery += OnNewDiscovery;
		}

		/// <summary>
		/// called when a new discovery is made
		/// </summary>
		/// <param name="person">The person</param>
		/// <param name="type">The discoveries made on the person</param>
		private void OnNewDiscovery(Person person, DiscoveryType type) {
			var saveState = SaveState.Instance;
			
			//check each required discovery and see if it was made yet
			foreach(var discovery in discoveriesRequired) {
				var discoveredAlready = saveState[discovery.person];
				var result = discoveredAlready & discovery.type;
				if(result != discovery.type) {
					return;
				}
			}
			
			//if here, the user won! tell them about it
			DialogManager.Instance.ShowDialog("All Done!", 
				"You discovered everything!\n\n" +
				"You can keep fooling around if you want, but there isn't anything left to discover. " +
				"Thanks for playing the demo, you've been a big help!");
			
			AnalyticsManager.Win();
		}
	}
}