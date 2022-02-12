using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potterblatt.Utils {
	[CreateAssetMenu(fileName = "Random Names", menuName = "Ancestor/Create Random Names")]
	public class RandomNames : ScriptableObject {
		[Serializable]
		public struct RandomPerson {
			public string fullName;
			public string gender;
		}

		public List<RandomPerson> people;
	}
}