using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Controller for displaying the family tree page, which currently only has three people on it
	/// </summary>
	public class TreePage : GamePage {
		[Tooltip("The person who is at the root of the tree")]
		public Person familyRoot;

		[Tooltip("The template of a person on the tree")]
		public VisualTreeAsset template;

		private void OnEnable() {
			//instantiate each person once the save system is setup
			if(SaveState.IsSetup) {
				AddPerson(RootElement.Q<VisualElement>("main"), familyRoot);
				AddPerson(RootElement.Q<VisualElement>("father"), familyRoot.father);
				AddPerson(RootElement.Q<VisualElement>("mother"), familyRoot.mother);
			}
		}

		/// <summary>
		/// Add a person to the family tree
		/// </summary>
		/// <param name="location">The location to put them on the tree</param>
		/// <param name="person">The person to add to the tree</param>
		private void AddPerson(VisualElement location, Person person) {
			//clone the template
			var newElement = template.CloneTree();
			location.Add(newElement);

			//get the button for the template
			var button = newElement.Q<Button>();

			//if the person is undiscovered, show the undiscovered state
			if(person.HasNoDiscoveries()) {
				button.AddToClassList("undiscovered");
				button.RemoveFromClassList("discovered");
			}

			//otherwise, show the discovered state
			else {
				button.AddToClassList("discovered");
				button.RemoveFromClassList("undiscovered");

				//add click handler to open the info page
				button.clicked += () => { UIManager.Instance.OpenInfo(person); };

				//set there name
				newElement.Q<Label>("name").text = person.IsDiscovered(DiscoveryType.Name)
					? $"{person.firstName} {person.lastName}"
					: "?";

				//set the birth if found
				var born = person.IsDiscovered(DiscoveryType.Birth)
					? DateUtils.GetYear(person.Born)
					: "?";

				//set the death if found
				var death = person.IsDiscovered(DiscoveryType.Death)
					? DateUtils.GetYear(person.Death)
					: "?";

				//set the birth - death label
				newElement.Q<Label>("years").text = $"{born}-{death}";
			}
		}
	}
}