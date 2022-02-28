using System;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potterblatt.Utils {
	public static class DateUtils {
		public const string DateTimeFormat = "M/d/yyyy h:mm tt";
		public const string StandardDateFormat = "M/d/yyyy";
		public const string IndexDateFormat = "MMM d yyyy";
		
		public static string GetYear(LifeEvent lifeEvent, string unknown = "?") {
			return lifeEvent == null ? unknown : lifeEvent.Year.ToString();
		}

		public static string GetDate(LifeEvent lifeEvent, string unknown = "?") {
			return lifeEvent == null ? unknown : lifeEvent.DateString;
		}

		public static DateTime RandomDateInYear(int year) {
			return RandomDate(year, year);
		}
		
		public static DateTime RandomDate(int startYear, int endYear) {
			var year = startYear == endYear ? startYear : Random.Range(startYear, endYear);
			var month = Random.Range(1, 13);
			var day = DateTime.DaysInMonth(year, month);
			var hour = Random.Range(0, 24);
			var minute = Random.Range(0, 60);

			return new DateTime(year, month, day, hour, minute, 0);
		}

		
	}
}