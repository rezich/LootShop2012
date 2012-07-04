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
            using (LootShop game = new LootShop())
            {
                game.Run();
            }
        }
    }
#endif
}

