using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Potterblatt.GUI {
	public class RecordRequestDropdown {
		public readonly DropdownField dropdown;
		public readonly VisualElement checkbox;

		public RecordRequestDropdown(VisualElement root, List<string> choices) {
			dropdown = root.Q<DropdownField>();
			checkbox = root.Q<VisualElement>(null, "check-mark");
			checkbox = checkbox.Children().First();

			SetEnabled(false);
			SetChecked(false);

			Choices = choices;
		}

		public void SetEnabled(bool enabled) {
			if(!enabled) {
				SetChecked(false);
			}
				
			dropdown.SetEnabled(enabled);
		}

		public void SetChecked(bool enabled) {
			checkbox.style.display =
				new StyleEnum<DisplayStyle>(enabled ? StyleKeyword.Undefined : StyleKeyword.None);
		}

		public event EventCallback<ChangeEvent<string>> OnChange {
			add => dropdown.RegisterValueChangedCallback(value);
			remove => dropdown.UnregisterValueChangedCallback(value);
		}

		public List<string> Choices {
			get => dropdown.choices;
			set {
				dropdown.choices = value;
				dropdown.choices.Insert(0, "None");
				dropdown.index = 0;
				SetEnabled(dropdown.choices.Count > 0);
			}
		}

		public bool HasValue => dropdown.index > 0 && dropdown.index < dropdown.choices?.Count;
	}
}