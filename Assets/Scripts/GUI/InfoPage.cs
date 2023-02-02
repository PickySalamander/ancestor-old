using System;
using System.Collections.Generic;
using Potterblatt.Storage.People;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Controller for a person's info page that describes information about them
	/// </summary>
	/// <seealso cref="LifeEventDisplay"/>
	public class InfoPage : GamePage {
		[Tooltip("Template for an event to show on the timeline")]
		public LifeEventDisplay lifeEventTemplate;

		/// <summary>The person this page is for</summary>
		private Person person;

		/// <summary>Label for the person's full name</summary>
		private Label fullName;

		/// <summary>Label for the person's date of birth</summary>
		private Label dob;

		/// <summary>Label for the person's date of death</summary>
		private Label dod;

		/// <summary>Start of the timeline</summary>
		private DateTime start;

		/// <summary>Length of the timeline</summary>
		private TimeSpan timeDuration;

		/// <summary>Events that happend on the timeline</summary>
		private List<LifeEventDisplay> events;

		private void OnEnable() {
			//setup the initial
			fullName = RootElement.Q<Label>("full-name");
			dob = RootElement.Q<Label>("dob");
			dod = RootElement.Q<Label>("dod");
		}

		/// <summary>The person this page is for, setting will repopulate the page</summary>
		public Person Person {
			get => person;
			set {
				person = value;

				//destroy previous events on the timeline
				if(events != null) {
					foreach(var lifeEvent in events) {
						Destroy(lifeEvent.gameObject);
					}
				}

				//set full name if discovered
				fullName.text = person.IsDiscovered(DiscoveryType.Name) ? $"{person.firstName} {person.lastName}" : "?";

				//set birth if discovered
				var born = person.Born;
				dob.text = person.IsDiscovered(DiscoveryType.Birth) ? born.DateString : "?";

				var death = person.Death;
				if(death == null) {
					//set that the person is alive if they didn't die
					dod.text = "Alive";
				}
				else if(person.IsDiscovered(DiscoveryType.Death)) {
					//set death if discovered
					dod.text = death.DateString;
				}
				else {
					//set unknown death
					dod.text = "?";
				}

				//get the timeline start
				start = born?.Parsed ?? DateTime.MinValue;
				var end = death?.Parsed ?? DateTime.Now;

				//get the duration of the timeline
				timeDuration = end - start;

				//catalog all discovered events on the timeline
				events = new List<LifeEventDisplay>();
				foreach(var lifeEvent in person.timeLine) {
					//create a new event template and populate it
					var newEvent = Instantiate(lifeEventTemplate, transform, false);
					newEvent.gameObject.name = $"{lifeEvent.type} - {lifeEvent.DateString}";
					newEvent.Setup(person, lifeEvent);

					//get it's position
					var distance = lifeEvent.Parsed - start;
					var percent = distance.Ticks / timeDuration.Ticks;

					//set the position on the timeline
					newEvent.RootElement.style.top = new StyleLength {
						value = new Length(percent * 100, LengthUnit.Percent)
					};

					//add to the list
					events.Add(newEvent);
				}
			}
		}
	}
}