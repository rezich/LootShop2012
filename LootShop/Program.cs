using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootShop {
	class Program {
		static void Main(string[] args) {
			Item.Attribute.Initialize();
			Item.RarityLevel.Initialize();
			Random r = new Random();
			for (; ; ) {
				Item.Generate(1, r).WriteStatBlock();
				Console.WriteLine();
				Console.WriteLine();
				Console.ReadKey();
			}
		}
	}
}
