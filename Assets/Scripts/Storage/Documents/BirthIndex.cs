﻿using Potterblatt.Storage.Documents;
using UnityEngine;

namespace Potterblatt.Storage {
	[CreateAssetMenu(fileName = "Birth Index", menuName = "Ancestor/Create BirthIndex")]
	public class BirthIndex : Document {
		public int startYear;
		public int endYear;
	}
}