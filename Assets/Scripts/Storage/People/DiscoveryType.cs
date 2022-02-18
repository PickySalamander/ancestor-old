using System;

namespace Potterblatt.Storage.People {
	[Flags]
	public enum DiscoveryType {
		None = 0,
		Name = 1,
		Birth = 2,
		Death = 4,
		Mother = 8,
		Father = 16
	}
}