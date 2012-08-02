﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace LootSystem {
	public class Campaign {
		public PlayerCharacter PlayerCharacter = new PlayerCharacter();
		public Time Time = new Time();
		public Shop Shop = new Shop();

		public Campaign() {
			Time.Year = 1000;
			Time.Month = 1;
			Time.Day = 1;
			Time.TimeOfDay = 0;
		}
	}
}
