using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potterblatt.GUI {
	public class Handwriting : MonoBehaviour {
		[SerializeField]
		private TMP_Text text;
		
		private void Awake() {
			text.text = "";
		}

		public string Text {
			get => text.text;
			set {
				text.text = value;
				RandomizePosition();
			}
		}

		private void RandomizePosition() {
			var globals = Globals.Instance;

			var posOffset = new Vector3(Random.Range(-globals.positionChange.x, globals.positionChange.x),
				Random.Range(-globals.positionChange.y, globals.positionChange.y));
			
			var rotOffset = Random.Range(-globals.rotationChange, globals.rotationChange);

			var textTransform = text.transform;
			textTransform.localPosition += posOffset;
			textTransform.eulerAngles += new Vector3(0, 0, rotOffset);
		}
	}
}