using System.Collections.Generic;
using System.Linq;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;

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

		private VisualElement resultsSection;
		private VisualTreeAsset rowTemplate;
		private VisualElement resultsParent;
		private List<VisualElement> rows;

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
			
			var documents = new SortedSet<DocumentLookup>(DocumentLookup.GetDocuments(year, location));

			var randomNames = UIManager.Instance.randomNames;

			for(var i = documents.Count; i < numResults; i++) {
				var randomPerson = ScriptableObject.CreateInstance<Person>();
				var isFemale = Random.Range(0, 2) == 1;
				randomPerson.firstName = randomNames.GetFirstName(isFemale);
				randomPerson.lastName = randomNames.GetLastName();

				var lifeEvent = new LifeEvent();
				randomPerson.timeLine = new[] {lifeEvent};
				
				var date = DateUtils.RandomDate(year - 5, year + 5);
				
				switch(Random.Range(0, 2)) {
					case 0:
						lifeEvent.type = LifeEventType.Birth;
						lifeEvent.source = BirthIndex.CreateRandom(year - 5, year + 5, location);
						break;
					case 1:
						lifeEvent.type = LifeEventType.Death;
						lifeEvent.source = DeathCert.CreateRandom(date, isFemale, location);
						break;
				}

				lifeEvent.dateTime = date.ToString(DateUtils.DateTimeFormat);

				documents.Add(new DocumentLookup {
					doc = lifeEvent.source,
					enabled = true,
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