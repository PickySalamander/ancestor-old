using Potterblatt.Storage.People;
using UnityEngine;

namespace Potterblatt.Storage.Documents {
	[CreateAssetMenu(fileName = "Death Cert", menuName = "Ancestor/Create Death Certificate")]
	public class DeathCert : Document {
		[Header("Basic Info")]
		[LabelFill]
		public string county;
		
		[LabelFill]
		public string state;
		
		[LabelFill]
		public string town;
		
		[LabelFill]
		public string address;

		[Header("Basic Info Pt 2")]
		[LabelFill]
		public string residence;
		[LabelFill]
		public string lengthOfResidence;
		[LabelFill]
		public string foreignBirth;
	}
}