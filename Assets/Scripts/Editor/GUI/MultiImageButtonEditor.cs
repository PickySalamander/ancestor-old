using Potterblatt.GUI;
using UnityEditor;
using UnityEditor.UI;

namespace Potterblatt.Editor.GUI {
	/// <summary>
	/// Custom Editor for the the delegating button since the <see cref="ButtonEditor"/> blocks the delegates
	/// </summary>
	[CustomEditor(typeof(MultiImageButton), true)]
	[CanEditMultipleObjects]
	public class MultiImageButtonEditor : ButtonEditor {
		/// <summary>Property for the children</summary>
		private SerializedProperty childrenProperty;

		protected override void OnEnable() {
			base.OnEnable();
			
			//setup
			childrenProperty = serializedObject.FindProperty("children");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			//Show the children
			serializedObject.Update();
			EditorGUILayout.PropertyField(childrenProperty);
			serializedObject.ApplyModifiedProperties();
		}
	}
}