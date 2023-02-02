using System;
using Bogus;
using Bogus.DataSets;
using Potterblatt.Utils;
using UnityEngine;

namespace Potterblatt.Storage.Documents {
	/// <summary>
	/// Storage for a death certificate real or generated, can be saved in the project for a real person who is on the
	/// page
	/// </summary>
	[CreateAssetMenu(fileName = "Death Cert", menuName = "Ancestor/Create Death Certificate")]
	public class DeathCert : Document {
		[Header("Basic Info")] [LabelFill] public string county;

		[LabelFill] public string state;

		[LabelFill] public string town;

		[LabelFill] public string address;

		[Header("Basic Info Pt 2")] [LabelFill]
		public string residence;

		[LabelFill] public string lengthOfResidence;

		[LabelFill] public string foreignBirth;

		[Header("Personal Info")] [LabelFill] public string sex;

		[LabelFill] public string race;

		[LabelFill] public string maritalStatus;

		[LabelFill] public string spouse;

		[LabelFill] public string occupation;

		[LabelFill] public string employer;

		[LabelFill] public string birthPlace;

		[LabelFill] public string nameOfFather;

		[LabelFill] public string birthplaceOfFather;

		[LabelFill] public string nameOfMother;

		[LabelFill] public string birthplaceOfMother;

		[LabelFill] public string informant;

		[Header("Certification")] [LabelFill] public string deathWorkEnd;

		[LabelFill] public string contracted;

		[LabelFill] public string causeOfDeath;

		[LabelFill] public string operation = "no";

		[LabelFill] public string autopsy = "no";

		[LabelFill] public string diagnosisTest;

		[LabelFill] public string placeOfBurial = "cremated";

		[Header("Discovery")]
		[Tooltip("Discoveries that can be made on the page")]
		public DeathDiscovery[] discoveries;

		public override string Location => state;

		public override string FileName => $"{state} Death Certificate";

		/// <summary>
		/// Create a random death certificate with the given guidelines
		/// </summary>
		/// <param name="faker">faker instance to use</param>
		/// <param name="deathDate">death date to show</param>
		/// <param name="gender">gender of the person</param>
		/// <param name="location">the us state this occurred in</param>
		/// <returns>the new generated certificate</returns>
		public static DeathCert CreateRandom(Faker faker, DateTime deathDate, Name.Gender gender, string location) {
			var random = CreateInstance<DeathCert>();

			var spouseGender = gender == Name.Gender.Female ? Name.Gender.Male : Name.Gender.Female;

			random.county = faker.Address.County();
			random.state = location;
			random.town = faker.Address.City();
			random.address = "home";
			random.residence = faker.Address.StreetAddress();
			random.sex = gender.ToString().ToLower();
			random.race = "";
			random.maritalStatus = "Married";
			random.spouse = faker.Name.FullName(spouseGender);
			random.occupation = "";
			random.employer = "";
			random.birthPlace = location;
			random.nameOfFather = faker.Name.FullName(Name.Gender.Male);
			random.nameOfMother = faker.Name.FullName(Name.Gender.Female);
			random.informant = faker.Name.FullName();
			random.deathWorkEnd = faker.Date.Soon(3, deathDate).ToString(DateUtils.StandardDateFormat);

			return random;
		}
	}
}