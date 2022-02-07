using System;

namespace Potterblatt.Storage {
	[Serializable]
	public class LifeEvent {
		public LifeEventType type;
		[DateAttribute]
		public string dateTime;
		public Document source;
	}
}