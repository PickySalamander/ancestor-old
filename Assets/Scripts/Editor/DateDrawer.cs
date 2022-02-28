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
			var value = property == null ? DateTime.Today.ToString(DateUtils.DateTimeFormat) : property.stringValue;

			var color = UnityEngine.GUI.color;

			if(!IsParsable(value)) {
				UnityEngine.GUI.color = Color.red;
			}
			
			EditorGUI.PropertyField(position, property, label);

			UnityEngine.GUI.color = color;
		}
		
		private static bool IsParsable(string value) {
			try {
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				DateTime.ParseExact(value, DateUtils.DateTimeFormat, CultureInfo.InvariantCulture);
			}
			catch(Exception) {
				return false;
			}

			return true;
		}
	}
}