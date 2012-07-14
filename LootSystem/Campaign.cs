using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootSystem {
	public class Campaign {
		public PlayerCharacter PlayerCharacter = new PlayerCharacter();
		public Time Time = new Time();

		public Campaign() {
			Time.Year = 1000;
			Time.Month = 1;
			Time.Day = 1;
			Time.TimeOfDay = 0;
		}
	}
}
