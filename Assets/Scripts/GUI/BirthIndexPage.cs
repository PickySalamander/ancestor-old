using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Potterblatt.Storage;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using Person = Potterblatt.Storage.People.Person;
using Random = UnityEngine.Random;

namespace Potterblatt.GUI {
	/// <summary>
	/// Controller for the Birth Index document
	/// </summary>
	public class BirthIndexPage : GamePage {
		/// <summary>Current index being shown</summary>
		private BirthIndex currentPage;

		/// <summary>The template for each row in the document</summary>
		private VisualTreeAsset rowTemplate;

		/// <summary>Where to add new rows to</summary>
		private VisualElement rowParent;

		/// <summary>Number of people to generate</summary>
		private int numberOfPeople;

		/// <summary>The height of the template to use for height calculations</summary>
		private int templateHeight;

		/// <summary>The label for the current US state of the document</summary>
		private Label stateLabel;

		private void OnEnable() {
			stateLabel = RootElement.Q<Label>("state-label");

			templateHeight = 0;
			numberOfPeople = 0;

			//wait until this page is attached to start doing extra work
			RootElement.RegisterCallback<GeometryChangedEvent>(OnAttached);
		}

		/// <summary>When the page is attached to the screen start working on display</summary>
		private void OnAttached(EventBase evt) {
			RootElement.UnregisterCallback<GeometryChangedEvent>(OnAttached);

			//get the template and remove it from the hierarchy
			var template = RootElement.Q<TemplateContainer>("Birth Row");
			rowParent = template.parent;
			template.RemoveFromHierarchy();
			rowTemplate = template.templateSource;

			//calculate the height and how many people can fit on the page
			templateHeight = (int) Math.Ceiling(template.contentRect.height);
			var maxHeight = rowParent.contentRect.height;
			numberOfPeople = (int) Math.Ceiling(maxHeight / templateHeight);
		}

		/// <summary>
		/// Called to setup the page with new people
		/// </summary>
		/// <param name="person">The real person who is supposed to be on the page</param>
		/// <param name="index">The page to show on the page</param>
		public void Setup(Person person, BirthIndex index) {
			//Set the US state label on the top of the page
			stateLabel.text = $"{index.state} State Board of Health".ToUpper();

			currentPage = index;

			//Start the population process
			StartCoroutine(WaitForSetup(person, index));
		}

		/// <summary>
		/// <see cref="Coroutine"/> that sets up the people on the page
		/// </summary>
		/// <param name="person">The real person who is supposed to be on the page</param>
		/// <param name="index">The page to show on the page</param>
		private IEnumerator WaitForSetup(Person person, BirthIndex index) {
			//wait until the page is added to the display
			yield return new WaitUntil(() => numberOfPeople > 0);

			//create a sorted list of the rows to display
			var newPeople = new SortedSet<BirthIndexRowInfo>();

			//faker for randomness
			var faker = new Faker();

			//generate the correct amount of people minus the real person
			for(var i = 0; i < numberOfPeople - 1; i++) {
				//generate the name details using Faker
				var lastName = faker.Name.LastName();
				var maidenName = Random.Range(0f, 1f) <= index.maidenNameFrequency;
				var motherLastName = maidenName ? $"{faker.Name.LastName()} {lastName}" : lastName;

				//add them to the list
				newPeople.Add(new BirthIndexRowInfo {
					name = $"{faker.Name.FirstName()} {lastName}",
					dob = DateUtils.RandomDate(index.startYear, index.endYear),
					father = $"{faker.Name.FirstName(Name.Gender.Male)} {lastName}",
					mother = $"{faker.Name.FirstName(Name.Gender.Female)} {motherLastName}"
				});
			}

			//add the real person to the list
			newPeople.Add(new BirthIndexRowInfo(person));

			//now go through each person in order of the sorted set and add them to the page
			TemplateContainer newRow = null;
			foreach(var newPerson in newPeople) {
				newRow = rowTemplate.CloneTree();
				newRow.Q<TextElement>("date").text = newPerson.dob.ToString(DateUtils.IndexDateFormat);
				newRow.Q<TextElement>("name").text = newPerson.name;

				newRow.Q<TextElement>("father").text = newPerson.father;
				newRow.Q<TextElement>("mother").text = newPerson.mother;
				rowParent.Add(newRow);
				newRow.userData = newPerson;
			}

			//add special css to the end
			newRow?.Children().First().AddToClassList("last");

			//listen to clicks
			rowParent.RegisterCallback<ClickEvent>(OnClick);
		}

		/// <summary>
		/// Called when someone clicks on the <see cref="rowParent"/>. It determines which row was clicked on if any.
		/// </summary>
		/// <param name="evt">The <see cref="ClickEvent"/> from the callback</param>
		private void OnClick(ClickEvent evt) {
			//only process buttons
			if(evt.target is Button button) {
				//find the user data set by the setup phase
				var userData = button.FindAncestorUserData();
				if(userData is BirthIndexRowInfo rowInfo) {
					//get any discoveries associated to this button
					var discovery = rowInfo.GetDiscovery(button.name);

					//if there was one, make the discovery
					if(discovery != null) {
						AnalyticsManager.NewDiscovery(discovery.person, discovery.type, currentPage);
						SaveState.Instance.ChangeDiscovery(discovery);
					}
				}
			}
		}
	}
}