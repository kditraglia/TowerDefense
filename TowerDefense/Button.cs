using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Button : GameObject
    {
        public Button(Point position, Texture2D tex, HoveringContext hoveringContext) : base(tex, position, hoveringContext)
        {

        }
    }
}
