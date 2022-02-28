using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Potterblatt.Storage.People {
	[CreateAssetMenu(fileName = "Person", menuName = "Ancestor/Create Person")]
	public class Person : ScriptableObject {
		public string uuid;
		public Person father;
		public Person mother;
		
		public string firstName;
		public string lastName;
		
		public LifeEvent[] timeLine;
		
		[SerializeField]
		private DiscoveryType defaultDiscovery = DiscoveryType.None;

		public DiscoveryType DefaultDiscovery => defaultDiscovery;

		private void Reset() {
			uuid ??= Guid.NewGuid().ToString();
		}
		
		public LifeEvent GetEventByType(LifeEventType type) {
			return timeLine.FirstOrDefault(lifeEvent => lifeEvent.type == type);
		}

		public string FullName => $"{firstName} {lastName}";

		public LifeEvent Born => GetEventByType(LifeEventType.Birth);
		
		public LifeEvent Death => GetEventByType(LifeEventType.Death);

		public bool IsReal => !string.IsNullOrWhiteSpace(uuid);
		
		public bool HasNoDiscoveries() {
			return SaveState.Instance[this] == DiscoveryType.None;
		}

		public bool IsDiscovered(DiscoveryType type) {
			return (SaveState.Instance[this] & type) != DiscoveryType.None;
		}
		
		public bool IsDiscovered(LifeEvent lifeEvent) {
			return IsDiscovered(lifeEvent.type);
		}
		
		public bool IsDiscovered(LifeEventType lifeEventType) {
			var save = SaveState.Instance[this];
			switch(lifeEventType) {
				case LifeEventType.Birth:
					return save.HasFlag(DiscoveryType.Birth);
				case LifeEventType.Death:
					return save.HasFlag(DiscoveryType.Death);
			}

			return false;
		}
	}
}