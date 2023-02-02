using System;
using UnityEngine;

namespace Potterblatt.Storage.People {
	/// <summary>
	/// A discovery that can be made by the user
	/// </summary>
	[Serializable]
	public class Discovery {
		[Tooltip("The person to be discovered")]
		public Person person;

		[Tooltip("The type discovery to make")]
		public DiscoveryType type;
	}
}