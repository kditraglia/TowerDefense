using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Button : GameObject
    {
        public bool hovering;
        public bool highlight;
        public int ID;

        public Button(Vector2 position, Texture2D tex, bool highlight, int ID) : base(tex, position)
        {
            this.highlight = highlight;
            this.ID = ID;
        }
    }
}
