using System;
using Potterblatt.Storage.People;

namespace Potterblatt.GUI {
	public class BirthIndexRowInfo : IComparable<BirthIndexRowInfo> {
		public string name;
		public DateTime dob;
		public string mother;
		public string father;

		public readonly Discovery main;
		public readonly Discovery discoverMother;
		public readonly Discovery discoverFather;
		
		public BirthIndexRowInfo() {}

		public BirthIndexRowInfo(Person person) {
			name = person.FullName;
			dob = person.Born.Parsed;

			main = GetDiscovery(person);

			if(person.father) {
				father = person.father.FullName;

				discoverFather = GetDiscovery(person.father);
			}

			if(person.mother) {
				mother = person.mother.FullName;
				discoverMother = GetDiscovery(person.mother);
			}
		}

		private static Discovery GetDiscovery(Person person) {
			return person.IsReal ? new Discovery {
				person = person,
				type = DiscoveryType.Name
			} : null;
		}

		public Discovery GetDiscovery(string buttonName) {
			return buttonName switch {
				"name" => main,
				"mother" => discoverMother,
				"father" => discoverFather,
				_ => null
			};
		}

		public int CompareTo(BirthIndexRowInfo other) {
			var sort = dob.CompareTo(other.dob);
			return sort != 0 ? sort : 
				string.Compare(name, other.name, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}