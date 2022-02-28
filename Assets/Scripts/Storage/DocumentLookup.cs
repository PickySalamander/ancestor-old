using System;
using System.Collections.Generic;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;

namespace Potterblatt.Storage {
	public struct DocumentLookup : IComparable<DocumentLookup> {
		public Person person;
		public LifeEvent lifeEvent;
		public Document doc;

		public bool enabled;
		
		public static List<DocumentLookup> GetDocuments(int year, string location) {
			var save = SaveState.Instance;

			var documents = new List<DocumentLookup>();
			
			foreach(var person in save.Person.Values) {
				foreach(var lifeEvent in person.timeLine) {
					if(lifeEvent.source != null && lifeEvent.Year == year && lifeEvent.source.Location == location) {
						documents.Add(new DocumentLookup {
							person = person,
							lifeEvent = lifeEvent,
							doc = lifeEvent.source,
							enabled = true //TODO check if the person can be discovered yet
						});
					}
				}
			}

			return documents;
		}

		public int CompareTo(DocumentLookup other) {
			var ret = lifeEvent.Parsed.CompareTo(other.lifeEvent.Parsed);
			return ret == 0 ? string.CompareOrdinal(person.firstName, other.person.firstName) : ret;
		}
	}
}