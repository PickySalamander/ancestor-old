using System;
using System.Collections;
using System.Collections.Generic;
using Potterblatt.Storage;
using TMPro;
using UnityEngine;

namespace Potterblatt.GUI {
	public class InfoPage : MonoBehaviour {
		public Person person;
		public RectTransform timelineDisplay;
		public LifeEventDisplay eventPrefab;

		public TMP_Text fullName;
		public TMP_Text dob;
		public TMP_Text dod;

		private DateTime start;
		private TimeSpan timeDuration;
		private List<LifeEventDisplay> events;

		private void OnEnable() {
			if(events != null) {
				foreach(var lifeEvent in events) {
					Destroy(lifeEvent.gameObject);
				}

				events = new List<LifeEventDisplay>();
			}
			
			if(person) {
				fullName.text = $"Full Name: {person.firstName} {person.lastName}";

				var born = person.Born;
				dob.text = $"Date of Birth: {(born == null ? "?" : born.DateString)}";
				
				var death = person.Death;
				dod.text = $"Date of Death: {(death == null ? "?" : death.DateString)}";
				
				start = born?.Parsed ?? DateTime.MinValue;
				var end = death?.Parsed ?? DateTime.Now;
				
				timeDuration = end - start;

				StartCoroutine(BuildTimeLine());
			}
		}

		private IEnumerator BuildTimeLine() {
			yield return new WaitForSeconds(1);

			var height = timelineDisplay.rect.height;
			Debug.Log($"Height: {height}");

			//TODO get the bounds of points for eventHeight
			foreach(var lifeEvent in person.timeLine) {
				var newEvent = Instantiate(eventPrefab, timelineDisplay, false);
				newEvent.gameObject.name = $"{lifeEvent.type} - {lifeEvent.DateString}";
				newEvent.Setup(lifeEvent);

				var distance = lifeEvent.Parsed - start;
				var percent = distance.Ticks / timeDuration.Ticks;
				var pos = percent * height;

				var eventTransform = (RectTransform) newEvent.transform;
				var eventHeight = eventTransform.rect.height / 2;
				eventTransform.anchorMin = new Vector2(eventTransform.anchorMin.x, 1);
				eventTransform.anchorMax = new Vector2(eventTransform.anchorMax.x, 1);
				eventTransform.anchoredPosition = new Vector2(eventTransform.anchoredPosition.x, -pos + eventHeight);
				
				Debug.Log($"Event dis:{distance} perc:{percent} pos:{pos} height:{eventTransform.rect.height}");
			}
		}
	}
}