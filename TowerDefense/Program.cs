using System;

namespace TowerDefense
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TowerDefense game = new TowerDefense())
            {
                game.Run();
            }
        }
    }
}

