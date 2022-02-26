using System;
using System.Collections.Generic;
using System.Linq;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Potterblatt.GUI {
	public class RecordRequestPage : GamePage {
		[Min(0)] public int requiredCount = 3;

		public int numResults = 20;

		private Dictionary<string, Person> nameToPerson;

		private RecordRequestDropdown nameList;
		private RecordRequestDropdown years;
		private RecordRequestDropdown locations;

		private Label requiredCounter;
		private Button submitButton;

		private int count;

		private void OnEnable() {
			nameToPerson =
				SaveState.Instance.Person.Values
					.Where(person => SaveState.Instance[person].HasFlag(DiscoveryType.Name))
					.ToDictionary(person => person.FullName);

			nameList = new RecordRequestDropdown(RootElement.Q<VisualElement>("name-list")) {
				Choices = nameToPerson.Keys.ToList()
			};
			nameList.OnChange += OnNewName;

			years = new RecordRequestDropdown(RootElement.Q<VisualElement>("year"));
			years.OnChange += OnChange;

			locations = new RecordRequestDropdown(RootElement.Q<VisualElement>("location"));
			locations.OnChange += OnChange;

			requiredCounter = RootElement.Q<Label>("required-counter");

			submitButton = RootElement.Q<Button>("search-button");
			submitButton.clicked += OnSubmit;

			OnChange();
		}

		private void OnChange(ChangeEvent<string> evt = null) {
			count = 0;

			if(nameList.HasValue) {
				count++;
			}

			years.SetChecked(years.HasValue);
			if(years.HasValue) {
				count++;
			}

			locations.SetChecked(locations.HasValue);
			if(locations.HasValue) {
				count++;
			}

			requiredCounter.text = $"{count} / {requiredCount}";
			submitButton.SetEnabled(count == requiredCount);
		}

		private void OnNewName(ChangeEvent<string> evt) {
			Debug.Log($"There was a name change! {evt.newValue}");

			if(!string.IsNullOrWhiteSpace(evt.newValue)) {
				var newPerson = nameToPerson[evt.newValue];

				years.Choices = new List<string>();
				locations.Choices = new List<string>();

				foreach(var lifeEvent in newPerson.timeLine) {
					if(newPerson.IsDiscovered(lifeEvent)) {
						years.Choices.Add(lifeEvent.Parsed.Year.ToString());

						if(lifeEvent.source != null) {
							locations.Choices.Add(lifeEvent.source.Location);
						}
					}
				}

				nameList.SetChecked(true);
				years.SetEnabled(true);
				locations.SetEnabled(true);
			}
			else {
				nameList.SetChecked(false);
				years.SetEnabled(false);
				locations.SetEnabled(false);
			}
		}

		private void OnSubmit() {
			var year = int.Parse(years.dropdown.value);
			var location = locations.dropdown.value;
			
			var documents = DocumentLookup.GetDocuments(year, location);

			var randomNames = UIManager.Instance.randomNames;

			for(var i = documents.Count; i < numResults; i++) {
				var randomPerson = ScriptableObject.CreateInstance<Person>();
				var isFemale = Random.Range(0, 2) == 1;
				randomPerson.firstName = randomNames.GetFirstName(isFemale);
				randomPerson.lastName = randomNames.GetLastName();

				var lifeEvent = new LifeEvent();
				randomPerson.timeLine = new[] {lifeEvent};
				
				switch(Random.Range(0, 2)) {
					case 0:
						lifeEvent.type = LifeEventType.Birth;
						lifeEvent.source = BirthIndex.CreateRandom(year - 5, year + 5, location);
						break;
					case 1:
						lifeEvent.type = LifeEventType.Death;
						var date = DateUtils.RandomDate(year - 5, year + 5);
						lifeEvent.source = DeathCert.CreateRandom(date, isFemale, location);
						break;
				}
			}
			
			//TODO populate
		}
	}
}