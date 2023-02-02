using System;
using Potterblatt.Storage.People;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Controller of an event on the timeline on a person's info page
	/// </summary>
	/// <seealso cref="InfoPage"/>
	public class LifeEventDisplay : GamePage {
		/// <summary>Label of the event text</summary>
		private Label text;

		/// <summary>Open button to trigger the document related to open</summary>
		private Button button;

		/// <summary>Person this event is for</summary>
		private Person person;

		/// <summary>The event being shown</summary>
		private LifeEvent lifeEvent;

		private void Awake() {
			//get the ui elements
			text = RootElement.Q<Label>();
			button = RootElement.Q<Button>();
		}

		/// <summary>
		/// Setup the display
		/// </summary>
		/// <param name="person">Person this event is for</param>
		/// <param name="lifeEvent">The event being shown</param>
		/// <exception cref="ArgumentOutOfRangeException">for unsupported events</exception>
		public void Setup(Person person, LifeEvent lifeEvent) {
			this.person = person;
			this.lifeEvent = lifeEvent;

			bool discovered;

			switch(lifeEvent.type) {
				case LifeEventType.Birth:
					//text for birth if discovered
					discovered = person.IsDiscovered(DiscoveryType.Birth);
					text.text = discovered ? $"Was born on {lifeEvent.DateTimeString}" : "?";
					break;
				case LifeEventType.Death:
					//text for death if discovered
					discovered = person.IsDiscovered(DiscoveryType.Death);
					text.text = discovered ? $"Died on {lifeEvent.DateTimeString}" : "?";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			//if discovered then setup the button for opening the source document
			if(discovered && lifeEvent.source) {
				button.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
				button.clicked += OnClicked;
			}

			//otherwise hide the button
			else {
				button.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
				button.clicked -= OnClicked;
			}
		}

		/// <summary>
		/// User wants to open the source document
		/// </summary>
		private void OnClicked() {
			UIManager.Instance.OpenDoc(person, lifeEvent.source);
		}
	}
}