using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootShop {
	public class TestMenu : GenericMenu {
		public TestMenu()
			: base("Test Menu") {
				Entry itemOne = new Entry("Test");
				Entry itemTwo = new Entry("Test 2");

				MenuEntries.Add(itemOne);
				MenuEntries.Add(itemTwo);
		}
	}
}
