using System;
using System.Collections.Generic;
using System.Linq;
using Potterblatt.GUI;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine.Events;

namespace Potterblatt.Storage {
	public class SaveState : SingletonMonobehaviour<SaveState> {
		public Person treeRoot;

		public event Action<Person, DiscoveryType> onDiscovery;

		public Dictionary<string, Person> Person {
			get;
			private set;
		}

		public Dictionary<string, DiscoveryType> State {
			get;
			private set;
		}

		protected override void Awake() {
			base.Awake();
			
			State = new Dictionary<string, DiscoveryType>();
			Person = new Dictionary<string, Person>();

			var queue = new Queue<Person>();
			queue.Enqueue(treeRoot);

			while(queue.Count > 0) {
				var person = queue.Dequeue();
				State[person.uuid] = person.DefaultDiscovery;
				Person[person.uuid] = person;

				if(person.mother) {
					queue.Enqueue(person.mother);
				}

				if(person.father) {
					queue.Enqueue(person.father);
				}
			}
		}

		public DiscoveryType this[string uuid] => State[uuid];

		public DiscoveryType this[Person person] => this[person.uuid];
		
		public DiscoveryType ChangeDiscovery(Discovery discovery, bool showDialog=true) {
			return ChangeDiscovery(discovery.person, discovery.type, showDialog);
		}
		
		public DiscoveryType ChangeDiscovery(Person person, DiscoveryType type, bool showDialog=true) {
			return ChangeDiscovery(person.uuid, type, showDialog);
		}
		
		public DiscoveryType ChangeDiscovery(string uuid, DiscoveryType type, bool showDialog=true) {
			if(showDialog) {
				if(IsDiscovered(uuid, type)) {
					DialogManager.Instance.ShowDialog("Already Discovered", 
						"You have already discovered this information");
				}
				else {
					DialogManager.Instance.ShowDialog("Discovered", 
						"You discovered something! Click the back button to go back to the family tree and " +
						"see what you discovered.");
				}
			}

			var newDiscovery = State[uuid] |= type;
			onDiscovery?.Invoke(Person[uuid], newDiscovery);
			return newDiscovery;
		}
		
		public bool IsDiscovered(Person person, DiscoveryType type) {
			return IsDiscovered(person.uuid, type);
		}
		
		public bool IsDiscovered(string uuid, DiscoveryType type) {
			return (this[uuid] & type) != DiscoveryType.None;
		}

		public static IEnumerable<Person> IteratePeople(DiscoveryType condition = DiscoveryType.None) {
			return condition == DiscoveryType.None ? 
				Instance.Person.Values : 
				Instance.Person.Values.Where(person => person.IsDiscovered(condition));
		} 
		
		public static IEnumerable<KeyValuePair<Person, LifeEvent>> IterateEvents(DiscoveryType condition = DiscoveryType.None) {
			foreach(var person in IteratePeople(condition)) {
				foreach(var lifeEvent in person.timeLine) {
					yield return new KeyValuePair<Person, LifeEvent>(person, lifeEvent);
				}
			}
		}
	}
}