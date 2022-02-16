using System.Collections;
using Potterblatt.Storage;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class UIManager : SingletonMonobehaviour<UIManager> {
		public TreePage treePage;
		public InfoPage infoPage;
		public BirthIndex birthIndex;

		private GamePage currentPage;

		protected override void Awake() {
			base.Awake();

			foreach(var page in GetComponentsInChildren<GamePage>()) {
				page.gameObject.SetActive(false);
			}
			
			AddPage(treePage);
		}

		public void OpenInfo(Person person) {
			Debug.Log($"Open person! {person.firstName}");
			AddPage(infoPage).Person = person;
		}

		public void OpenBirth(Person person) {
			AddPage(birthIndex);
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

			return page;
		}
	}
}