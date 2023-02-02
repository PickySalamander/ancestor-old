using System;
using Potterblatt.Storage.People;

namespace Potterblatt.GUI {
	/// <summary>
	/// Storage of a row on the <see cref="BirthIndexPage"/>
	/// </summary>
	public class BirthIndexRowInfo : IComparable<BirthIndexRowInfo> {
		/// <summary>Full name of the person</summary>
		public string name;

		/// <summary>Date and time when they were born</summary>
		public DateTime dob;

		/// <summary>Full name of mother</summary>
		public string mother;

		/// <summary>Full name of father</summary>
		public string father;

		/// <summary>The main person's discovery</summary>
		public readonly Discovery main;

		/// <summary>Discovery of the mother</summary>
		public readonly Discovery discoverMother;

		/// <summary>Discovery of the father</summary>
		public readonly Discovery discoverFather;

		/// <summary>Create a new row</summary>
		public BirthIndexRowInfo() { }

		/// <summary>
		/// Create a new row with a real person in mind
		/// </summary>
		/// <param name="person">the real person to generate the row with</param>
		public BirthIndexRowInfo(Person person) {
			//populate name and dob
			name = person.FullName;
			dob = person.Born.Parsed;

			//get main discovery
			main = GetDiscovery(person);

			//get father and discovery
			if(person.father) {
				father = person.father.FullName;

				discoverFather = GetDiscovery(person.father);
			}

			//get mother and discovery
			if(person.mother) {
				mother = person.mother.FullName;
				discoverMother = GetDiscovery(person.mother);
			}
		}

		/// <summary>
		/// Helper function to get a discovery from a real person
		/// </summary>
		/// <param name="person">The person to get the discover from</param>
		/// <returns>the discovery or null if the person isn't real</returns>
		private static Discovery GetDiscovery(Person person) {
			return person.IsReal
				? new Discovery {
					person = person,
					type = DiscoveryType.Name
				}
				: null;
		}

		/// <summary>
		/// Get a discovery for a button that was clicked
		/// </summary>
		/// <param name="buttonName">The name of the button should be "name", "mother", or "father"</param>
		/// <returns></returns>
		public Discovery GetDiscovery(string buttonName) {
			return buttonName switch {
				"name" => main,
				"mother" => discoverMother,
				"father" => discoverFather,
				_ => null
			};
		}

		/// <summary>
		/// Sorting by dob and then name for rows
		/// </summary>
		/// <param name="other">the row to sort against</param>
		public int CompareTo(BirthIndexRowInfo other) {
			var sort = dob.CompareTo(other.dob);
			return sort != 0 ? sort : string.Compare(name, other.name, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}