using System;
using System.Collections.Generic;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
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

		private void OnEnable() {
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

				fullName.text = person.IsDiscovered(DiscoveryType.Name) ? 
					$"{person.firstName} {person.lastName}" : "?";

				var born = person.Born;
				dob.text = person.IsDiscovered(DiscoveryType.Birth) ? born.DateString : "?";
				
				var death = person.Death;
				if(death == null) {
					dod.text = "Alive";
				} else if(person.IsDiscovered(DiscoveryType.Death)) {
					dod.text = death.DateString;
				}
				else {
					dod.text = "?";
				}

				start = born?.Parsed ?? DateTime.MinValue;
				var end = death?.Parsed ?? DateTime.Now;

				timeDuration = end - start;

				events = new List<LifeEventDisplay>();

				foreach(var lifeEvent in person.timeLine) {
					var newEvent = Instantiate(lifeEventTemplate, transform, false);
					newEvent.gameObject.name = $"{lifeEvent.type} - {lifeEvent.DateString}";
					newEvent.Setup(person, lifeEvent);
					
					var distance = lifeEvent.Parsed - start;
					var percent = distance.Ticks / timeDuration.Ticks;

					newEvent.RootElement.style.top = new StyleLength {
						value = new Length(percent * 100, LengthUnit.Percent)
					};
				
					events.Add(newEvent);
				}
			}
		}
	}
}