using System;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
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
			return DateUtils.RandomDateInYear(year).ToString(DateUtils.DateTimeFormat);
		}
		
		/// <summary>Quick open function to open the window</summary>
		[MenuItem("Ancestor/Date Generator")]
		public static void Open() {
			GetWindow<DateGenerator>(false, "Date Generator", true);
		}
	}
}