using Potterblatt.Storage.Documents;
using UnityEngine;

namespace Potterblatt.Storage {
	[CreateAssetMenu(fileName = "Birth Index", menuName = "Ancestor/Create BirthIndex")]
	public class BirthIndex : Document {
		public int startYear;
		public int endYear;
		public string state;
		
		[Range(0, 1)]
		public float maidenNameFrequency = .2f;
	}
}