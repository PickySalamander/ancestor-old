using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using Potterblatt.Utils;
using UnityEngine;

namespace Potterblatt.Storage {
	public class DatabaseManager : SingletonMonobehaviour<DatabaseManager> {
		public class PersonTest {
			[BsonId]
			public Guid Id { get; set; }
			public string FirstName { get; set; }
			public string Sex { get; set; }
		}
		
		public string databasePath = "database.bytes";

		[SerializeField]
		private bool clearDatabase;
		
		//TODO look into storing the database in streaming assets or resources (copy to persistentDataPath)

		protected override void Awake() {
			base.Awake();

			if(clearDatabase && Debug.isDebugBuild) {
				Debug.Log("Clearing old database, because flag is set");
				File.Delete(DatabasePath);
			}

			if(!File.Exists(DatabasePath)) {
				FirstTimeSetup();
			}

			using(var db = new LiteDatabase(DatabasePath)) {
				var persons = db.GetCollection<PersonTest>("Person");
				
				var newPerson = new PersonTest {
					Id = Guid.NewGuid(),
					FirstName = "bob",
					Sex = "male"
				};

				persons.Insert(newPerson);

				Debug.Log(persons.FindById(newPerson.Id));

				foreach(var test in persons.FindAll()) {
					Debug.Log(test);
				}

				var id = Guid.Parse("de61c630-8e8c-4773-9c5a-aa31daff8318");
				var value = persons.FindById(id);
				
				Debug.Log(value);
				
				Debug.Log($"{value != null}");
				
				Debug.Log($"{value.FirstName} {value.Sex}");
			}
		}

		private void FirstTimeSetup() {
			Debug.Log("Starting first time setup...");

			var path = Path.GetFileNameWithoutExtension(databasePath);
			var initialDatabase = Resources.Load<TextAsset>(path);
			File.WriteAllBytes(DatabasePath, initialDatabase.bytes);
			
			Debug.Log($"Database setup at {DatabasePath}");
			Resources.UnloadAsset(initialDatabase);
		}

		public string DatabasePath => Path.Combine(Application.persistentDataPath, databasePath);
	}
}