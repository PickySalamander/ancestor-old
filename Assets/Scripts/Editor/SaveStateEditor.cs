using Potterblatt.Storage;
using Potterblatt.Storage.People;
using UnityEditor;

namespace Potterblatt.Editor {
	[CustomEditor(typeof(SaveState))]
	public class SaveStateEditor : UnityEditor.Editor {
		private bool foldout;
		
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			
			EditorGUILayout.Separator();

			foldout = EditorGUILayout.Foldout(foldout, "People");

			if(foldout) {
				var saveState = (SaveState) target;

				if(saveState.People != null) {
					var keys = saveState.People.Keys;
					foreach(var key in keys) {
						saveState.People[key] = (DiscoveryType) 
							EditorGUILayout.EnumFlagsField(key, saveState.People[key]);
					}
				}
			}
		}
	}
}