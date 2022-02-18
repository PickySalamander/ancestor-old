using System;
using Potterblatt.Storage.People;

namespace Potterblatt.GUI {
	public partial class BirthIndexPage {
		public class BirthIndexRowInfo : IComparable<BirthIndexRowInfo> {
			public string name;
			public DateTime dob;
			public string mother;
			public string father;

			public Discovery discoverMother;
			public Discovery discoverFather;

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
}