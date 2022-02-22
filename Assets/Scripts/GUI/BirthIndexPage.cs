using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Potterblatt.GUI {
	public partial class BirthIndexPage : GamePage {
		public string dateFormat = "MMM d yyyy";
		public RandomNames randomNames;

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

			StartCoroutine(WaitForSetup(person, index));
		}

		private IEnumerator WaitForSetup(Person person, BirthIndex index) {
			yield return new WaitUntil(() => numberOfPeople > 0);
			
			var newPeople = new SortedSet<BirthIndexRowInfo>();
			
			for(var i = 0; i < numberOfPeople-1; i++) {
				var lastName = randomNames.GetLastName();
				var maidenName = Random.Range(0f, 1f) <= index.maidenNameFrequency;
				
				newPeople.Add(new BirthIndexRowInfo {
					name = $"{randomNames.GetFirstName()} {lastName}",
					dob = DateUtils.RandomDate(index.startYear, index.endYear),
					father = $"{randomNames.GetFirstName(false)} {lastName}",
					mother = maidenName ? 
						$"{randomNames.GetFirstName(true)} {randomNames.GetLastName()} {lastName}" : 
						$"{randomNames.GetFirstName()} {lastName}"
				});
			}

			const DiscoveryType discovery = DiscoveryType.Name | DiscoveryType.Death;
			
			newPeople.Add(new BirthIndexRowInfo {
				name = person.FullName,
				dob = person.Born.Parsed,
				father = person.father.FullName,
				mother = person.mother.FullName,
				discoverFather = new Discovery {
					person = person.father,
					type = discovery
				},
				discoverMother = new Discovery {
					person = person.mother,
					type = discovery
				}
			});

			lookup = new Dictionary<VisualElement, BirthIndexRowInfo>();

			TemplateContainer newRow = null;
			foreach(var newPerson in newPeople) {
				newRow = rowTemplate.CloneTree();
				newRow.Q<TextElement>("date").text = newPerson.dob.ToString(dateFormat);
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
				var classToAdd = "wrong";
				var userData = button.FindAncestorUserData();
				if(userData is BirthIndexRowInfo rowInfo && rowInfo.IsDiscoverable()) {
					switch(button.name) {
						case "mother" when rowInfo.IsDiscoverable(true):
							SaveState.Instance.ChangeDiscovery(rowInfo.discoverMother);
							classToAdd = "right";
							break;
						case "father" when rowInfo.IsDiscoverable(false):
							SaveState.Instance.ChangeDiscovery(rowInfo.discoverFather);
							classToAdd = "right";
							break;
					}
				}
				
				button.AddToClassList(classToAdd);
				button.pickingMode = PickingMode.Ignore;

				StartCoroutine(RemoveClass(button));
			}
		}

		private static IEnumerator RemoveClass(VisualElement button) {
			yield return new WaitForSeconds(3.0f);
			
			button.RemoveFromClassList("right");
			button.RemoveFromClassList("wrong");
			button.pickingMode = PickingMode.Position;
		}
	}
}