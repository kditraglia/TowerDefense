using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class ActionCard
    {
        List<Button> _buttonList = new List<Button>();
        Point _position;

        internal ActionCard(Point position, List<Button> buttonList)
        {
            _position = position;
            _buttonList = buttonList;

            Point _tempPos = _position + new Point(10 + 32, 10 + 32);
            _buttonList.ForEach(b =>
            {
                b.Position = _tempPos;
                _tempPos.X += 64;
                if (_tempPos.X >= 64 * 2)
                {
                    _tempPos.X = _position.X + 10 + 32;
                    _tempPos.Y += 64;
                }
            });
        }

        internal void Update(GameTime gameTime, MouseHandler mouse)
        {
            _buttonList.ForEach(b => b.Update(gameTime));

            if (!GameStats.AttackPhase)
            {
                _buttonList.ForEach(b =>
                {
                    b.Hovering = b.BoundingBox().Contains(mouse.Position);
                    if (b.Hovering)
                    {
                        mouse.HoveredObject = b;
                        mouse.HoveringContext = b.HoveringContext;
                    }
                });
            }
        }

        internal void Draw(SpriteBatch batch)
        {
            batch.Draw(ResourceManager.Block, new Rectangle(_position, new Point(Constants.MapStart.X, 500)), Color.DarkKhaki);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X + 74, _position.Y, 1, 500), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 10, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 10 + 64, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 10 + 128, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 10 + 192, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 10 + 256, Constants.MapStart.X, 1), Color.Black);
            _buttonList.ForEach(b => b.Draw(batch));
        }
    }
}
