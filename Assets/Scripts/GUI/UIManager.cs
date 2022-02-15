using System.Collections;
using Potterblatt.Storage;
using Potterblatt.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class UIManager : SingletonMonobehaviour<UIManager> {
		public UIDocument birthIndex;

		private void Start() {
			var index = AddPage<BirthIndex>(birthIndex);
			index.Setup(null, "New York");
		}

		public void OpenInfo(Person person) {
			// AddPage(infoPage).person = person;
		}
		
		private T AddPage<T>(UIDocument document) where T : AncestorVisualElement {
			document.gameObject.SetActive(true);
			document.rootVisualElement.style.left = new StyleLength {
				value = new Length(100, LengthUnit.Percent)
			};
			
			StartCoroutine(Transition(document));
			
			return document.rootVisualElement.Q<T>();
		}

		private IEnumerator Transition(UIDocument document) {
			yield return new WaitForSeconds(0.5f);
			
			document.rootVisualElement.style.left = new StyleLength {
				value = new Length(0, LengthUnit.Percent)
			};
		}
	}
}