using System.Linq;
using Potterblatt.GUI;
using UnityEditor;
using UnityEngine;

namespace Potterblatt.Editor.GUI {
	/// <summary>
	/// Custom Editor for the the <see cref="MultiImageButtonChild"/>, mostly to quickly add this to the parent scope
	/// </summary>
	[CustomEditor(typeof(MultiImageButtonChild), true)]
	[CanEditMultipleObjects]
	public class MultiImageButtonChildEditor : UnityEditor.Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			
			EditorGUILayout.Space();

			//Show the button
			if(GUILayout.Button("Add to Parent")) {
				AddToParent();
			}
		}

		/// <summary>
		/// Add the object to any parents
		/// </summary>
		private void AddToParent() {
			//set the undo group
			Undo.SetCurrentGroupName("Add to Parent");
			
			//got through each object being edited
			foreach(var childTarget in targets) {
				var child = childTarget as MultiImageButtonChild;
				if(child) {
					//find the parent if it is either on the current object or one up
					var parent = child.GetComponent<MultiImageButton>();
					if(!parent) {
						parent = child.GetComponentInParent<MultiImageButton>();
					}

					//if no parent abort
					if(!parent) {
						Debug.LogWarning($"Failed to find parent for {child.name}", child);
						continue;
					}

					//if not already in the children
					if(parent.children == null || !parent.children.Contains(child)) {
						//get the serialized object
						var parentObj = new SerializedObject(parent);
						parentObj.Update();
						
						//get the property and add the child
						var children = parentObj.FindProperty("children");
						if(children.isArray) {
							var index = children.arraySize;
							children.InsertArrayElementAtIndex(index);
							var newProp = children.GetArrayElementAtIndex(index);
							newProp.objectReferenceValue = child;
						}
						
						//apply the change
						parentObj.ApplyModifiedProperties();
					}
				}
			}
			
			//collapse the undo operations
			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
		}
	}
}