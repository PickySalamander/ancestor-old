using System;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Potterblatt.Editor {
	/// <summary>
	/// Helper editor script to create a random date in a year
	/// </summary>
	public class DateGenerator : EditorWindow {
		/// <summary>The year to generate the date in</summary>
		private int year = DateTime.Now.Year;

		/// <summary>The dates that were generated</summary>
		private string dates = "";

		private void OnGUI() {
			UnityEngine.GUI.skin.label.wordWrap = true;

			GUILayout.Label("Random Date Generator", EditorStyles.boldLabel);

			EditorGUILayout.Space();

			//get the year
			year = EditorGUILayout.IntField("Year to generate in", year);

			EditorGUILayout.Space();

			//generate a series of dates when button pressed
			if(GUILayout.Button("Generate")) {
				dates = "";
				
				for(var i = 0; i < 5; i++) {
					dates += GenerateDates() + "\n";
				}
			}
			
			EditorGUILayout.Space();

			//out put dates
			EditorGUILayout.TextArea(dates);
		}

		/// <summary>
		/// Generate a series of dates within a year
		/// </summary>
		/// <returns>a string of the date generated</returns>
		private string GenerateDates() {
			return DateUtils.RandomDateInYear(year).ToString(DateUtils.DateTimeFormat);
		}
		
		/// <summary>Quick open function to open the window</summary>
		[MenuItem("Ancestor/Date Generator")]
		public static void Open() {
			GetWindow<DateGenerator>(false, "Date Generator", true);
		}
	}
}