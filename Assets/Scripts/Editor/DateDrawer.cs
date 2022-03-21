using System;
using System.Globalization;
using JetBrains.Annotations;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;

namespace Potterblatt.Editor {
	[CustomPropertyDrawer(typeof(DateAttribute))]
	public class DateDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			
			var value = new DateTime(property.longValue);
			var currentString = value.ToString(DateUtils.DateTimeFormat);

			var str = EditorGUI.TextField(position, label, currentString);

			if(str != currentString) {
				Debug.Log($"New Value! {str}");
			}

			if(str != currentString && TryParse(str, out var newDate)) {
				property.serializedObject.Update();
				property.longValue = newDate.Ticks;
				property.serializedObject.ApplyModifiedProperties();
			}
			
			EditorGUI.EndProperty();
		}
		
		private static bool TryParse(string value, out DateTime dateTime) {
			return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal,
				out dateTime);
		}
	}
}