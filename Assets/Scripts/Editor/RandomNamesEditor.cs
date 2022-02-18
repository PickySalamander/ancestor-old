using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Potterblatt.Editor {
	[CustomEditor(typeof(RandomNames))]
	public class RandomNamesEditor : UnityEditor.Editor {
		private const string GeneratorUrl = "https://randomuser.me/api/";

		private int numNames = 10;

		private bool append = true;

		private UnityWebRequest request;
		
		private void OnEnable() {
			request = null;
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.Separator();
                
			EditorGUILayout.LabelField("Generate Names", EditorStyles.boldLabel);
                
			EditorGUILayout.Separator();

			numNames = EditorGUILayout.IntField("Number of names to generate", numNames);
			
			append = !EditorGUILayout.Toggle("Clear out the file?", !append);

			UnityEngine.GUI.enabled = numNames > 0 || request != null;

			if(GUILayout.Button("Generate")) {
				Generate();
			}

			UnityEngine.GUI.enabled = true;

			base.OnInspectorGUI();
		}

		private void Generate() {
			var requestUrl = $"{GeneratorUrl}?results={numNames}&inc=gender,name&nat=US";
			request = UnityWebRequest.Get(requestUrl);
			request.SendWebRequest();

			EditorApplication.update += Update;
		}

		private void Update() {
			if(request != null && request.isDone) {
				if(request.result == UnityWebRequest.Result.Success) {
					ParseResult();
				}
				else {
					Debug.LogError($"Failed to get names: {request.error}");
				}
				
				request = null;
				EditorApplication.update -= Update;
			}
		}

		private void ParseResult() {
			var parsed = request.GetJsonResponse<JObject>();
			var results = (JArray) parsed["results"];
			var targetRandom = (RandomNames) target;

			if(results != null) {
				Undo.RegisterCompleteObjectUndo(target, "Generate Random Names");

				var femaleNames = new HashSet<string>();
				var maleNames = new HashSet<string>();
				var lastNames = new HashSet<string>();
				
				if(append) {
					femaleNames.UnionWith(targetRandom.femaleNames);
					maleNames.UnionWith(targetRandom.maleNames);
					lastNames.UnionWith(targetRandom.lastNames);
				}
				
				foreach(var result in results) {
					var nameObj = result.Value<JObject>("name");
					Debug.Assert(nameObj != null, nameof(nameObj) + " != null");

					var isFemale = result.Value<string>("gender") == "female";
					var list = isFemale ? femaleNames : maleNames;
					list.Add(nameObj.Value<string>("first"));
					lastNames.Add(nameObj.Value<string>("last"));
				}

				targetRandom.lastNames = lastNames.ToList();
				targetRandom.femaleNames = femaleNames.ToList();
				targetRandom.maleNames = maleNames.ToList();
				
				EditorUtility.SetDirty(target);
			}
		}
	}
}