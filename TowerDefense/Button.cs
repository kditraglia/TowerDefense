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
        //Set up variables
        public Button(Vector2 position, Texture2D tex, bool highlight, int ID) //Our constructor
        {
            this.position = position; //Position in 2D
            this.tex = tex; //Our texture to draw
            this.highlight = highlight;
            this.ID = ID;
        }
        public void Draw(SpriteBatch batch) //Draw function, same as mousehandler one.
        {
            batch.Draw(tex, position, null, color);
        }


    }
}
