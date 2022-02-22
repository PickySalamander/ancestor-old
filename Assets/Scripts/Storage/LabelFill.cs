using System;
using JetBrains.Annotations;

namespace Potterblatt.Storage {
	[AttributeUsage(System.AttributeTargets.Field)] 
	[MeansImplicitUse]
	public class LabelFill : Attribute {
		public readonly string nameOfLabel;

		public LabelFill(string nameOfLabel = null) {
			this.nameOfLabel = nameOfLabel;
		}
	}
}