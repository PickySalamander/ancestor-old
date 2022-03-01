using System;
using System.Collections.Generic;
using System.Linq;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;

namespace Potterblatt.Storage {
	public struct DocumentLookup : IComparable<DocumentLookup> {
		public Person person;
		public LifeEvent lifeEvent;
		public Document doc;

		public int CompareTo(DocumentLookup other) {
			var ret = lifeEvent.Parsed.CompareTo(other.lifeEvent.Parsed);
			return ret == 0 ? string.CompareOrdinal(person.firstName, other.person.firstName) : ret;
		}
		
		public static List<string> GetNames() {
			return SaveState.IteratePeople(DiscoveryType.Name).Select(person => person.FullName).ToList();
		}

		public static List<string> GetLocations() {
			return SaveState.IterateEvents(DiscoveryType.Birth | DiscoveryType.Death)
				.Where(pair => pair.Value.source != null)
				.Select(pair => pair.Value.source.Location)
				.ToList();
		}

		public static List<string> GetYears() {
			return SaveState.IterateEvents(DiscoveryType.Birth | DiscoveryType.Death)
				.Select(pair => pair.Value.Year.ToString())
				.ToList();
		}
		
		public static List<DocumentLookup> GetDocuments(int requiredCount, int year, string location, string name) {
			var save = SaveState.Instance;

			var documents = new List<DocumentLookup>();
			
			foreach(var person in save.Person.Values) {
				foreach(var lifeEvent in person.timeLine) {
					if(lifeEvent.source) {
						var count = 0;
						
						if(lifeEvent.Year == year) {
							count++;
						}

						if(lifeEvent.source.Location == location) {
							count++;
						}

						if(person.FullName.Equals(name, StringComparison.InvariantCultureIgnoreCase)) {
							count++;
						}
						
						if(count >= requiredCount) {
							documents.Add(new DocumentLookup {
								person = person,
								lifeEvent = lifeEvent,
								doc = lifeEvent.source
							});
						}
					}
				}
			}

			return documents;
		}

		
	}
}