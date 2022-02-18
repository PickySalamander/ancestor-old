using System;
using Potterblatt.Storage.People;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class LifeEventDisplay : GamePage {
		private Label text;
		private Button button;

		private Person person;
		private LifeEvent lifeEvent;

		private void Awake() {
			text = RootElement.Q<Label>();
			button = RootElement.Q<Button>();
		}

		public void Setup(Person person, LifeEvent lifeEvent) {
			this.person = person;
			this.lifeEvent = lifeEvent;
			
			bool discovered;
			
			switch(lifeEvent.type) {
				case LifeEventType.Birth:
					discovered = person.IsDiscovered(DiscoveryType.Birth);
					text.text = discovered ? $"Was born on {lifeEvent.dateTime}" : "?";
					break;
				case LifeEventType.Death:
					discovered = person.IsDiscovered(DiscoveryType.Death);
					text.text = discovered ? $"Died on {lifeEvent.dateTime}" : "?";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if(discovered && lifeEvent.source) {
				button.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
				button.clicked += OnClicked;
			}
			else {
				button.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
				button.clicked -= OnClicked;
			}
		}

		private void OnClicked() {
			UIManager.Instance.OpenDoc(person, lifeEvent.source);
		}
	}
}