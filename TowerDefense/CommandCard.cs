using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefense
{
    class CommandCard
    {
        public string Title { get; set; }
        public string Damage { get; set; }
        public string AttackSpeed { get; set; }
        public string Range { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }

        public CommandCard(string title, string damage = null, string attackSpeed = null, string range = null, string cost = null, string description = null)
        {
            Title = title;
            Damage = damage;
            AttackSpeed = attackSpeed;
            Range = range;
            Cost = cost;
            Description = description;
        }

        public void Draw(Point pos, SpriteBatch batch)
        {
            List<string> cardList = new List<string>();
            int Y = pos.Y;
            cardList.Add(Title);
            if (Damage != null) cardList.Add("Damage - " + Damage);
            if (AttackSpeed != null) cardList.Add("Attack Speed - " + AttackSpeed);
            if (Range != null) cardList.Add("Range - " + Range);
            if (Cost != null) cardList.Add("Cost - " + Cost);
            if (Description != null) cardList.Add("Description - " + Description);

            foreach (string text in cardList)
            {
                SpriteFont font = text == Title ? ResourceManager.BoldFont : ResourceManager.GameFont;
                int stringlengthX = (int)font.MeasureString(text).X + 10;
                int stringlengthY = (int)font.MeasureString(text).Y + 10;
                Y = Y + stringlengthY;
                batch.DrawString(font, text, new Vector2(pos.X - stringlengthX, Y), Color.Black,
                    0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            }
        }
    }
}
