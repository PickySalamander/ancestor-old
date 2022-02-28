using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.Storage.Documents {
	[Serializable]
	public abstract class Document : ScriptableObject {
		public abstract string Location { get; }
		
		public abstract string FileName { get; }
		
		public void FillLabels(VisualElement elementToFill) {
			foreach(var field in GetType().GetFields()) {
				var labelFill = (LabelFill) Attribute.GetCustomAttribute(field, typeof(LabelFill));

				if(labelFill != null) {
					var nameOfLabel = labelFill.nameOfLabel ?? field.Name;

					var label = elementToFill.Q<Label>(nameOfLabel);
					if(label == null) {
						Debug.LogWarning($"Failed to fill label \"{nameOfLabel}\"");
					}
					else {
						label.text = field.GetValue(this)?.ToString();
					}
				}
			}
		}
	}
}