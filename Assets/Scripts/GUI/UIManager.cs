using System.Collections;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Potterblatt.GUI {
	[RequireComponent(typeof(UIDocument))]
	public class UIManager : SingletonMonobehaviour<UIManager> {
		public TreePage treePage;
		public RecordRequestPage recordRequestPage;
		public InfoPage infoPage;
		public BirthIndexPage birthIndexPage;
		public DeathCertPage deathCertPage;

		[Header("Settings")] 
		public RandomNames randomNames;

		[Min(0)]
		public float handwritingRotationAmount = 2;

		[Min(0)]
		public Vector2 handwritingOffset = new Vector2(20, 5);

		private UIDocument rootDoc;
		private GamePage currentPage;
		private Button backButton;

		public VisualElement ModalRoot { get; private set; }

		protected override void Awake() {
			base.Awake();

			rootDoc = GetComponent<UIDocument>();

			foreach(var page in GetComponentsInChildren<GamePage>()) {
				page.gameObject.SetActive(false);
			}
			
			backButton = rootDoc.rootVisualElement.Q<Button>("back-button");
			backButton.clicked += () => AddPage(treePage);

			var searchButton = rootDoc.rootVisualElement.Q<Button>("record-request");
			searchButton.clicked += () => AddPage(recordRequestPage);

			ModalRoot = rootDoc.rootVisualElement.Q<VisualElement>("modal-root");

			StartCoroutine(WaitToSetup());
		}

		private IEnumerator WaitToSetup() {
			yield return new WaitUntil(() => SaveState.IsSetup);
			
			AddPage(treePage);
		}

		public void OpenInfo(Person person) {
			Debug.Log($"Open person! {person.firstName}");
			AddPage(infoPage).Person = person;
		}

		public void OpenDoc(Person person, Document doc) {
			switch(doc) {
				case BirthIndex birthIndex:
					AddPage(birthIndexPage).Setup(person, birthIndex);
					break;
				case DeathCert deathCert:
					AddPage(deathCertPage).Setup(person, deathCert);
					break;
			}
		}
		
		private T AddPage<T>(T page) where T : GamePage{
			if(currentPage == page) {
				return page;
			}
			
			if(currentPage) {
				currentPage.gameObject.SetActive(false);
			}
			
			page.gameObject.SetActive(true);
			currentPage = page;

			var enabled = currentPage != treePage;
			
			backButton.SetEnabled(enabled);

			return page;
		}
	}
}