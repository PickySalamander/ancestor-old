using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	/// <summary>
	/// Controller for a drop down on the record request page
	/// </summary>
	/// <seealso cref="RecordRequestPage"/>
	public class RecordRequestDropdown {
		/// <summary>The dropdown on the page</summary>
		public readonly DropdownField dropdown;

		/// <summary>Checkbox on the page that indicates if the information was set correctly</summary>
		public readonly VisualElement checkbox;

		/// <summary>
		/// Setup the dropdown with the choices given
		/// </summary>
		/// <param name="root">parent of the row on the page to get the elements from</param>
		/// <param name="choices">The choices to show on the page</param>
		public RecordRequestDropdown(VisualElement root, List<string> choices) {
			//get the elements
			dropdown = root.Q<DropdownField>();
			checkbox = root.Q<VisualElement>(null, "check-mark");
			checkbox = checkbox.Children().First();

			//set defaults
			SetEnabled(false);
			SetChecked(false);

			//set the choices
			Choices = choices;
		}

		/// <summary>
		/// Set whether the user is allowed to work with the dropdown
		/// </summary>
		/// <param name="enabled">is the dropdown is able to open</param>
		public void SetEnabled(bool enabled) {
			//reset the checked value if disabled
			if(!enabled) {
				SetChecked(false);
			}

			//set the dropdown's enabled state
			dropdown.SetEnabled(enabled);
		}

		/// <summary>Set whether the check is visible</summary>
		public void SetChecked(bool enabled) {
			checkbox.style.display =
				new StyleEnum<DisplayStyle>(enabled ? StyleKeyword.Undefined : StyleKeyword.None);
		}

		/// <summary>
		/// events for when the dropdown changes
		/// </summary>
		public event EventCallback<ChangeEvent<string>> OnChange {
			add => dropdown.RegisterValueChangedCallback(value);
			remove => dropdown.UnregisterValueChangedCallback(value);
		}

		/// <summary>The choices to show on the page</summary>
		public List<string> Choices {
			get => dropdown.choices;
			set {
				//put the choices in the dropdown and enable if there are multiples
				dropdown.choices = value;
				dropdown.choices.Insert(0, "None");
				dropdown.index = 0;
				SetEnabled(dropdown.choices.Count > 1);
			}
		}

		/// <summary>Whether or not the dropdown has a value set</summary>
		public bool HasValue => dropdown.index > 0 && dropdown.index < dropdown.choices?.Count;
	}
}