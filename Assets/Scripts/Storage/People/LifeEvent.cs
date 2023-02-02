using System;
using Potterblatt.Storage.Documents;
using Potterblatt.Utils;
using UnityEngine;

namespace Potterblatt.Storage.People {
	/// <summary>
	/// Storage of an event that occurs in person's life
	/// </summary>
	[Serializable]
	public class LifeEvent {
		[Tooltip("The type of event that occurred")]
		public LifeEventType type;

		[Tooltip("The time the event occurred")] [Date]
		public long dateTime;

		[Tooltip("The document that the event can be discovered in")]
		public Document source;

		/// <summary>Get the date and time from the long stored value</summary>
		public DateTime Parsed => new(dateTime);

		/// <summary>Get the year from the parsed date</summary>
		public int Year => Parsed.Year;

		/// <summary>Get the date string</summary>
		public string DateString => Parsed.ToString(DateUtils.StandardDateFormat);

		/// <summary>Get the date time string</summary>
		public string DateTimeString => Parsed.ToString(DateUtils.DateTimeFormat);
	}
}