using System;
using System.Diagnostics;
using System.IO;
using LiteDB;
using Potterblatt.Storage;
using Potterblatt.Storage.People;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Potterblatt.Editor {
	[CustomEditor(typeof(DatabaseManager))]
	public class DatabaseManagerEditor : UnityEditor.Editor {
		private const string studioPref = "litedb.studio.loc";
		
		public string studioLocation;

		private void OnEnable() {
			studioLocation = EditorPrefs.GetString(studioPref);
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			EditorGUILayout.Space();

			GUILayout.Label("Database Editor", EditorStyles.boldLabel);

			EditorGUILayout.Space();

			GUILayout.BeginHorizontal();

			EditorGUILayout.TextField("Studio Location", studioLocation);

			if(GUILayout.Button("Set")) {
				studioLocation =
					EditorUtility.OpenFilePanel("LiteDB Studio", Application.dataPath, "exe");

				EditorPrefs.SetString(studioPref, studioLocation);
			}

			GUILayout.EndHorizontal();

			UnityEngine.GUI.enabled = !string.IsNullOrWhiteSpace(studioLocation);

			if(GUILayout.Button("Open Studio")) {
				OpenStudio();
			}

			UnityEngine.GUI.enabled = true;
		}

		private void OpenStudio() {
			var targetDb = ((DatabaseManager) target).databasePath;
			var pathToFile = 
				new Uri(Path.Combine(Application.dataPath, "Resources", targetDb)).LocalPath;
			
			Debug.Log($"Opening {studioLocation} with arguments {pathToFile}");
			
			using var process = new Process {
				StartInfo = {
					FileName = studioLocation,
					Arguments = pathToFile,
					UseShellExecute = true,
					RedirectStandardOutput = false,
					WindowStyle = ProcessWindowStyle.Normal
				}
			};
			
			process.Start();
		}
	}
}