using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potterblatt.Storage.People {
	[CreateAssetMenu(fileName = "Random Names", menuName = "Ancestor/Create Random Names")]
	public class RandomNames : ScriptableObject {
		public List<string> maleNames;
		public List<string> femaleNames;
		public List<string> lastNames;

		public List<string> streets;
		public List<string> counties;
		public List<string> towns;

		public string GetFirstName() {
			var isFemale = Random.Range(0, 2) == 1;
			return GetFirstName(isFemale);
		}
		
		public string GetFirstName(bool isFemale) {
			var list = isFemale ? femaleNames : maleNames;
			return list[Random.Range(0, list.Count)];
		}
		
		public string GetLastName() {
			return lastNames[Random.Range(0, lastNames.Count)];
		}

		public string GetStreet() {
			return streets[Random.Range(0, streets.Count)];
		}
		
		public string GetTown() {
			return towns[Random.Range(0, towns.Count)];
		}
		
		public string GetCounty() {
			return counties[Random.Range(0, counties.Count)];
		}
	}
}