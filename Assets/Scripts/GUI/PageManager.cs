using System;
using System.Collections;
using Pixelplacement;
using Potterblatt.Storage;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Potterblatt.GUI {
	public class PageManager : SingletonMonobehaviour<PageManager> {
		public Canvas canvas;
		public Button backButton;

		[Header("Pages")]
		public TreePage treePage;
		public InfoPage infoPage;

		private RectTransform canvasTransform;
		private GamePage currentPage;

		private void Start() {
			canvasTransform = (RectTransform) canvas.transform;
			
			backButton.onClick.AddListener(GoBack);
			
			AddPage(treePage);
		}

		public void OpenInfo(Person person) {
			AddPage(infoPage).person = person;
		}

		private T AddPage<T>(T page) where T : GamePage {
			var oldPage = currentPage;
			
			currentPage = Instantiate(page, canvasTransform, false);
			var rectTransform = (RectTransform) currentPage.transform;
			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(1, 1);
			rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);

			currentPage.CanvasGroup.alpha = 0;
			currentPage.CanvasGroup.interactable = false;

			if(oldPage) {
				MoveOff(oldPage);
			}

			StartCoroutine(MoveOn());
			
			backButton.gameObject.SetActive(!(page is TreePage));

			return currentPage as T;
		}

		private IEnumerator MoveOn() {
			yield return 0;
			
			var rectTransform = (RectTransform) currentPage.transform;
			var pageRect = rectTransform.rect;
			
			var canvasRect = canvasTransform.rect;
			var localPosition = rectTransform.localPosition;
			var startlocation = new Vector3(canvasRect.width / 2 + pageRect.width / 2, 
				localPosition.y);
			
			Tween.LocalPosition(rectTransform, startlocation, 
				localPosition, 0.5f, 0, Tween.EaseInOutStrong, 
				completeCallback:() => currentPage.CanvasGroup.interactable = true);

			Tween.CanvasGroupAlpha(currentPage.CanvasGroup, 0, 1, 0.25f, 0);
		}
		
		private void MoveOff(GamePage page) {
			var rectTransform = (RectTransform) page.transform;
			var pageRect = rectTransform.rect;
			
			var canvasRect = canvasTransform.rect;
			var localPosition = rectTransform.localPosition;
			var endLocation = new Vector3(-canvasRect.width / 2 - pageRect.width / 2, 
				localPosition.y);
			
			page.CanvasGroup.interactable = false;
			
			Tween.LocalPosition(rectTransform, localPosition, 
				endLocation, 0.5f, 0, Tween.EaseInOutStrong, 
				completeCallback:() => Destroy(page.gameObject));
			
			Tween.CanvasGroupAlpha(currentPage.CanvasGroup, 0, 0.25f, 0.25f);
		}

		private void GoBack() {
			AddPage(treePage);
		}

		[ContextMenu("Simulation Move On")]
		public void SimulationMoveOn() {
			StartCoroutine(MoveOn());
		}
	}
}