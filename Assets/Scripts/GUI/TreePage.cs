using System;
using Potterblatt.Storage;
using Potterblatt.Utils;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class TreePage : GamePage {
		public Person familyRoot;

		public VisualTreeAsset template;

		private void Start() {
			AddPerson(RootElement.Q<VisualElement>("main"), familyRoot);
			AddPerson(RootElement.Q<VisualElement>("father"), familyRoot.father);
			AddPerson(RootElement.Q<VisualElement>("mother"), familyRoot.mother);
		}

		private void AddPerson(VisualElement location, Person person) {
			var newElement = template.CloneTree();
			location.Add(newElement);

			var button = newElement.Q<Button>();
			button.clicked += () => {
				UIManager.Instance.OpenInfo(person);
			};

			newElement.Q<Label>("name").text = $"{person.firstName} {person.lastName}";
			newElement.Q<Label>("years").text = $"{DateUtils.GetYear(person.Born)}-{DateUtils.GetYear(person.Death)}";
		}
	}
}