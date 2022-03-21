using System;
using Bogus;
using Bogus.DataSets;
using Potterblatt.GUI;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Potterblatt.Storage.Documents {
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

		public DeathDiscovery[] discoveries;

		public override string Location => state;

		public override string FileName => $"{state} Death Certificate";

		public static DeathCert CreateRandom(Faker faker, DateTime deathDate, Name.Gender gender, string location) {
			var rando = CreateInstance<DeathCert>();

			var spouseGender = gender == Name.Gender.Female ? Name.Gender.Male : Name.Gender.Female;

			rando.county = faker.Address.County();
			rando.state = location;
			rando.town = faker.Address.City();
			rando.address = "home";
			rando.residence = faker.Address.StreetAddress();
			rando.sex = gender.ToString().ToLower();
			rando.race = "";
			rando.maritalStatus = "Married";
			rando.spouse = faker.Name.FullName(spouseGender);
			rando.occupation = "";
			rando.employer = "";
			rando.birthPlace = location;
			rando.nameOfFather = faker.Name.FullName(Name.Gender.Male);
			rando.nameOfMother = faker.Name.FullName(Name.Gender.Female);
			rando.informant = faker.Name.FullName();
			rando.deathWorkEnd = faker.Date.Soon(3, deathDate).ToString(DateUtils.StandardDateFormat);

			return rando;
		}
	}
}