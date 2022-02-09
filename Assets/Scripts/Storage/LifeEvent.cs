using System;
using System.Globalization;

namespace Potterblatt.Storage {
	[Serializable]
	public class LifeEvent {
		public const string DateTimeFormat = "M/d/yyyy h:mm tt";
		public const string DateFormat = "M/d/yyyy";
		
		public LifeEventType type;
		[DateAttribute]
		public string dateTime;
		public Document source;

		public DateTime Parsed => DateTime.ParseExact(dateTime, DateTimeFormat, CultureInfo.InvariantCulture);

		public int Year => Parsed.Year;

		public string DateString => Parsed.ToString(DateFormat);
		
		public string DateTimeString => Parsed.ToString(DateTimeFormat);
	}
}