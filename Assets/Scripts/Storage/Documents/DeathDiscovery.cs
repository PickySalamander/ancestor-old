using System;
using Potterblatt.Storage.People;
using UnityEngine;

namespace Potterblatt.Storage.Documents {
	/// <summary>
	/// Discoveries that can be made on a death certificate
	/// </summary>
	[Serializable]
	public class DeathDiscovery : Discovery {
		[Tooltip("Name of the button on the page this discovery is for")]
		public string deathLabel;
	}
}