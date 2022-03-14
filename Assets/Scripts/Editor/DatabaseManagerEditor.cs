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
	public class DatabaseManagerEditor : EditorWindow {
		private const string studioPref = "litedb.studio.loc";
		
		public string studioLocation;

		private string personLookup;
		
		private Person editing;

		private void OnEnable() {
			studioLocation = EditorPrefs.GetString(studioPref);
		}

		private void OnGUI() {
			UnityEngine.GUI.skin.label.wordWrap = true;

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

			EditorGUILayout.Space();
		}

		private void OpenStudio() {
			var pathToFile = 
				new Uri(Path.Combine(Application.dataPath, "Resources", "database")).LocalPath;
			
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
		
		/// <summary>Quick open function to open the window</summary>
		[MenuItem("Ancestor/Database Editor")]
		public static void Open() {
			GetWindow<DatabaseManagerEditor>(false, "Database Editor", true);
		}
	}
}