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

				if(saveState.State != null) {
					var keys = saveState.State.Keys;
					foreach(var key in keys) {
						saveState.State[key] = (DiscoveryType) 
							EditorGUILayout.EnumFlagsField(key, saveState.State[key]);
					}
				}
			}
		}
	}
}