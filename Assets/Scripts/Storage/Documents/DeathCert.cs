using System;
using Potterblatt.GUI;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potterblatt.Storage.Documents {
	[CreateAssetMenu(fileName = "Death Cert", menuName = "Ancestor/Create Death Certificate")]
	public class DeathCert : Document {
		[Header("Basic Info")] 
		[LabelFill] public string county;

		[LabelFill] public string state;

		[LabelFill] public string town;

		[LabelFill] public string address;

		[Header("Basic Info Pt 2")] 
		[LabelFill]
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
		
		[Header("Certification")]
		[LabelFill] public string deathWorkEnd;
		
		[LabelFill] public string contracted;
		
		[LabelFill] public string causeOfDeath;
		
		[LabelFill] public string operation = "no";
		
		[LabelFill] public string autopsy = "no";

		[LabelFill] public string diagnosisTest;

		[LabelFill] public string placeOfBurial = "cremated";
		
		public override string Location => state;
		
		public override string FileName => $"{state} Death Certificate";

		public static DeathCert CreateRandom(DateTime deathDate, bool isFemale, string location) {
			var rando = CreateInstance<DeathCert>();
			var randomNames = UIManager.Instance.randomNames;

			rando.county = randomNames.GetCounty();
			rando.state = location;
			rando.town = randomNames.GetTown();
			rando.address = "home";
			rando.residence = randomNames.GetStreet();
			rando.sex = isFemale ? "female" : "male";
			rando.race = "";
			rando.maritalStatus = "Married";
			rando.spouse = $"{randomNames.GetFirstName(!isFemale)} {randomNames.GetLastName()}";
			rando.occupation = "";
			rando.employer = "";
			rando.birthPlace = location;
			rando.nameOfFather = $"{randomNames.GetFirstName(false)} {randomNames.GetLastName()}";
			rando.nameOfMother = $"{randomNames.GetFirstName(true)} {randomNames.GetLastName()}";
			rando.informant = $"{randomNames.GetFirstName()} {randomNames.GetLastName()}";

			var deathEnd = Random.Range(0, 3);
			rando.deathWorkEnd = deathDate.AddDays(deathEnd).ToString("M/d/yyyy");
			
			return rando;
		}
	}
}