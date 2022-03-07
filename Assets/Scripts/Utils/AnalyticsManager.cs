﻿using System;
using System.Collections.Generic;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Potterblatt.Utils {
	public class AnalyticsManager : SingletonMonobehaviour<AnalyticsManager> {
		private async void Start() {
			var options = new InitializationOptions();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
			options.SetEnvironmentName("test");
#else
			options.SetEnvironmentName("production");
#endif

			await UnityServices.InitializeAsync(options);
		}

		public static void NewPage(string pageName) {
			SendEvent("newPage", new Dictionary<string, object> {
				{"pageName", pageName}
			});
		}
		
		public static void NewPersonPage(Person person) {
			SendEvent("newPage", new Dictionary<string, object> {
				{"pageName", $"{person.firstName}-{person.lastName}"},
				{"ancestorPerson", person.FullName},
				{"ancestorPersonID", person.uuid}
			});
		}

		public static void NewDocPage(Person person, Document document) {
			SendEvent("newPage", new Dictionary<string, object> {
				{"pageName", $"{person.firstName}-{person.lastName}-{document.GetType().Name}"},
				{"ancestorPerson", person.FullName},
				{"ancestorPersonID", person.uuid ?? "not-real"},
				{"ancestorDoc", document.FileName}
			});
		}
		
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
		
		public static void RecordRequest(int year, string location, string searchName) {
			SendEvent("ancestorRecordRequest", new Dictionary<string, object> {
				{"ancestorSearchYear", year},
				{"ancestorSearchLocation", location},
				{"ancestorSearchName", searchName}
			});
		}
		
		public static void Win() {
			SendEvent("win", new Dictionary<string, object>());
		}

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