using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootShop {
	class Program {
		static void Main(string[] args) {
			Random r = new Random();
			for (; ; ) {
				for (int i = 0; i < 5; i++) {
					Item item = Item.Generate(1, r);
					//item.Attributes.Add(Item.Attribute.Names.Strength, 40);
					item.WriteStatBlock();
					//Item.Generate(r).WriteStatBlock();
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine();
				}
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("----------");
				Console.ReadKey();
				Console.WriteLine();
				Console.ResetColor();
			}
		}
	}
}
