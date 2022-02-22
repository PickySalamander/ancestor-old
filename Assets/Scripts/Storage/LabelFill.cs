using System;

namespace Potterblatt.Storage {
	[AttributeUsage(System.AttributeTargets.Field)] 
	public class LabelFill : Attribute {
		public readonly string nameOfLabel;

		public LabelFill(string nameOfLabel = null) {
			this.nameOfLabel = nameOfLabel;
		}
	}
}