using System.Collections.Generic;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class DeathCertPage : GamePage {
		public RandomNames randomNames;

		private Dictionary<string, DeathDiscovery> discoveriesAllowed;
		
		public void Setup(Person person, DeathCert deathCert) {
			deathCert.FillLabels(RootElement);

			var stateLabel = RootElement.Q<TextElement>("state-label");
			stateLabel.text = $"{deathCert.state.ToUpper()} {stateLabel.text}";

			RootElement.Q<TextElement>("fullName").text = person.FullName;

			var dateOfDeath = person.Death.Parsed;
			var deathStr = dateOfDeath.ToString(DateUtils.StandardDateFormat);
			RootElement.Q<TextElement>("deathWorkStart").text = deathStr;
			RootElement.Q<TextElement>("dateOfDeath").text = deathStr;
			RootElement.Q<TextElement>("lastSaw").text = deathStr;
			
			RootElement.Q<TextElement>("dob").text = person.Born.Parsed.ToString(DateUtils.StandardDateFormat);
			
			RootElement.Q<TextElement>("deathTime").text = dateOfDeath.ToString("h:mm tt");

			RootElement.Q<TextElement>("cornerName").text = $"{randomNames.GetFirstName()} {randomNames.GetLastName()}";
			
			RootElement.Q<TextElement>("undertaker").text = $"{randomNames.GetFirstName()} {randomNames.GetLastName()}";

			discoveriesAllowed = new Dictionary<string, DeathDiscovery>();
			foreach(var discovery in deathCert.discoveries) {
				discoveriesAllowed[discovery.deathLabel] = discovery;
			}
			
			RootElement.RegisterCallback<ClickEvent>(OnClick);
		}

		private void OnClick(ClickEvent evt) {
			if(evt.target is Button button && discoveriesAllowed.TryGetValue(button.name, out var value)) {
				SaveState.Instance.ChangeDiscovery(value.person, value.type);
			}
		}
	}
}