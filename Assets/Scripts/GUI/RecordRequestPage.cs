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
	/// <summary>
	/// Page where record requests are made for different documents
	/// </summary>
	public class RecordRequestPage : GamePage {
		[Tooltip("The number of dropdowns to be filled out before a request can be made")]
		[Min(0)] public int requiredCount = 3;

		[Tooltip("Number of results to show")]
		public int numResults = 20;

		/// <summary>The year range to request</summary>
		private RecordRequestDropdown years;
		
		/// <summary>The location to request</summary>
		private RecordRequestDropdown locations;
		
		/// <summary>The name of the documents to get</summary>
		private RecordRequestDropdown nameSearch;

		/// <summary>Counter showing the <see cref="requiredCount"/></summary>
		private Label requiredCounter;
		
		/// <summary>Button to submit the request</summary>
		private Button submitButton;

		/// <summary>Shown results display</summary>
		private VisualElement resultsSection;
		
		/// <summary>Each row shown in the results section</summary>
		private VisualTreeAsset rowTemplate;
		
		/// <summary>The parent to add rows to</summary>
		private VisualElement resultsParent;
		
		/// <summary>The rows that have been instantiated</summary>
		private List<VisualElement> rows;

		private void OnEnable() {
			//add dropdowns and listen for changes
			years = new RecordRequestDropdown(RootElement.Q<VisualElement>("year"), 
				DocumentLookup.GetYears());
			years.OnChange += OnChange;

			locations = new RecordRequestDropdown(RootElement.Q<VisualElement>("location"), 
				DocumentLookup.GetLocations());
			locations.OnChange += OnChange;

			nameSearch = new RecordRequestDropdown(RootElement.Q<VisualElement>("name"), 
				DocumentLookup.GetNames());
			nameSearch.OnChange += OnChange;

			//get various buttons and labels
			requiredCounter = RootElement.Q<Label>("required-counter");

			submitButton = RootElement.Q<Button>("search-button");
			submitButton.clicked += OnSubmit;

			resultsSection = RootElement.Q<VisualElement>("results");
			resultsSection.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);

			resultsParent = resultsSection.Q<ScrollView>();

			//get row template
			var container = RootElement.Q<TemplateContainer>("request-result");
			container.RemoveFromHierarchy();
			rowTemplate = container.templateSource;

			rows = null;

			//issue first change
			OnChange();
		}
		
		/// <summary>
		/// Called when something changes to recompute what is in each dropdown
		/// </summary>
		/// <param name="evt"></param>
		private void OnChange(ChangeEvent<string> evt = null) {
			//count the dropdowns that have been populated
			var count = 0;

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

			//if at the required count then the submit button enabled
			requiredCounter.text = $"{count} / {requiredCount}";
			submitButton.SetEnabled(count >= requiredCount);
		}

		/// <summary>
		/// Called when the submit button is press
		/// </summary>
		private void OnSubmit() {
			//get all the values from the dropdowns
			int.TryParse(years.dropdown.value, out var year);
			var location = locations.dropdown.value;
			var searchName = nameSearch.dropdown.value;
			
			//track what the request was
			AnalyticsManager.RecordRequest(year, location, searchName);

			//get all the documents that the user can look up 
			var documents = new SortedSet<DocumentLookup>(
				DocumentLookup.GetDocuments(requiredCount, year, location, searchName));

			var faker = new Faker();

			//generate a series of fake documents subtracting those already found
			for(var i = documents.Count; i < numResults; i++) {
				//get a random person and set their gender and name
				var randomPerson = ScriptableObject.CreateInstance<Person>();
				var gender = faker.PickRandom<Name.Gender>();
				randomPerson.firstName = faker.Name.FirstName(gender);
				randomPerson.lastName = faker.Name.LastName();

				//generate the birth and death dates
				var minYear = year > 0 ? year - 5 : 1800;
				var maxYear = year > 0 ? year + 5 : DateTime.Now.Year;
				var birth = DateUtils.RandomDate(minYear, maxYear);
				var death = DateUtils.RandomDateInYear(Random.Range(0, 100) + birth.Year);
				
				randomPerson.timeLine = new[] {
					new LifeEvent { type = LifeEventType.Birth, dateTime = birth.Ticks},
					new LifeEvent { type = LifeEventType.Death, dateTime = death.Ticks}
				};
				
				var events = death > DateTime.Now
					? new[] {LifeEventType.Birth}
					: new[] {LifeEventType.Birth, LifeEventType.Death};
				
				//pick either the birth or death and setup a document for it
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

				//add the document to the list
				documents.Add(new DocumentLookup {
					doc = lifeEvent.source,
					lifeEvent = lifeEvent,
					person = randomPerson
				});
			}

			//remove all previous results
			if(rows != null) {
				foreach(var row in rows) {
					row.RemoveFromHierarchy();
				}
			}

			rows = new List<VisualElement>();

			//set the results section to visible
			resultsSection.style.display = new StyleEnum<DisplayStyle>(StyleKeyword.Undefined);
			
			//add each document to the list and populate the text
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

		/// <summary>
		/// Called when the user clicks on a document
		/// </summary>
		/// <param name="document">The document that was clicked</param>
		private static void OnClick(DocumentLookup document) {
			//don't open if they already discovered the person in the document
			if(document.person.IsReal && document.person.IsDiscovered(document.lifeEvent)) {
				DialogManager.Instance.ShowDialog("Already Discovered",
					"You have already discovered this document");
			}
			
			//open the doc if otherwise
			else {
				UIManager.Instance.OpenDoc(document.person, document.doc);
			}
		}
	}
}