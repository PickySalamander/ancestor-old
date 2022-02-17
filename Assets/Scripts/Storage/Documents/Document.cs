using System;
using UnityEngine;

namespace Potterblatt.Storage.Documents {
	[Serializable]
	public abstract class Document : ScriptableObject {
		public string state;
	}
}