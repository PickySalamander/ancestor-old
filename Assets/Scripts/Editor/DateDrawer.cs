using System;
using System.Globalization;
using JetBrains.Annotations;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;

namespace Potterblatt.Editor {
	/// <summary>
	/// Helper editor property for showing and converting a date from  and from a string in the editor to a long. Unity
	/// doesn't have a way to currently serialize dates.
	/// </summary>
	[CustomPropertyDrawer(typeof(DateAttribute))]
	public class DateDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			
			//get the date value and convert it to a string
			var value = new DateTime(property.longValue);
			var currentString = value.ToString(DateUtils.DateTimeFormat);

			//put the current string in the text field
			var str = EditorGUI.TextField(position, label, currentString);

			//if the string is different and can be parsed put the new value into the property
			if(str != currentString && TryParse(str, out var newDate)) {
				property.serializedObject.Update();
				property.longValue = newDate.Ticks;
				property.serializedObject.ApplyModifiedProperties();
			}
			
			EditorGUI.EndProperty();
		}
		
		/// <summary>
		/// Helper function to try and parse a string to a date
		/// </summary>
		/// <param name="value">The value to parse</param>
		/// <param name="dateTime">The output of the parsing it it worked</param>
		/// <returns>True is the parse was successful, false if otherwise</returns>
		private static bool TryParse(string value, out DateTime dateTime) {
			return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal,
				out dateTime);
		}
	}
}