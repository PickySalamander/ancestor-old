using System.Collections.Generic;
using UnityEngine;

namespace Potterblatt.Utils {
	public struct UsState {
		public static List<UsState> states;

		public readonly string Name;
		public readonly string Abbreviations;

		public UsState(string ab, string name) {
			Name = name;
			Abbreviations = ab;
		}

		public override string ToString() {
			return $"{Abbreviations} - {Name}";
		}

		static UsState() {
			states = new List<UsState>() {
				new UsState("AL", "Alabama"),
				new UsState("AZ", "Arizona"),
				new UsState("AR", "Arkansas"),
				new UsState("CA", "California"),
				new UsState("CO", "Colorado"),
				new UsState("CT", "Connecticut"),
				new UsState("DE", "Delaware"),
				new UsState("DC", "District Of Columbia"),
				new UsState("FL", "Florida"),
				new UsState("GA", "Georgia"),
				new UsState("ID", "Idaho"),
				new UsState("IL", "Illinois"),
				new UsState("IN", "Indiana"),
				new UsState("IA", "Iowa"),
				new UsState("KS", "Kansas"),
				new UsState("KY", "Kentucky"),
				new UsState("LA", "Louisiana"),
				new UsState("ME", "Maine"),
				new UsState("MD", "Maryland"),
				new UsState("MA", "Massachusetts"),
				new UsState("MI", "Michigan"),
				new UsState("MN", "Minnesota"),
				new UsState("MS", "Mississippi"),
				new UsState("MO", "Missouri"),
				new UsState("MT", "Montana"),
				new UsState("NE", "Nebraska"),
				new UsState("NV", "Nevada"),
				new UsState("NH", "New Hampshire"),
				new UsState("NJ", "New Jersey"),
				new UsState("NM", "New Mexico"),
				new UsState("NY", "New York"),
				new UsState("NC", "North Carolina"),
				new UsState("ND", "North Dakota"),
				new UsState("OH", "Ohio"),
				new UsState("OK", "Oklahoma"),
				new UsState("OR", "Oregon"),
				new UsState("PA", "Pennsylvania"),
				new UsState("RI", "Rhode Island"),
				new UsState("SC", "South Carolina"),
				new UsState("SD", "South Dakota"),
				new UsState("TN", "Tennessee"),
				new UsState("TX", "Texas"),
				new UsState("UT", "Utah"),
				new UsState("VT", "Vermont"),
				new UsState("VA", "Virginia"),
				new UsState("WA", "Washington"),
				new UsState("WV", "West Virginia"),
				new UsState("WI", "Wisconsin"),
				new UsState("WY", "Wyoming")
			};
		}

		public static UsState GetRandom() {
			return states[Random.Range(0, states.Count)];
		}
	}
}