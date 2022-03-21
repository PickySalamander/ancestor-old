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
	public class BirthIndexPage : GamePage {
		private BirthIndex currentPage;
		private VisualTreeAsset rowTemplate;
		private VisualElement rowParent;

		private int numberOfPeople;
		private int templateHeight;

		private Label stateLabel;

		private Dictionary<VisualElement, BirthIndexRowInfo> lookup;

		private void OnEnable() {
			RootElement.RegisterCallback<GeometryChangedEvent>(OnAttached);
			stateLabel = RootElement.Q<Label>("state-label");

			templateHeight = 0;
			numberOfPeople = 0;
		}

		private void OnAttached(EventBase evt) {
			RootElement.UnregisterCallback<GeometryChangedEvent>(OnAttached);
			
			var template = RootElement.Q<TemplateContainer>("Birth Row");
			rowParent = template.parent;
			template.RemoveFromHierarchy();
			rowTemplate = template.templateSource;

			templateHeight = (int) Math.Ceiling(template.contentRect.height);
			var maxHeight = rowParent.contentRect.height;
			numberOfPeople = (int) Math.Ceiling(maxHeight / templateHeight);
		}

		public void Setup(Person person, BirthIndex index) {
			stateLabel.text = $"{index.state} State Board of Health".ToUpper();

			currentPage = index;

			StartCoroutine(WaitForSetup(person, index));
		}

		private IEnumerator WaitForSetup(Person person, BirthIndex index) {
			yield return new WaitUntil(() => numberOfPeople > 0);
			
			var newPeople = new SortedSet<BirthIndexRowInfo>();

			var faker = new Faker();
			
			for(var i = 0; i < numberOfPeople-1; i++) {
				var lastName = faker.Name.LastName();
				var maidenName = Random.Range(0f, 1f) <= index.maidenNameFrequency;
				var motherLastName = maidenName ? $"{faker.Name.LastName()} {lastName}" : lastName;
				
				newPeople.Add(new BirthIndexRowInfo {
					name = $"{faker.Name.FirstName()} {lastName}",
					dob = DateUtils.RandomDate(index.startYear, index.endYear),
					father = $"{faker.Name.FirstName(Name.Gender.Male)} {lastName}",
					mother = $"{faker.Name.FirstName(Name.Gender.Female)} {motherLastName}"
				});
			}
			
			newPeople.Add(new BirthIndexRowInfo(person));

			lookup = new Dictionary<VisualElement, BirthIndexRowInfo>();

			TemplateContainer newRow = null;
			foreach(var newPerson in newPeople) {
				newRow = rowTemplate.CloneTree();
				newRow.Q<TextElement>("date").text = newPerson.dob.ToString(DateUtils.IndexDateFormat);
				newRow.Q<TextElement>( "name").text = newPerson.name;
				
				newRow.Q<TextElement>( "father").text = newPerson.father;
				newRow.Q<TextElement>( "mother").text = newPerson.mother;
				rowParent.Add(newRow);
				newRow.userData = newPerson;

				lookup[newRow] = newPerson;
			}
			
			newRow?.Children().First().AddToClassList("last");
			
			rowParent.RegisterCallback<ClickEvent>(OnClick);
		}

		private void OnClick(ClickEvent evt) {
			if(evt.target is Button button) {
				var userData = button.FindAncestorUserData();
				if(userData is BirthIndexRowInfo rowInfo) {
					var discovery = rowInfo.GetDiscovery(button.name);

					if(discovery != null) {
						AnalyticsManager.NewDiscovery(discovery.person, discovery.type, currentPage);
						SaveState.Instance.ChangeDiscovery(discovery);
					}
				}
			}
		}
	}
}