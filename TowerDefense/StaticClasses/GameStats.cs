
namespace TowerDefense
{
    static class GameStats
    {
        internal static int Gold { get; set; }
        internal static bool AttackPhase { get; set;}
        internal static bool PlayerLoses { get; set; }
        internal static int Level { get; set; }

        static GameStats()
        {
            Gold = Constants.StartingGold;
            AttackPhase = false;
            PlayerLoses = false;
            Level = 0;
        }
    }
}
