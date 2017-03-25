using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense
{
    class MouseHandler
    {
        public Vector2 pos;
        public Texture2D tex;
        public MouseState mouseState;
        public int towerID = 0;
        public bool highlight = true;
        public bool hovering = false;
        public bool clicking = false;
        public bool clicked = false;
        public bool wallClicked = false;
        public bool portalClicked = false;
        public bool rClicking = false;
        public bool portalComplete = true;
        public Tower towerSelected = null;
        public Tower towerClicked = null;
        public Tower towerHovered = null;
        public Button buttonHovered = null;
        public Node nodeHovered = null;
        public Node portalLocation = null;
        public Enemy enemyHovered = null;

        public MouseHandler(Vector2 pos, Texture2D tex)
        {
            this.pos = pos;
            this.tex = tex;
        }
        public void Update()
        {
            mouseState = Mouse.GetState();
            this.pos.X = mouseState.X;
            this.pos.Y = mouseState.Y;
        }
        public void UpdateTex(Texture2D tex)
        {
            this.tex = tex;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(this.tex, this.pos, Color.White);
        }
    }
}
