using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootSystem {
	public class Shop {
		public enum ShopType {
			StreetVendor,
			NormalShop
		}

		public ShopType Type;
		public int RegularSlots;
		public int WindowSlots;

		public Shop() { }
	}
}
