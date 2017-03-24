using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Button
    {
        public Vector2 position;
        public Texture2D tex;
        public bool hovering;
        public bool highlight;
        public int ID;
        public Color color = Color.White;

        public Button(Vector2 position, Texture2D tex, bool highlight, int ID)
        {
            this.position = position;
            this.tex = tex;
            this.highlight = highlight;
            this.ID = ID;
        }
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, position, null, color);
        }


    }
}
