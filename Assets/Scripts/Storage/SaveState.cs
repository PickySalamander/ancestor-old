using System.Collections.Generic;
using Potterblatt.Storage.People;
using Potterblatt.Utils;
using UnityEngine;

namespace Potterblatt.Storage {
	public class SaveState : SingletonMonobehaviour<SaveState> {
		public Person treeRoot;

		public Dictionary<string, DiscoveryType> People {
			get;
			private set;
		}

		protected override void Awake() {
			base.Awake();
			
			People = new Dictionary<string, DiscoveryType>();

			var queue = new Queue<Person>();
			queue.Enqueue(treeRoot);

			while(queue.Count > 0) {
				var person = queue.Dequeue();
				People[person.uuid] = person.DefaultDiscovery;

				if(person.mother) {
					queue.Enqueue(person.mother);
				}

				if(person.father) {
					queue.Enqueue(person.father);
				}
			}
		}

		public DiscoveryType this[string uuid] => People[uuid];

		public DiscoveryType this[Person person] => this[person.uuid];

		public DiscoveryType ChangeDiscovery(Person person, DiscoveryType type) {
			return ChangeDiscovery(person.uuid, type);
		}
		
		public DiscoveryType ChangeDiscovery(string uuid, DiscoveryType type) {
			Debug.Log($"New discovery, before:{People[uuid]}, type:{type} after:{People[uuid] | type}");
			return People[uuid] |= type;
		}
		
		public DiscoveryType ChangeDiscovery(Discovery discovery) {
			return ChangeDiscovery(discovery.person, discovery.type);
		}
	}
}