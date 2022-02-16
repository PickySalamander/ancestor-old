using System;
using System.Collections.Generic;
using Potterblatt.Storage;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class InfoPage : GamePage {
		public LifeEventDisplay lifeEventTemplate;
		
		private Person person;

		private Label fullName;
		private Label dob;
		private Label dod;

		private DateTime start;
		private TimeSpan timeDuration;
		private List<LifeEventDisplay> events;

		private void Awake() {
			fullName = RootElement.Q<Label>("full-name");
			dob = RootElement.Q<Label>("dob");
			dod = RootElement.Q<Label>("dod");
		}

		public Person Person {
			get => person;
			set {
				person = value;
				
				if(events != null) {
					foreach(var lifeEvent in events) {
						Destroy(lifeEvent.gameObject);
					}
				}

				fullName.text = $"Full Name: {person.firstName} {person.lastName}";

				var born = person.Born;
				dob.text = $"Date of Birth: {(born == null ? "?" : born.DateString)}";

				var death = person.Death;
				dod.text = $"Date of Death: {(death == null ? "?" : death.DateString)}";

				start = born?.Parsed ?? DateTime.MinValue;
				var end = death?.Parsed ?? DateTime.Now;

				timeDuration = end - start;

				events = new List<LifeEventDisplay>();

				foreach(var lifeEvent in person.timeLine) {
					var newEvent = Instantiate(lifeEventTemplate, transform, false);
					newEvent.gameObject.name = $"{lifeEvent.type} - {lifeEvent.DateString}";
					newEvent.LifeEvent = lifeEvent;
					
					var distance = newEvent.LifeEvent.Parsed - start;
					var percent = distance.Ticks / timeDuration.Ticks;

					newEvent.RootElement.style.top = new StyleLength {
						value = new Length(percent * 100, LengthUnit.Percent)
					};

					newEvent.button.clicked += () => {
						UIManager.Instance.OpenBirth(person);
					};
				
					events.Add(newEvent);
				}
			}
		}
	}
}