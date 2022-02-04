using UnityEngine;

namespace Potterblatt {
	public class Globals : MonoBehaviour {

		[Header("Handwriting")]
		public float rotationChange;

		public Vector2 positionChange;

		private void Awake() {
			Instance = this;
		}

		private void OnDestroy() {
			Instance = null;
		}
		
		public static Globals Instance {
			get;
			private set;
		}
	}
}