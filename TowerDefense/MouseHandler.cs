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
        private MouseState mouseState;
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
        //We create variables

        public MouseHandler(Vector2 pos, Texture2D tex)
        {
            this.pos = pos; //Inital pos (0,0)
            this.tex = tex; //Cursor texture
        }
        public void Update()
        {
            mouseState = Mouse.GetState(); //Needed to find the most current mouse states.
            this.pos.X = mouseState.X; //Change x pos to mouseX
            this.pos.Y = mouseState.Y; //Change y pos to mouseY
        }
        public void UpdateTex(Texture2D tex)
        {
            this.tex = tex;
        }
        //Drawing function to be called in the main Draw function.
        public void Draw(SpriteBatch batch) //SpriteBatch to use.
        {
            batch.Draw(this.tex, this.pos, Color.White); //Draw it using the batch.
        }

        public bool ButtonClick(Button b)
        {
            if (this.pos.X >= b.position.X // To the right of the left side
            && this.pos.X < b.position.X + b.tex.Width //To the left of the right side
            && this.pos.Y > b.position.Y //Below the top side
            && this.pos.Y < b.position.Y + b.tex.Height) //Above the bottom side
                return true; //We are; return true.
            else
                return false; //We're not; return false.
        }
        public bool NodeClick(Node n)
        {
            if (this.pos.X >= n.actualPos.X // To the right of the left side
            && this.pos.X < n.actualPos.X + n.tex.Width //To the left of the right side
            && this.pos.Y > n.actualPos.Y //Below the top side
            && this.pos.Y < n.actualPos.Y + n.tex.Height) //Above the bottom side
                return true; //We are; return true.
            else
                return false; //We're not; return false.
        }
        public bool TowerClick(Tower t)
        {
            if (this.pos.X >= t.position.X // To the right of the left side
            && this.pos.X < t.position.X + t.tex.Width //To the left of the right side
            && this.pos.Y > t.position.Y //Below the top side
            && this.pos.Y < t.position.Y + t.tex.Height) //Above the bottom side
                return true; //We are; return true.
            else
                return false; //We're not; return false.
        }
        public bool EnemyClick(Enemy e)
        {
            if (this.pos.X >= e.pos.X // To the right of the left side
            && this.pos.X < e.pos.X + e.tex.Width //To the left of the right side
            && this.pos.Y > e.pos.Y //Below the top side
            && this.pos.Y < e.pos.Y + e.tex.Height) //Above the bottom side
                return true; //We are; return true.
            else
                return false; //We're not; return false.
        }
    }
}
