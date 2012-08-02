using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootShop {
	class PauseScreen : GenericMenu {
		public PauseScreen()
			: base("Paused", true, false) {
				Centered = true;
				Entry saveEntry = new Entry("Save game");
				MenuEntries.Add(saveEntry);
				Entry resumeEntry = new Entry("Resume");
				resumeEntry.Selected += OnCancel;
				resumeEntry.IsCancel = true;
				MenuEntries.Add(resumeEntry);
		}
	}
}
