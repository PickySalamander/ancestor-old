using System;
using Potterblatt.Storage.People;

namespace Potterblatt.GUI {
	public class BirthIndexRowInfo : IComparable<BirthIndexRowInfo> {
		public string name;
		public DateTime dob;
		public string mother;
		public string father;

		public Discovery discoverMother;
		public Discovery discoverFather;
		
		public BirthIndexRowInfo() {}

		public BirthIndexRowInfo(Person person, DiscoveryType discovery) {
			name = person.FullName;
			dob = person.Born.Parsed;

			if(person.father) {
				father = person.father.FullName;

				discoverFather = new Discovery {
					person = person.father,
					type = discovery
				};
			}

			if(person.mother) {
				mother = person.mother.FullName;
				discoverMother = new Discovery {
					person = person.mother,
					type = discovery
				};
			}
		}

		public int CompareTo(BirthIndexRowInfo other) {
			var sort = dob.CompareTo(other.dob);
			return sort != 0 ? sort : 
				string.Compare(name, other.name, StringComparison.InvariantCultureIgnoreCase);
		}

		public bool IsDiscoverable() {
			return discoverMother.person || discoverFather.person;
		}
		
		public bool IsDiscoverable(bool isMother) {
			return isMother ? discoverMother.person : discoverFather.person;
		}
	}
}