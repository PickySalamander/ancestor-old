using System;
using Potterblatt.Storage;
using Potterblatt.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Potterblatt.GUI {
	public class PersonProfile : MonoBehaviour {
		public bool visible;
		public Person person;
		public Button button;
		public TMP_Text nameLabel;

		private void Reset() {
			if(button == null) {
				button = GetComponentInChildren<Button>();
			}
		}

		private void Start() {
			button.onClick.AddListener(OnClick);
		}

		private void OnEnable() {
			SetName();
		}

		[ContextMenu("Set Name")]
		private void SetName() {
			nameLabel.text = $"{person.firstName} {person.lastName}\n{DateUtils.GetYear(person.Born)}-{DateUtils.GetYear(person.Death)}";
		}

		private void OnClick() {
			PageManager.Instance.OpenInfo(person);
		}
	}
}