using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootShop {
	class Program {
		static void Main(string[] args) {
			for (; ; ) {
				Random r = new Random();
				for (int i = 0; i < 5; i++) {
					Item.Generate(r).WriteStatBlock();
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
