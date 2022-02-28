using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class DeathCertPage : GamePage {
		public RandomNames randomNames;
		
		public void Setup(Person person, DeathCert deathCert) {
			deathCert.FillLabels(RootElement);

			var stateLabel = RootElement.Q<Label>("state-label");
			stateLabel.text = $"{deathCert.state.ToUpper()} {stateLabel.text}";

			RootElement.Q<Label>("fullName").text = person.FullName;

			var dateOfDeath = person.Death.Parsed;
			var deathStr = dateOfDeath.ToString(DateUtils.StandardDateFormat);
			RootElement.Q<Label>("deathWorkStart").text = deathStr;
			RootElement.Q<Label>("dateOfDeath").text = deathStr;
			RootElement.Q<Label>("lastSaw").text = deathStr;
			
			RootElement.Q<Label>("deathTime").text = dateOfDeath.ToString("h:mm tt");

			RootElement.Q<Label>("cornerName").text = $"{randomNames.GetFirstName()} {randomNames.GetLastName()}";
			
			RootElement.Q<Label>("undertaker").text = $"{randomNames.GetFirstName()} {randomNames.GetLastName()}";
		}
	}
}