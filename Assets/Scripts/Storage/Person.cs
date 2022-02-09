using System.Linq;
using UnityEngine;

namespace Potterblatt.Storage {
	[CreateAssetMenu(fileName = "Person", menuName = "Ancestor/Create Person")]
	public class Person : ScriptableObject {
		public Person father;
		public Person mother;
		
		public string firstName;
		public string lastName;
		public LifeEvent[] timeLine;

		public LifeEvent GetEventByType(LifeEventType type) {
			return timeLine.FirstOrDefault(lifeEvent => lifeEvent.type == type);
		}

		public LifeEvent Born => GetEventByType(LifeEventType.Birth);
		
		public LifeEvent Death => GetEventByType(LifeEventType.Death);
	}
}