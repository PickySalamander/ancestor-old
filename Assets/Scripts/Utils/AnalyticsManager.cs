using System.Collections.Generic;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEditor;

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
			Events.CustomData("newPage", new Dictionary<string, object> {
				{"pageName", pageName}
			});
		}
		
		public static void NewPersonPage(Person person) {
			Events.CustomData("newPage", new Dictionary<string, object> {
				{"pageName", $"{person.firstName}-{person.lastName}"},
				{"ancestorPerson", person.FullName},
				{"ancestorPersonID", person.uuid}
			});
		}

		public static void NewDocPage(Person person, Document document) {
			Events.CustomData("newPage", new Dictionary<string, object> {
				{"pageName", $"{person.firstName}-{person.lastName}-{document.GetType().Name}"},
				{"ancestorPerson", person.FullName},
				{"ancestorPersonID", person.uuid},
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
			
			Events.CustomData("ancestorDiscovery", options);
		}
		
		public static void RecordRequest(int year, string location, string searchName) {
			Events.CustomData("ancestorDiscovery", new Dictionary<string, object> {
				{"ancestorSearchYear", year},
				{"ancestorSearchLocation", location},
				{"ancestorSearchName", searchName}
			});
		}
		
		public static void Win() {
			Events.CustomData("win", new Dictionary<string, object>());
		}
	}
}