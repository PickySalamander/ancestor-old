using UnityEngine;

namespace Potterblatt.Storage {
	[CreateAssetMenu(fileName = "Person", menuName = "Ancestor/Create Person")]
	public class Person : ScriptableObject {
		public Person father;
		public Person mother;
		
		public string firstName;
		public string lastName;
		public LifeEvent[] timeLine;
	}
}