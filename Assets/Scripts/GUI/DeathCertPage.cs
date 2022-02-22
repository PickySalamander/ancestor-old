using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;

namespace Potterblatt.GUI {
	public class DeathCertPage : GamePage {
		public void Setup(Person person, DeathCert deathCert) {
			deathCert.FillLabels(RootElement);
		}
		
// 		public Handwriting county;
// 		public Handwriting state;
// 		public Handwriting town;
// 		public Handwriting location;
// 		public Handwriting fullName;
// 		public Handwriting address;
//
// #if UNITY_EDITOR
// 		[ContextMenu("Sample")]
// 		public void DoSample() {
// 			if(!Application.isPlaying) {
// 				return;
// 			}
// 			
// 			county.Text = "The County of Fun";
// 			state.Text = "New York";
// 			town.Text = "Albany";
// 			location.Text = "Albany Medical Center";
// 			fullName.Text = "Bob Smith";
// 			address.Text = "123 Ashbury Lane";
// 		}
// #endif
	}
}