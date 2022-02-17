using System;
using System.Collections.Generic;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class BirthIndexPage : GamePage {
		public struct BirthIndexRowInfo : IComparable<BirthIndexRowInfo> {
			public string name;
			public DateTime dob;
			public string mother;
			public string father;
			
			public int CompareTo(BirthIndexRowInfo other) {
				var sort = dob.CompareTo(other.dob);
				return sort != 0 ? sort : 
					string.Compare(name, other.name, StringComparison.InvariantCultureIgnoreCase);
			}
		}

		public string dateFormat = "MMM/d/yyyy";
		public RandomNames randomNames;
		public int numberOfPeople = 32;
		
		private VisualTreeAsset rowTemplate;
		private VisualElement rowParent;

		private Label stateLabel;

		private void Awake() {
			stateLabel = RootElement.Q<Label>("state-label");
			var template = RootElement.Q<TemplateContainer>("Birth Row");
			rowParent = template.parent;
			template.RemoveFromHierarchy();
			rowTemplate = template.templateSource;
		}

		public void Setup(Person person, BirthIndex index) {
			stateLabel.text = $"{index.state} State Board of Health".ToUpper();

			var newPeople = new SortedSet<BirthIndexRowInfo>();

			for(var i = 0; i < numberOfPeople-1; i++) {
				newPeople.Add(new BirthIndexRowInfo {
					name = randomNames.GetNext(),
					dob = DateUtils.RandomDate(index.startYear, index.endYear),
					father = randomNames.GetNext(false),
					mother = randomNames.GetNext(true)
				});
			}

			newPeople.Add(new BirthIndexRowInfo {
				name = person.FullName,
				dob = person.Born.Parsed,
				father = person.father.FullName,
				mother = person.mother.FullName
			});

			foreach(var newPerson in newPeople) {
				var newRow = rowTemplate.CloneTree();
				newRow.Q<Label>("date").text = newPerson.dob.ToString(dateFormat);
				newRow.Q<Label>( "name").text = newPerson.name;
				newRow.Q<Label>( "father").text = newPerson.father;
				newRow.Q<Label>( "mother").text = newPerson.mother;
				rowParent.Add(newRow);
			}
		}
	}
}