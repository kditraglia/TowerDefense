using System;
using Foundation;
using UIKit;

namespace TowerDefenseiOS
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static TowerDefense.TowerDefense game;

        internal static void RunGame()
        {
            game = new TowerDefense.TowerDefense();
            game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
