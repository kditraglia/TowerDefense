using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    static class MessageLog
    {
        static String string1 = "";

        public static void Draw(SpriteBatch batch, SpriteFont font, Viewport viewport)
        {
            int stringlength1 = (int)font.MeasureString(string1).X + 10;
            int stringlength2 = (int)font.MeasureString(string1).Y + 10;
            batch.DrawString(font, string1, new Vector2(viewport.Width - stringlength1, viewport.Height - stringlength2), Color.Black,
                0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
        }

        public static void Level( int level)
        {
            string1 = "Level - " + level;
        }
        public static void GameOver()
        {
            string1 = "GAME OVER!";
        }
        public static void NotEnoughGold()
        {
            string1 = "Insufficient Gold";
        }
        public static void IllegalPosition()
        {
            string1 = "Cannot Place Here";
        }
        public static void LevelComplete(int gold, int level)
        {
            string1 = "level - " + level + " complete!  + " + gold + "$";
        }
    }
}
