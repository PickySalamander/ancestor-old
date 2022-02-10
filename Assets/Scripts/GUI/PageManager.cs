using System;
using Pixelplacement;
using Potterblatt.Storage;
using Potterblatt.Utils;
using UnityEngine;

namespace Potterblatt.GUI {
	public class PageManager : SingletonMonobehaviour<PageManager> {
		public Canvas canvas;
		
		public InfoPage infoPage;
		public TreePage treePage;

		public Vector2 newPageLocation;

		private void Start() {
			treePage.Show();
		}

		public void OpenInfo(Person person) {
			treePage.gameObject.SetActive(false);
			infoPage.person = person;
			infoPage.gameObject.SetActive(true);
		}
	}
}