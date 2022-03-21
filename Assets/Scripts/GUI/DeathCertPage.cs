using System.Collections.Generic;
using Bogus;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Utils;
using UnityEngine.UIElements;
using Person = Potterblatt.Storage.People.Person;

namespace Potterblatt.GUI {
	public class DeathCertPage : GamePage {

		private DeathCert currentPage;
		private Dictionary<string, DeathDiscovery> discoveriesAllowed;
		
		public void Setup(Person person, DeathCert deathCert) {
			currentPage = deathCert;
			
			deathCert.FillLabels(RootElement);

			var stateLabel = RootElement.Q<TextElement>("state-label");
			stateLabel.text = $"{deathCert.state.ToUpper()} {stateLabel.text}";

			RootElement.Q<TextElement>("fullName").text = person.FullName;

			var dateOfDeath = person.Death.Parsed;
			var deathStr = dateOfDeath.ToString(DateUtils.StandardDateFormat);
			RootElement.Q<TextElement>("deathWorkStart").text = deathStr;
			RootElement.Q<TextElement>("dateOfDeath").text = deathStr;
			RootElement.Q<TextElement>("lastSaw").text = deathStr;
			
			RootElement.Q<TextElement>("dob").text = 
				person.Born.Parsed.ToString(DateUtils.StandardDateFormat);
			
			RootElement.Q<TextElement>("deathTime").text = dateOfDeath.ToString("h:mm tt");

			var faker = new Faker();

			RootElement.Q<TextElement>("cornerName").text = faker.Name.FullName();
			
			RootElement.Q<TextElement>("undertaker").text = faker.Name.FullName();

			discoveriesAllowed = new Dictionary<string, DeathDiscovery>();
			if(deathCert.discoveries != null) {
				foreach(var discovery in deathCert.discoveries) {
					discoveriesAllowed[discovery.deathLabel] = discovery;
				}
			}

			RootElement.RegisterCallback<ClickEvent>(OnClick);
		}

		private void OnClick(ClickEvent evt) {
			if(evt.target is Button button && discoveriesAllowed.TryGetValue(button.name, out var value)) {
				AnalyticsManager.NewDiscovery(value.person, value.type, currentPage);
				SaveState.Instance.ChangeDiscovery(value.person, value.type);
			}
		}
	}
}