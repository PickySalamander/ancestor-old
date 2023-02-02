using System;
using System.Collections.Generic;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Potterblatt.Utils {
	/// <summary>
	/// Singleton manager for tracking user interactions through Unity Analytics
	/// </summary>
	public class AnalyticsManager : SingletonMonobehaviour<AnalyticsManager> {
		private async void Start() {
			var options = new InitializationOptions();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
			options.SetEnvironmentName("test");
#else
			options.SetEnvironmentName("production");
#endif

			//initialize
			await UnityServices.InitializeAsync(options);
		}

		/// <summary>
		/// Track that the user made it to a new page
		/// </summary>
		/// <param name="pageName">The name of the page</param>
		public static void NewPage(string pageName) {
			SendEvent("newPage", new Dictionary<string, object> {
				{"pageName", pageName}
			});
		}
		
		/// <summary>
		/// Track that the user made it to a new user info page
		/// </summary>
		/// <param name="person">the person whose info page was viewed</param>
		public static void NewPersonPage(Person person) {
			SendEvent("newPage", new Dictionary<string, object> {
				{"pageName", $"{person.firstName}-{person.lastName}"},
				{"ancestorPerson", person.FullName},
				{"ancestorPersonID", person.uuid}
			});
		}

		/// <summary>
		/// Track that the user made it to a view a new document
		/// </summary>
		/// <param name="person">the person who's document this is</param>
		/// <param name="document">The document that was viewed</param>
		public static void NewDocPage(Person person, Document document) {
			SendEvent("newPage", new Dictionary<string, object> {
				{"pageName", $"{person.firstName}-{person.lastName}-{document.GetType().Name}"},
				{"ancestorPerson", person.FullName},
				{"ancestorPersonID", person.uuid ?? "not-real"},
				{"ancestorDoc", document.FileName}
			});
		}
		
		/// <summary>
		/// Track that the person made a new discovery
		/// </summary>
		/// <param name="person">The person the discovery was for</param>
		/// <param name="type">The type of discovery</param>
		/// <param name="document">The document that the discovery was made in</param>
		public static void NewDiscovery(Person person, DiscoveryType type, Document document=null) {
			var options = new Dictionary<string, object> {
				{"ancestorPerson", person.FullName},
				{"ancestorPersonID", person.uuid},
				{"ancestorDiscoveryType", type.ToString()}
			};

			if(document) {
				options["ancestorDoc"] = document.FileName;
			}
			
			SendEvent("ancestorDiscovery", options);
		}
		
		/// <summary>
		/// Track that the user made a record request
		/// </summary>
		/// <param name="year">The year of the search</param>
		/// <param name="location">The location of the search</param>
		/// <param name="searchName">The name of the person being searched</param>
		public static void RecordRequest(int year, string location, string searchName) {
			SendEvent("ancestorRecordRequest", new Dictionary<string, object> {
				{"ancestorSearchYear", year},
				{"ancestorSearchLocation", location},
				{"ancestorSearchName", searchName}
			});
		}
		
		/// <summary>
		/// Track that the use won
		/// </summary>
		public static void Win() {
			SendEvent("win", new Dictionary<string, object>());
		}

		/// <summary>
		/// Send an event to the analytics system
		/// </summary>
		/// <param name="eventName">The name of the event</param>
		/// <param name="eventParams">The parameters of the event</param>
		private static void SendEvent(string eventName, IDictionary<string, object> eventParams = null) {
			try {
				Events.CustomData(eventName, eventParams ?? new Dictionary<string, object>());
			}
			catch(Exception e) {
				Debug.LogWarning($"Failed to make tracking event: {e.GetType()} - {e.Message}", Instance);

				if(Debug.isDebugBuild) {
					Debug.LogException(e);
				}
			}
		}
	}
}