using System;
using System.Globalization;
using Potterblatt.Storage.Documents;
using Potterblatt.Utils;

namespace Potterblatt.Storage.People {
	[Serializable]
	public class LifeEvent {
		public LifeEventType type;
		[Date]
		public string dateTime;
		public Document source;

		public DateTime Parsed => DateTime.ParseExact(dateTime, DateUtils.DateTimeFormat, CultureInfo.InvariantCulture);

		public int Year => Parsed.Year;

		public string DateString => Parsed.ToString(DateUtils.StandardDateFormat);
		
		public string DateTimeString => Parsed.ToString(DateUtils.DateTimeFormat);
	}
}