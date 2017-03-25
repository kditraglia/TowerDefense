using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class FloatingText
    {
        Point pos;
        SpriteFont font;
        string text;
        byte transparency = 255;
        Color color;

        public FloatingText(Point pos, SpriteFont font, string text)
        {
            this.pos = pos;
            this.font = font;
            this.text = text;
        }

        public bool Update(GameTime gameTime)
        {
            pos.Y--;
            transparency -= 5;
            color = Color.FromNonPremultiplied(200, 50, 50, transparency);
            if (transparency <= 0)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch batch)
        {
            color.A = transparency;
            batch.DrawString(font, text, pos.ToVector2(), color);
        }
    }
}
