using System;
using System.Linq;
using UnityEngine;

namespace Potterblatt.Storage.People {
	/// <summary>
	/// Storage of a person in the game (real or generated)
	/// </summary>
	[CreateAssetMenu(fileName = "Person", menuName = "Ancestor/Create Person")]
	public class Person : ScriptableObject {
		[Tooltip("unique id of the person")]
		public string uuid;
		
		[Tooltip("Gender storage")]
		public bool isFemale;
		
		[Tooltip("The person's father")]
		public Person father;
		
		[Tooltip("The person's mother")]
		public Person mother;
		
		[Tooltip("The person's first name")]
		public string firstName;
		
		[Tooltip("The person's last name")]
		public string lastName;
		
		[Tooltip("All the events in the person's life")]
		public LifeEvent[] timeLine;
		
		[Tooltip("Any discoveries that should already be made")]
		[SerializeField]
		private DiscoveryType defaultDiscovery = DiscoveryType.None;

		/// <summary>
		/// Any discoveries that should already be made
		/// </summary>
		public DiscoveryType DefaultDiscovery => defaultDiscovery;

		private void Reset() {
			//generate a new id automatically when a new person is created in the editor
			uuid ??= Guid.NewGuid().ToString();
		}
		
		/// <summary>
		/// Get an event by it's event type
		/// </summary>
		/// <param name="type">the type of the event</param>
		/// <returns>the event from the timeline if found, otherwise null</returns>
		public LifeEvent GetEventByType(LifeEventType type) {
			return timeLine.FirstOrDefault(lifeEvent => lifeEvent.type == type);
		}

		/// <summary>Get a person's full name</summary>
		public string FullName => $"{firstName} {lastName}";

		/// <summary>Get the event for a person's birth, if found</summary>
		public LifeEvent Born => GetEventByType(LifeEventType.Birth);
		
		/// <summary>Get the event for a person's death, if found</summary>
		public LifeEvent Death => GetEventByType(LifeEventType.Death);

		/// <summary>Whether this person is real or fake/generated</summary>
		public bool IsReal => !string.IsNullOrWhiteSpace(uuid);
		
		/// <summary>Has the user made any discoveries for this person yet?</summary>
		public bool HasNoDiscoveries() {
			return SaveState.Instance[this] == DiscoveryType.None;
		}

		/// <summary>
		/// Has the user made the given discovery type yet?
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <returns>true if discovered already</returns>
		public bool IsDiscovered(DiscoveryType type) {
			return SaveState.Instance.IsDiscovered(this, type);
		}
		
		/// <summary>
		/// Has the user made the discovery for the given life event yet?
		/// </summary>
		/// <param name="lifeEvent">The event to check</param>
		/// <returns>true if discovered already</returns>
		public bool IsDiscovered(LifeEvent lifeEvent) {
			return IsDiscovered(lifeEvent.type);
		}
		
		/// <summary>
		/// Has the user made the given discovery type yet?
		/// </summary>
		/// <param name="lifeEventType">The event type</param>
		/// <returns>true if discovered already</returns>
		public bool IsDiscovered(LifeEventType lifeEventType) {
			var save = SaveState.Instance[this];
			switch(lifeEventType) {
				case LifeEventType.Birth:
					return save.HasFlag(DiscoveryType.Birth);
				case LifeEventType.Death:
					return save.HasFlag(DiscoveryType.Death);
			}

			return false;
		}
	}
}