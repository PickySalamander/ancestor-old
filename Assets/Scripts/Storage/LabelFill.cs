using System;
using JetBrains.Annotations;
using Potterblatt.Storage.Documents;
using UnityEngine.UIElements;

namespace Potterblatt.Storage {
	/// <summary>
	/// Helper attribute for designating a field on a class as the value of a <see cref="Label"/> in a
	/// <see cref="UIDocument"/>.
	/// <seealso cref="Document.FillLabels"/>
	/// </summary>
	[AttributeUsage(System.AttributeTargets.Field)] 
	[MeansImplicitUse]
	public class LabelFill : Attribute {
		/// <summary>Optional name the label, otherwise the name of the field will be it</summary>
		public readonly string nameOfLabel;

		/// <summary>
		/// Create with the optional name of the label
		/// </summary>
		/// <param name="nameOfLabel">Optional name the label, otherwise the name of the field will be it</param>
		public LabelFill(string nameOfLabel = null) {
			this.nameOfLabel = nameOfLabel;
		}
	}
}