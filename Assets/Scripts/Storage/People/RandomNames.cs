using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potterblatt.Storage.People {
	[CreateAssetMenu(fileName = "Random Names", menuName = "Ancestor/Create Random Names")]
	public class RandomNames : ScriptableObject {
		public List<string> maleNames;
		public List<string> femaleNames;

		public string GetNext() {
			var isFemale = Random.Range(0, 2) == 1;
			return GetNext(isFemale);
		}
		
		public string GetNext(bool isFemale) {
			var list = isFemale ? femaleNames : maleNames;
			return list[Random.Range(0, list.Count)];
		}
	}
}