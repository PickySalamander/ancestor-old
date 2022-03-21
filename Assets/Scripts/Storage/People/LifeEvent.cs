using System;
using Potterblatt.Storage.Documents;
using Potterblatt.Utils;

namespace Potterblatt.Storage.People {
	[Serializable]
	public class LifeEvent {
		public LifeEventType type;
		
		[Date]
		public long dateTime;
		
		public Document source;

		public DateTime Parsed => new(dateTime);

		public int Year => Parsed.Year;

		public string DateString => Parsed.ToString(DateUtils.StandardDateFormat);
		
		public string DateTimeString => Parsed.ToString(DateUtils.DateTimeFormat);
	}
}