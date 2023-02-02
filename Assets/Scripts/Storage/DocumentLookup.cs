using System;
using System.Collections.Generic;
using System.Linq;
using Potterblatt.GUI;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;

namespace Potterblatt.Storage {
	/// <summary>
	/// Helper utility for storing documents that were found in a search
	/// </summary>
	public struct DocumentLookup : IComparable<DocumentLookup> {
		/// <summary>The person this document is for</summary>
		public Person person;

		/// <summary>The event that this document concerns</summary>
		public LifeEvent lifeEvent;

		/// <summary>The document found </summary>
		public Document doc;

		/// <summary>Sort the documents by their date then by name</summary>
		public int CompareTo(DocumentLookup other) {
			var ret = lifeEvent.Parsed.CompareTo(other.lifeEvent.Parsed);
			return ret == 0 ? string.CompareOrdinal(person.firstName, other.person.firstName) : ret;
		}

		/// <summary>Helper function to get all the names in the storage</summary>
		public static List<string> GetNames() {
			return SaveState.IteratePeople(DiscoveryType.Name).Select(person => person.FullName).ToList();
		}

		/// <summary>Helper function to get all the locations in all the people's events in the storage</summary>
		public static List<string> GetLocations() {
			return SaveState.IterateEvents(DiscoveryType.Birth | DiscoveryType.Death)
				.Where(pair => pair.Value.source != null)
				.Select(pair => pair.Value.source.Location)
				.ToList();
		}

		/// <summary>Helper function to get all the years in all the people's events in the storage</summary>
		public static List<string> GetYears() {
			return SaveState.IterateEvents(DiscoveryType.Birth | DiscoveryType.Death)
				.Select(pair => pair.Value.Year.ToString())
				.ToList();
		}

		/// <summary>
		/// Get all the documents in a record request
		/// </summary>
		/// <param name="requiredCount">The number of search values needed to return a document</param>
		/// <param name="year">The year being searched for</param>
		/// <param name="location">The location the document should happen in</param>
		/// <param name="name">The name of the person in the document</param>
		/// <returns>A list of documents found</returns>
		/// <seealso cref="RecordRequestPage"/>
		public static IEnumerable<DocumentLookup> GetDocuments(int requiredCount, int year, string location,
			string name) {
			//get the storage
			var save = SaveState.Instance;

			//start a list
			var documents = new List<DocumentLookup>();

			//go through each person stored
			foreach(var person in save.Person.Values) {
				//go through their life events
				foreach(var lifeEvent in person.timeLine) {
					//if the event has a document attached start checking if it should be included
					if(lifeEvent.source) {
						var count = 0;

						//if year, count
						if(lifeEvent.Year == year) {
							count++;
						}

						//if location, count
						if(lifeEvent.source.Location == location) {
							count++;
						}

						//if full name, count
						if(person.FullName.Equals(name, StringComparison.InvariantCultureIgnoreCase)) {
							count++;
						}

						//if it has the required amount then return the document
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