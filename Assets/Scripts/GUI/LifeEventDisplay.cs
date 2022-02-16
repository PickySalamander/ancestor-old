using System;
using Potterblatt.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class LifeEventDisplay : GamePage {
		public Label text;
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
						button.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void Awake() {
			text = RootElement.Q<Label>();
			button = RootElement.Q<Button>();
		}
	}
}