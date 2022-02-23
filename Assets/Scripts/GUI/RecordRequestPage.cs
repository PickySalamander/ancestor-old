using System.Collections.Generic;
using System.Linq;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class RecordRequestPage : GamePage {
		private Dictionary<string, Person> nameToPerson;
		private void OnEnable() {
			nameToPerson = 
				SaveState.Instance.Person.Values
					.Where(person => SaveState.Instance[person].HasFlag(DiscoveryType.Name))
					.ToDictionary(person => person.FullName);
			
			var dropdown = RootElement.Q<DropdownField>("name-list");
			dropdown.choices = nameToPerson.Keys.ToList();

			dropdown.RegisterValueChangedCallback(OnNewPerson);
		}

		private void OnNewPerson(ChangeEvent<string> evt) {
			Debug.Log($"New Person {evt.newValue}");
		}
	}
}