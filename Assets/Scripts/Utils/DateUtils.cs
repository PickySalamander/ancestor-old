using Potterblatt.Storage;

namespace Potterblatt.Utils {
	public static class DateUtils {
		public static string GetYear(LifeEvent lifeEvent, string unknown = "?") {
			return lifeEvent == null ? unknown : lifeEvent.Year.ToString();
		}
		
		public static string GetDate(LifeEvent lifeEvent, string unknown = "?") {
			return lifeEvent == null ? unknown : lifeEvent.DateString;
		}
	}
}