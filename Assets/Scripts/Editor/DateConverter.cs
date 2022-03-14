using System;
using System.Globalization;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Potterblatt.Editor {
	public class DateConverter : EditorWindow {
		private string dateToConvert;
		private string dateConverted;

		private void OnGUI() {
			UnityEngine.GUI.skin.label.wordWrap = true;

			GUILayout.Label("Date Converter", EditorStyles.boldLabel);

			EditorGUILayout.Space();

			dateToConvert = EditorGUILayout.TextField("To Convert", dateToConvert);

			EditorGUILayout.Space();

			//export the animation to the asset database
			if(GUILayout.Button("Convert")) {
				var date = DateTime.Parse(dateToConvert, CultureInfo.InvariantCulture);
				date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
				dateConverted = date.ToString("yyyy-MM-ddTHH:mm:ssZ");
			}
			
			EditorGUILayout.Space();

			EditorGUILayout.TextField(dateConverted);
		}
		
		/// <summary>Quick open function to open the window</summary>
		[MenuItem("Ancestor/Date Converter")]
		public static void Open() {
			GetWindow<DateConverter>(false, "Date Converter", true);
		}
	}
}