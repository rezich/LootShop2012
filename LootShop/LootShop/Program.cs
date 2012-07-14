using System;
using LootSystem;

namespace LootShop {
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameSession game = new GameSession())
            {
                game.Run();
            }
        }
    }
#endif

	public class Tuple<T1, T2> {
		public T1 Item1 { get; set; }
		public T2 Item2 { get; set; }

		public Tuple(T1 item1, T2 item2) {
			Item1 = item1;
			Item2 = item2;
		}
	} 

}

