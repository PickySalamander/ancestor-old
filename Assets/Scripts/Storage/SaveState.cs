using System;
using System.Collections.Generic;
using System.Linq;
using Potterblatt.GUI;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;

namespace Potterblatt.Storage {
	/// <summary>
	/// The save and storage system. Right now things aren't saved to disk and each time the app is started it is
	/// restarted.
	/// </summary>
	public class SaveState : SingletonMonobehaviour<SaveState> {
		[Tooltip("The person who is the root of the tree")]
		public Person treeRoot;

		/// <summary>Listen for new discoveries being made</summary>
		public event Action<Person, DiscoveryType> onDiscovery;
		
		/// <summary>Dictionary of all the people in the game</summary>
		public Dictionary<string, Person> Person {
			get;
			private set;
		}

		/// <summary>Dictionary of all the people in the game and their discoveries made</summary>
		public Dictionary<string, DiscoveryType> State {
			get;
			private set;
		}

		protected override void Awake() {
			base.Awake();
			
			//create the dictionaries
			State = new Dictionary<string, DiscoveryType>();
			Person = new Dictionary<string, Person>();

			//queue for the people parsing in the tree
			var queue = new Queue<Person>();
			queue.Enqueue(treeRoot);

			//go through the queue
			while(queue.Count > 0) {
				var person = queue.Dequeue();
				
				//add the person to the dictionaries
				State[person.uuid] = person.DefaultDiscovery;
				Person[person.uuid] = person;

				//queue up mother if exists
				if(person.mother) {
					queue.Enqueue(person.mother);
				}

				//queue up father if exists
				if(person.father) {
					queue.Enqueue(person.father);
				}
			}
		}

		/// <summary>
		/// Get the discoveries made for a person's id
		/// </summary>
		/// <param name="uuid">The person's id</param>
		public DiscoveryType this[string uuid] => State[uuid];
		
		/// <summary>
		/// Get the discoveries for a person
		/// </summary>
		/// <param name="person">The person to look up</param>
		public DiscoveryType this[Person person] => this[person.uuid];
		
		/// <summary>
		/// Make a discovery for a person
		/// </summary>
		/// <param name="discovery">The discovery to make</param>
		/// <param name="showDialog">Should a dialog be shown to the user?</param>
		/// <returns>All the types now discovered</returns>
		public DiscoveryType ChangeDiscovery(Discovery discovery, bool showDialog=true) {
			return ChangeDiscovery(discovery.person, discovery.type, showDialog);
		}
		
		/// <summary>
		/// Make a discovery for a person
		/// </summary>
		/// <param name="person">The person</param>
		/// <param name="type">The discovery type</param>
		/// <param name="showDialog">Should a dialog be shown to the user?</param>
		/// <returns>All the types now discovered</returns>
		public DiscoveryType ChangeDiscovery(Person person, DiscoveryType type, bool showDialog=true) {
			return ChangeDiscovery(person.uuid, type, showDialog);
		}
		
		/// <summary>
		/// Make a discovery for a person
		/// </summary>
		/// <param name="uuid">The person</param>
		/// <param name="type">The discovery type</param>
		/// <param name="showDialog">Should a dialog be shown to the user?</param>
		/// <returns>All the types now discovered</returns>
		public DiscoveryType ChangeDiscovery(string uuid, DiscoveryType type, bool showDialog=true) {
			//show a dialog
			if(showDialog) {
				//if already discovered then warn the user
				if(IsDiscovered(uuid, type)) {
					DialogManager.Instance.ShowDialog("Already Discovered", 
						"You have already discovered this information");
				}
				
				//otherwise tell them they were successful
				else {
					DialogManager.Instance.ShowDialog("Discovered", 
						"You discovered something! Click the back button to go back to the family tree and " +
						"see what you discovered.");
				}
			}
			
			//make the discovery and notify
			var newDiscovery = State[uuid] |= type;
			onDiscovery?.Invoke(Person[uuid], newDiscovery);
			return newDiscovery;
		}
		
		/// <summary>
		/// Is the type or type already discovered on the person?
		/// </summary>
		/// <param name="person">The person</param>
		/// <param name="type">The type of discovery to check</param>
		/// <returns>true if discovered</returns>
		public bool IsDiscovered(Person person, DiscoveryType type) {
			return IsDiscovered(person.uuid, type);
		}
		
		/// <summary>
		/// Is the type or type already discovered on the person?
		/// </summary>
		/// <param name="uuid">The id of the person</param>
		/// <param name="type">The type of discovery to check</param>
		/// <returns>true if discovered</returns>
		public bool IsDiscovered(string uuid, DiscoveryType type) {
			return (this[uuid] & type) != DiscoveryType.None;
		}

		/// <summary>
		/// Iterate over all of the people in the system with the given discoveries
		/// </summary>
		/// <param name="condition">The discoveries to iterate</param>
		public static IEnumerable<Person> IteratePeople(DiscoveryType condition = DiscoveryType.None) {
			return condition == DiscoveryType.None ? 
				Instance.Person.Values : 
				Instance.Person.Values.Where(person => person.IsDiscovered(condition));
		} 
		
		/// <summary>
		/// Iterated over all the events on all the people in the system with the given discoveries
		/// </summary>
		/// <param name="condition">The discoveries to iterate</param>
		public static IEnumerable<KeyValuePair<Person, LifeEvent>> IterateEvents(DiscoveryType condition = DiscoveryType.None) {
			foreach(var person in IteratePeople(condition)) {
				foreach(var lifeEvent in person.timeLine) {
					yield return new KeyValuePair<Person, LifeEvent>(person, lifeEvent);
				}
			}
		}
	}
}