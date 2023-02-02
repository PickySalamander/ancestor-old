using System;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potterblatt.Utils {
	/// <summary>
	/// Date parsing utilities
	/// </summary>
	public static class DateUtils {
		/// <summary>Format of date time</summary>
		public const string DateTimeFormat = "M/d/yyyy h:mm tt";
		
		/// <summary>Default format of dates</summary>
		public const string StandardDateFormat = "M/d/yyyy";
		
		/// <summary>Format of dates on index documents, like the birth index</summary>
		public const string IndexDateFormat = "MMM d yyyy";
		
		/// <summary>
		/// Get the year of an event 
		/// </summary>
		/// <param name="lifeEvent">Event to get the year from</param>
		/// <param name="unknown">default string to return if the event wasn't found</param>
		/// <returns>string of the year or unknown if not found</returns>
		public static string GetYear(LifeEvent lifeEvent, string unknown = "?") {
			return lifeEvent == null ? unknown : lifeEvent.Year.ToString();
		}

		/// <summary>
		/// Get the full date of an event
		/// </summary>
		/// <param name="lifeEvent">Event to get the date from</param>
		/// <param name="unknown">default string to return if the event wasn't found</param>
		/// <returns>string of the date or unknown if not found</returns>
		public static string GetDate(LifeEvent lifeEvent, string unknown = "?") {
			return lifeEvent == null ? unknown : lifeEvent.DateString;
		}

		/// <summary>
		/// Get a random date time within a year
		/// </summary>
		/// <param name="year">the year to get the date from</param>
		/// <returns>Date and time in the year</returns>
		public static DateTime RandomDateInYear(int year) {
			return RandomDate(year, year);
		}
		
		/// <summary>
		/// Get a random date with a year range
		/// </summary>
		/// <param name="startYear">inclusive start year</param>
		/// <param name="endYear">exclusive end year</param>
		/// <returns>The random date and time in the start and end years</returns>
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