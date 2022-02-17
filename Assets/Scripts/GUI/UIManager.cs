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
		public InfoPage infoPage;
		public BirthIndexPage birthIndexPage;

		private UIDocument rootDoc;
		private GamePage currentPage;
		private Button backButton;

		protected override void Awake() {
			base.Awake();

			rootDoc = GetComponent<UIDocument>();

			foreach(var page in GetComponentsInChildren<GamePage>()) {
				page.gameObject.SetActive(false);
			}
			
			backButton = rootDoc.rootVisualElement.Q<Button>("back-button");
			backButton.clicked += () => AddPage(treePage);
			
			AddPage(treePage);
		}

		public void OpenInfo(Person person) {
			Debug.Log($"Open person! {person.firstName}");
			AddPage(infoPage).Person = person;
		}

		public void OpenDoc(Person person, Document doc) {
			if(doc is BirthIndex birthIndex) {
				AddPage(birthIndexPage).Setup(person, birthIndex);
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

			backButton.style.display =
				new StyleEnum<DisplayStyle>(currentPage == treePage ? DisplayStyle.None : DisplayStyle.Flex);

			return page;
		}
	}
}