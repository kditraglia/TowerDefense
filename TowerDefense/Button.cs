using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Button : GameObject
    {
        public HoveringContext HoveringContext { get; set; }

        public Button(Vector2 position, Texture2D tex, HoveringContext hoveringContext) : base(tex, position)
        {
            HoveringContext = hoveringContext;
        }
    }
}
