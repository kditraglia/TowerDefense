
namespace TowerDefense
{
    static class GameStats
    {
        public static int Gold { get; set; }

        static GameStats()
        {
            Gold = Constants.StartingGold;
        }
    }
}
