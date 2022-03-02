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

			var documents = new SortedSet<DocumentLookup>(
				DocumentLookup.GetDocuments(requiredCount, year, location, searchName));

			var randomNames = UIManager.Instance.randomNames;

			for(var i = documents.Count; i < numResults; i++) {
				var randomPerson = ScriptableObject.CreateInstance<Person>();
				var isFemale = Random.Range(0, 2) == 1;
				randomPerson.firstName = randomNames.GetFirstName(isFemale);
				randomPerson.lastName = randomNames.GetLastName();

				var lifeEvent = new LifeEvent();
				randomPerson.timeLine = new[] {lifeEvent};

				//TODO hard code the min year (maybe birth indexes only go back so far?)
				var minYear = year > 0 ? year - 5 : 1700;
				var maxYear = year > 0 ? year + 5 : DateTime.Now.Year;
				var date = DateUtils.RandomDate(minYear, maxYear);

				switch(Random.Range(0, 2)) {
					case 0:
						lifeEvent.type = LifeEventType.Birth;
						lifeEvent.source = BirthIndex.CreateRandom(
							date.Year - 25,
							Mathf.Min(date.Year + 25, DateTime.Now.Year),
							location);
						break;
					case 1:
						lifeEvent.type = LifeEventType.Death;
						lifeEvent.source = DeathCert.CreateRandom(date, isFemale, location);
						break;
				}

				lifeEvent.dateTime = date.ToString(DateUtils.DateTimeFormat);

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