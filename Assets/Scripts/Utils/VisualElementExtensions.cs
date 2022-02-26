using UnityEngine.UIElements;

namespace Potterblatt.Utils {
	public static class VisualElementExtensions {
		public static T GetUserData<T>(this VisualElement visualElement) {
			if(visualElement.userData is T userData) {
				return userData;
			}

			return default;
		}
	}
}