using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.Storage.Documents {
	/// <summary>
	/// Base of document storage
	/// </summary>
	[Serializable]
	public abstract class Document : ScriptableObject {
		/// <summary>The US state this document is for</summary>
		public abstract string Location { get; }
		
		
		/// <summary>The name of this document</summary>
		public abstract string FileName { get; }
		
		/// <summary>
		/// Helper function that works with the <see cref="LabelFill"/> attribute to automatically populate labels in
		/// a <see cref="VisualElement"/>.
		/// </summary>
		/// <param name="elementToFill">The element to get the labels for and fill out</param>
		public void FillLabels(VisualElement elementToFill) {
			//go through each field in the doc reflectively
			foreach(var field in GetType().GetFields()) {
				//get the attribute
				var labelFill = (LabelFill) Attribute.GetCustomAttribute(field, typeof(LabelFill));

				//if it has the attribute try and populate the label with the value
				if(labelFill != null) {
					//get the name of the label, if it was set
					var nameOfLabel = labelFill.nameOfLabel ?? field.Name;

					//get the label
					var label = elementToFill.Q<TextElement>(nameOfLabel);
					if(label == null) {
						Debug.LogWarning($"Failed to fill label \"{nameOfLabel}\"");
					}
					else {
						//set the text if it was found
						label.text = field.GetValue(this)?.ToString();
					}
				}
			}
		}
	}
}