using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Potterblatt.Utils {
	/// <summary>
	/// Base class for singleton <see cref="MonoBehaviour"/>s
	/// </summary>
	[SuppressMessage("ReSharper", "StaticMemberInGenericType")]
	public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour {
		/// <summary>The singleton instance</summary>
		private static T instance;

		/// <summary>Lock to prevent multi-thread instantiation of singleton</summary>
		private static readonly object instanceLock = new object();

		/// <summary>
		/// Sets the instance (easier then the find in the instance get
		/// </summary>
		protected virtual void Awake() {
			lock(instanceLock) {
				if(instance == null && this is T) {
					instance = (T) (object) this;
				}
				else {
					Debug.LogError(
						$"This singleton ({GetType()}) is either setup as the wrong type or it already exists, deleting to be safe", gameObject);
					Destroy(this);
				}
			}
		}

		/// <summary>
		/// returns whether the singleton exists
		/// </summary>
		public static bool IsSetup => instance != null;

		/// <summary>
		/// Get the singleton instance of this class. Returns null if there is no-singleton yet
		/// </summary>
		public static T Instance {
			get {
				lock(instanceLock) {
					return instance;
				}
			}
		}

		/// <summary>
		/// If destroyed release the singleton
		/// </summary>
		protected virtual void OnDestroy() {
			lock(instanceLock) {
				if(this == instance) {
					instance = null;
				}
			}
		}
	}
}