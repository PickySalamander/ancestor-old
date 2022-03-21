using System;
using System.Collections.Generic;
using Bogus;
using Bogus.DataSets;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using Person = Potterblatt.Storage.People.Person;
using Random = UnityEngine.Random;

namespace Potterblatt.GUI {
	public class RecordRequestPage : GamePage {
		[Min(0)] public int requiredCount = 3;

		public int numResults = 20;

		private RecordRequestDropdown years;
		private RecordRequestDropdown locations;
		private RecordRequestDropdown nameSearch;

		private Label requiredCounter;
		private Button submitButton;

		private int count;

		private VisualElement resultsSection;
		private VisualTreeAsset rowTemplate;
		private VisualElement resultsParent;
		private List<VisualElement> rows;

		private void OnEnable() {
			years = new RecordRequestDropdown(RootElement.Q<VisualElement>("year"), 
				DocumentLookup.GetYears());
			years.OnChange += OnChange;

			locations = new RecordRequestDropdown(RootElement.Q<VisualElement>("location"), 
				DocumentLookup.GetLocations());
			locations.OnChange += OnChange;

			nameSearch = new RecordRequestDropdown(RootElement.Q<VisualElement>("name"), 
				DocumentLookup.GetNames());
			nameSearch.OnChange += OnChange;

			requiredCounter = RootElement.Q<Label>("required-counter");

			submitButton = RootElement.Q<Button>("search-button");
			submitButton.clicked += OnSubmit;

			resultsSection = RootElement.Q<VisualElement>("results");
			resultsSection.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

			resultsParent = resultsSection.Q<ScrollView>();

			var container = RootElement.Q<TemplateContainer>("request-result");
			container.RemoveFromHierarchy();
			rowTemplate = container.templateSource;

			rows = null;

			OnChange();
		}

		

		private void OnChange(ChangeEvent<string> evt = null) {
			count = 0;

			//TODO make this cleaner

			years.SetChecked(years.HasValue);
			if(years.HasValue) {
				count++;
			}

			locations.SetChecked(locations.HasValue);
			if(locations.HasValue) {
				count++;
			}

			nameSearch.SetChecked(nameSearch.HasValue);
			if(nameSearch.HasValue) {
				count++;
			}

			requiredCounter.text = $"{count} / {requiredCount}";
			submitButton.SetEnabled(count >= requiredCount);
		}

		private void OnSubmit() {
			int.TryParse(years.dropdown.value, out var year);
			var location = locations.dropdown.value;
			var searchName = nameSearch.dropdown.value;
			
			AnalyticsManager.RecordRequest(year, location, searchName);

			var documents = new SortedSet<DocumentLookup>(
				DocumentLookup.GetDocuments(requiredCount, year, location, searchName));

			var faker = new Faker();

			for(var i = documents.Count; i < numResults; i++) {
				var randomPerson = ScriptableObject.CreateInstance<Person>();
				var gender = faker.PickRandom<Name.Gender>();
				randomPerson.firstName = faker.Name.FirstName(gender);
				randomPerson.lastName = faker.Name.LastName();

				//TODO hard code the min year (maybe birth indexes only go back so far?)
				var minYear = year > 0 ? year - 5 : 1700;
				var maxYear = year > 0 ? year + 5 : DateTime.Now.Year;
				var birth = DateUtils.RandomDate(minYear, maxYear);
				var death = DateUtils.RandomDateInYear(Random.Range(0, 100) + birth.Year);
				
				randomPerson.timeLine = new[] {
					new LifeEvent { type = LifeEventType.Birth, dateTime = birth.Ticks},
					new LifeEvent { type = LifeEventType.Death, dateTime = death.Ticks}
				};
				
				//TODO maybe do this differently
				var events = death > DateTime.Now
					? new[] {LifeEventType.Birth}
					: new[] {LifeEventType.Birth, LifeEventType.Death};
				
				var eventType = faker.PickRandom(events);
				LifeEvent lifeEvent;
				switch(eventType) {
					case LifeEventType.Birth:
						lifeEvent = randomPerson.Born;
						lifeEvent.source = BirthIndex.CreateRandom(
							birth.Year - 25,
							Mathf.Min(birth.Year + 25, DateTime.Now.Year),
							location);
						break;
					case LifeEventType.Death:
						lifeEvent = randomPerson.Death;
						lifeEvent.source = DeathCert.CreateRandom(faker, death, gender, location);
						break;
					default:
						throw new IndexOutOfRangeException("This shouldn't happen");
				}

				documents.Add(new DocumentLookup {
					doc = lifeEvent.source,
					lifeEvent = lifeEvent,
					person = randomPerson
				});
			}

			if(rows != null) {
				foreach(var row in rows) {
					row.RemoveFromHierarchy();
				}
			}

			rows = new List<VisualElement>();

			resultsSection.style.display = new StyleEnum<DisplayStyle>(StyleKeyword.Undefined);
			foreach(var document in documents) {
				var newElement = rowTemplate.CloneTree();
				newElement.Q<Label>("date").text = document.lifeEvent.Parsed.ToString(DateUtils.IndexDateFormat);
				newElement.Q<Label>("file-name").text = document.doc.FileName;
				newElement.Q<Label>("person-name").text = document.person.FullName;
				newElement.RegisterCallback<ClickEvent>(_ => OnClick(document));

				rows.Add(newElement);
				resultsParent.Add(newElement);
			}
		}

		private void OnClick(DocumentLookup document) {
			if(document.person.IsReal && document.person.IsDiscovered(document.lifeEvent)) {
				DialogManager.Instance.ShowDialog("Already Discovered",
					"You have already discovered this document");
			}
			else {
				UIManager.Instance.OpenDoc(document.person, document.doc);
			}
		}
	}
}