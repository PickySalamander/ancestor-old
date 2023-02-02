using System.Collections.Generic;
using Bogus;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Utils;
using UnityEngine.UIElements;
using Person = Potterblatt.Storage.People.Person;

namespace Potterblatt.GUI {
	/// <summary>
	/// Controller for the Death Certificate document
	/// </summary>
	public class DeathCertPage : GamePage {
		/// <summary>Current death cert being shown</summary>
		private DeathCert currentPage;
		
		/// <summary>The discoveries to be made on the page keyed by the button name</summary>
		private Dictionary<string, DeathDiscovery> discoveriesAllowed;
		
		/// <summary>
		/// Called to setup the page with new people
		/// </summary>
		/// <param name="person">The real person who is supposed to be on the page</param>
		/// <param name="deathCert">The page to show on the page</param>
		public void Setup(Person person, DeathCert deathCert) {
			currentPage = deathCert;
			
			//fill the text labels on the page that it can
			deathCert.FillLabels(RootElement);

			//Set the US state label on the top of the page
			var stateLabel = RootElement.Q<TextElement>("state-label");
			stateLabel.text = $"{deathCert.state.ToUpper()} {stateLabel.text}";

			//set the persons fullname
			RootElement.Q<TextElement>("fullName").text = person.FullName;

			//set the date fields
			var dateOfDeath = person.Death.Parsed;
			var deathStr = dateOfDeath.ToString(DateUtils.StandardDateFormat);
			RootElement.Q<TextElement>("deathWorkStart").text = deathStr;
			RootElement.Q<TextElement>("dateOfDeath").text = deathStr;
			RootElement.Q<TextElement>("lastSaw").text = deathStr;
			
			RootElement.Q<TextElement>("dob").text = 
				person.Born.Parsed.ToString(DateUtils.StandardDateFormat);
			
			RootElement.Q<TextElement>("deathTime").text = dateOfDeath.ToString("h:mm tt");

			//set random data
			var faker = new Faker();
			RootElement.Q<TextElement>("cornerName").text = faker.Name.FullName();
			RootElement.Q<TextElement>("undertaker").text = faker.Name.FullName();

			//set up all the discoveries that can be made on the page
			discoveriesAllowed = new Dictionary<string, DeathDiscovery>();
			if(deathCert.discoveries != null) {
				foreach(var discovery in deathCert.discoveries) {
					discoveriesAllowed[discovery.deathLabel] = discovery;
				}
			}

			//listen to clicks
			RootElement.RegisterCallback<ClickEvent>(OnClick);
		}

		/// <summary>
		/// Called when someone clicks on the page. It determines which discovery was clicked on if any.
		/// </summary>
		/// <param name="evt">The <see cref="ClickEvent"/> from the callback</param>
		private void OnClick(ClickEvent evt) {
			//if a button and in the discovery list, add the discovery
			if(evt.target is Button button && discoveriesAllowed.TryGetValue(button.name, out var value)) {
				AnalyticsManager.NewDiscovery(value.person, value.type, currentPage);
				SaveState.Instance.ChangeDiscovery(value.person, value.type);
			}
		}
	}
}