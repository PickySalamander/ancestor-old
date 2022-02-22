using Potterblatt.Storage.People;
using UnityEngine;

namespace Potterblatt.Storage.Documents {
	[CreateAssetMenu(fileName = "Death Cert", menuName = "Ancestor/Create Death Certificate")]
	public class DeathCert : Document {
		public string cornerDateFormat = "M/d/yyyy";
		
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
	}
}