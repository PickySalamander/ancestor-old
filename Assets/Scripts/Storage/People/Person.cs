using System.Linq;
using UnityEngine;

namespace Potterblatt.Storage.People {
	[CreateAssetMenu(fileName = "Person", menuName = "Ancestor/Create Person")]
	public class Person : ScriptableObject {
		public Person father;
		public Person mother;
		
		public string firstName;
		public string lastName;
		
		public LifeEvent[] timeLine;
		
		public DiscoveryType discovered = DiscoveryType.None;
		
		public LifeEvent GetEventByType(LifeEventType type) {
			return timeLine.FirstOrDefault(lifeEvent => lifeEvent.type == type);
		}

		public string FullName => $"{firstName} {lastName}";

		public LifeEvent Born => GetEventByType(LifeEventType.Birth);
		
		public LifeEvent Death => GetEventByType(LifeEventType.Death);
		
		public bool HasNoDiscoveries() {
			return discovered == DiscoveryType.None;
		}

		public bool IsDiscovered(DiscoveryType type) {
			return (discovered & type) != DiscoveryType.None;
		}
	}
}