using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public class Node
    {
        public Vector2 actualPos;
        public Vector2 simplePos;
        public Texture2D tex;
        public Texture2D tex2;
        public int cost;
        public Node parent;
        public bool wall = false;
        public bool hovering;
        public bool portal = false;
        public Color color = Color.White;
        public Node portalsTo = null;
        public Node(Vector2 actualPos, Vector2 simplePos, Texture2D tex) 
        {
            this.actualPos = actualPos;
            this.simplePos = simplePos;
            this.tex = tex; 
        }
        public void Draw(SpriteBatch batch) 
        {
            batch.Draw(tex, actualPos, null, color);
            if ( tex2 != null )
                batch.Draw(tex2, actualPos, null, color);
        }
        public void UpdateTex(Texture2D tex)
        {
            this.tex2 = tex;
        }
        public void defaultSet()
        {
            this.tex2 = null;
        }

    }
}
