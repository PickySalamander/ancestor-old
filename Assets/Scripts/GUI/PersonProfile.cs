using System;
using Potterblatt.Storage;
using TMPro;
using UnityEngine;

namespace Potterblatt.GUI {
	public class PersonProfile : MonoBehaviour {
		public bool visible;
		public Person person;

		public TMP_Text name;

		private void OnEnable() {
			SetName();
		}

		[ContextMenu("Set Name")]
		private void SetName() {
			name.text = $"{person.firstName} {person.lastName}\n{person.Born}-{person.Death}";
		}
	}
}