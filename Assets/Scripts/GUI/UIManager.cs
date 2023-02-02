using System.Collections;
using Potterblatt.Storage;
using Potterblatt.Storage.Documents;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Manager that shows what UI page the user wants to see
	/// </summary>
	[RequireComponent(typeof(UIDocument))]
	public class UIManager : SingletonMonobehaviour<UIManager> {
		[Header("Page Controllers")]
		[Tooltip("The family tree page")]
		public TreePage treePage;
		
		[Tooltip("Record request page")]
		public RecordRequestPage recordRequestPage;
		
		[Tooltip("Person Info page")]
		public InfoPage infoPage;
		
		[Tooltip("Birth index page")]
		public BirthIndexPage birthIndexPage;
		
		[Tooltip("Death certificate page page")]
		public DeathCertPage deathCertPage;

		[Header("Settings")]
		[Min(0)]
		[Tooltip("The max degrees to rotate handwritten text by in either direction (+/-)")]
		public float handwritingRotationAmount = 2;

		[Min(0)]
		[Tooltip("The max amount to translate handwritten text by in either direction (+/-)")]
		public Vector2 handwritingOffset = new(20, 5);

		/// <summary>The root of the UI hierarchy</summary>
		private UIDocument rootDoc;
		
		/// <summary>The current page being displayed</summary>
		private GamePage currentPage;
		
		/// <summary>Back button on the header that goes to the tree page</summary>
		private Button backButton;

		/// <summary>The root of modal dialogs on the page</summary>
		public VisualElement ModalRoot { get; private set; }

		protected override void Awake() {
			base.Awake();

			rootDoc = GetComponent<UIDocument>();

			//get each page and make sure inactive
			foreach(var page in GetComponentsInChildren<GamePage>()) {
				page.gameObject.SetActive(false);
			}
			
			//get the back button and hook it up
			backButton = rootDoc.rootVisualElement.Q<Button>("back-button");
			backButton.clicked += () => {
				AnalyticsManager.NewPage("tree");
				AddPage(treePage);
			};

			//get the search button on the header and hook it up to the record request
			var searchButton = rootDoc.rootVisualElement.Q<Button>("record-request");
			searchButton.clicked += () => {
				AnalyticsManager.NewPage("recordRequest");
				AddPage(recordRequestPage);
			};

			//get the modal root
			ModalRoot = rootDoc.rootVisualElement.Q<VisualElement>("modal-root");

			//start waiting for setup
			StartCoroutine(WaitToSetup());
		}

		/// <summary>
		/// <see cref="Coroutine"/> that waits for the save system to startup then continues setup
		/// </summary>
		private IEnumerator WaitToSetup() {
			yield return new WaitUntil(() => SaveState.IsSetup);
			
			//start with the tree page
			AnalyticsManager.NewPage("tree");
			AddPage(treePage);
		}

		/// <summary>
		/// Open an info page for the user
		/// </summary>
		/// <param name="person">The person to show</param>
		public void OpenInfo(Person person) {
			//add the info page and set it up
			AnalyticsManager.NewPersonPage(person);
			AddPage(infoPage).Person = person;
		}

		/// <summary>
		/// Open a document page
		/// </summary>
		/// <param name="person">the person to show on the doc</param>
		/// <param name="doc">the document to show</param>
		public void OpenDoc(Person person, Document doc) {
			AnalyticsManager.NewDocPage(person, doc);
			
			switch(doc) {
				case BirthIndex birthIndex:
					AddPage(birthIndexPage).Setup(person, birthIndex);
					break;
				case DeathCert deathCert:
					AddPage(deathCertPage).Setup(person, deathCert);
					break;
			}
		}
		
		/// <summary>
		/// Add a page to the display and show it
		/// </summary>
		/// <param name="page">The page to show</param>
		/// <returns>The page</returns>
		private T AddPage<T>(T page) where T : GamePage{
			//don't do it if already shown
			if(currentPage == page) {
				return page;
			}
			
			//hide the previous page
			if(currentPage) {
				currentPage.gameObject.SetActive(false);
			}
			
			//turn on the current page
			page.gameObject.SetActive(true);
			currentPage = page;
			
			//show the back button if not the tree page
			backButton.SetEnabled(currentPage != treePage);

			return page;
		}
	}
}