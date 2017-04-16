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

            Point _tempPos = _position + new Point(10 + 32, 32);
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

        internal void Update(GameTime gameTime, InputHandler inputHandler)
        {
            _buttonList.ForEach(b => b.Update(gameTime, inputHandler));

            if (!GameStats.AttackPhase)
            {
                _buttonList.ForEach(b =>
                {
                    if (b.BoundingBox().Contains(inputHandler.Position) && inputHandler.SelectionOccurring)
                    {
                        inputHandler.SelectionContext = SelectionContext.PlacingTower;
                    }
                });
            }
        }

        internal void Draw(SpriteBatch batch)
        {
            batch.Draw(ResourceManager.Block, new Rectangle(_position, new Point(Constants.MapStart.X, Constants.NodeSize.Y * (Constants.MapSize.Y + 1))), Color.DarkKhaki);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X + 74, _position.Y, 1, Constants.NodeSize.Y * (Constants.MapSize.Y + 1)), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 64, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 128, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 192, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 256, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 320, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 384, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 448, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 512, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 576, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 640, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 704, Constants.MapStart.X, 1), Color.Black);
            batch.Draw(ResourceManager.Block, new Rectangle(_position.X, _position.Y + 768, Constants.MapStart.X, 1), Color.Black);
            _buttonList.ForEach(b => b.Draw(batch));
        }
    }
}
