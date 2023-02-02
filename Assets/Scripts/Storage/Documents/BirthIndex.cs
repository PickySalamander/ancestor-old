using Potterblatt.Storage.Documents;
using UnityEngine;

namespace Potterblatt.Storage {
	/// <summary>
	/// Storage for a birth index real or generated, can be saved in the project for a real person who is on the page
	/// </summary>
	[CreateAssetMenu(fileName = "Birth Index", menuName = "Ancestor/Create BirthIndex")]
	public class BirthIndex : Document {
		[Tooltip("The starting year on the index")]
		public int startYear;
		
		[Tooltip("The ending year on the index")]
		public int endYear;
		
		[Tooltip("The US state the document is for")]
		public string state;
		
		[Range(0, 1)]
		public float maidenNameFrequency = .2f;

		public override string Location => state;

		public override string FileName => $"{state} Birth Index {startYear}-{endYear}";

		/// <summary>
		/// Create a random birth index
		/// </summary>
		/// <param name="startYear">The starting year on the index</param>
		/// <param name="endYear">The ending year on the index</param>
		/// <param name="location">he US state the document is for</param>
		/// <returns>The new document</returns>
		public static BirthIndex CreateRandom(int startYear, int endYear, string location) {
			var random = CreateInstance<BirthIndex>();
			random.startYear = startYear;
			random.endYear = endYear;
			random.state = location;
			
			return random;
		}
	}
}