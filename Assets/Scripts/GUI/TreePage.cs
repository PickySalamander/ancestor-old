using System;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class TreePage : GamePage {
		public Person familyRoot;

		public VisualTreeAsset template;

		private void OnEnable() {
			if(SaveState.IsSetup) {
				AddPerson(RootElement.Q<VisualElement>("main"), familyRoot);
				AddPerson(RootElement.Q<VisualElement>("father"), familyRoot.father);
				AddPerson(RootElement.Q<VisualElement>("mother"), familyRoot.mother);
			}
		}

		private void AddPerson(VisualElement location, Person person) {
			var newElement = template.CloneTree();
			location.Add(newElement);

			var button = newElement.Q<Button>();
			if(person.HasNoDiscoveries()) {
				button.AddToClassList("undiscovered");
				button.RemoveFromClassList("discovered");
			}
			else {
				button.AddToClassList("discovered");
				button.RemoveFromClassList("undiscovered");
				
				button.clicked += () => {
					UIManager.Instance.OpenInfo(person);
				};
				
				newElement.Q<Label>("name").text = person.IsDiscovered(DiscoveryType.Name) ? 
					$"{person.firstName} {person.lastName}" : "?";

				var born = person.IsDiscovered(DiscoveryType.Birth) 
					? DateUtils.GetYear(person.Born) : "?";
				
				var death = person.IsDiscovered(DiscoveryType.Death) 
					? DateUtils.GetYear(person.Death) : "?";
				newElement.Q<Label>("years").text = $"{born}-{death}";
			}
		}
	}
}