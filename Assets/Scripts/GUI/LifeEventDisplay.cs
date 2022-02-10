using System;
using Potterblatt.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Potterblatt.GUI {
	public class LifeEventDisplay : MonoBehaviour {
		public TMP_Text text;
		public Button button;

		private LifeEvent lifeEvent;

		public LifeEvent LifeEvent {
			get => lifeEvent;

			set {
				lifeEvent = value;
				
				switch(lifeEvent.type) {
					case LifeEventType.Birth:
						text.text = $"Was born on {lifeEvent.dateTime}";
						break;
					case LifeEventType.Death:
						text.text = $"Died on {lifeEvent.dateTime}";
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void Reset() {
			if(!text) {
				text = GetComponentInChildren<TMP_Text>();
			}

			if(!button) {
				button = GetComponentInChildren<Button>();
			}
		}
	}
}