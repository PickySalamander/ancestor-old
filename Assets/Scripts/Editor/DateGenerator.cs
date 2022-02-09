using System;
using Potterblatt.Storage;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Potterblatt.Editor {
	public class DateGenerator : EditorWindow {
		private Random random = new Random();

		private int year = DateTime.Now.Year;

		private string dates = "";

		private void OnGUI() {
			UnityEngine.GUI.skin.label.wordWrap = true;

			GUILayout.Label("Random Date Generator", EditorStyles.boldLabel);

			EditorGUILayout.Space();

			year = EditorGUILayout.IntField("Year to generate in", year);

			EditorGUILayout.Space();

			//export the animation to the asset database
			if(GUILayout.Button("Generate")) {
				dates = "";
				
				for(var i = 0; i < 5; i++) {
					dates += GenerateDates() + "\n";
				}
			}
			
			EditorGUILayout.Space();

			EditorGUILayout.TextArea(dates);
		}

		private string GenerateDates() {
			var month = random.Next(1, 13);
			var day = DateTime.DaysInMonth(year, month);
			var hour = random.Next(0, 24);
			var minute = random.Next(0, 60);

			try {
				var date = new DateTime(year, month, day, hour, minute, 0);
				return date.ToString(LifeEvent.DateTimeFormat);
			}
			catch(ArgumentOutOfRangeException) {
				Debug.LogError($"Failed to generate date {year}/{month}/{day} {hour}:{minute},");
			}

			return "error";
		}
		
		/// <summary>Quick open function to open the window</summary>
		[MenuItem("Ancestor/Date Generator")]
		public static void Open() {
			GetWindow<DateGenerator>(false, "Date Generator", true);
		}
	}
}