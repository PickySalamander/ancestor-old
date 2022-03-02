using System;

namespace Potterblatt.Storage.People {
	[Serializable]
	public class Discovery {
		/// <summary>The person to be discovered</summary>
		public Person person;
		
		/// <summary>The type discovery to make</summary>
		public DiscoveryType type;
	}
}