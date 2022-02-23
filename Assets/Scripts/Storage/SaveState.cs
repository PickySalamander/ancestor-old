using System.Collections.Generic;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;

namespace Potterblatt.Storage {
	public class SaveState : SingletonMonobehaviour<SaveState> {
		public Person treeRoot;

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

		public DiscoveryType ChangeDiscovery(Person person, DiscoveryType type) {
			return ChangeDiscovery(person.uuid, type);
		}
		
		public DiscoveryType ChangeDiscovery(string uuid, DiscoveryType type) {
			return State[uuid] |= type;
		}
		
		public DiscoveryType ChangeDiscovery(Discovery discovery) {
			return ChangeDiscovery(discovery.person, discovery.type);
		}
	}
}