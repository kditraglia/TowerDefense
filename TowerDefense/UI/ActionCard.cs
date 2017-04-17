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
        List<Button> buttonList = new List<Button>();
        Point position;

        internal ActionCard(Point position)
        {
            this.position = position;
            buttonList.Add(new Button(position + new Point(0, 0), ResourceManager.UpgradeButton, ButtonType.UpgradeButton));
            buttonList.Add(new Button(position + new Point(80, 0), ResourceManager.SellButton, ButtonType.SellButton));
            buttonList.Add(new Button(position + new Point(160, 0), ResourceManager.CancelButton, ButtonType.CancelButton));
        }

        internal void Update(GameTime gameTime, InputHandler inputHandler)
        {
            buttonList.Clear();
            switch(inputHandler.SelectionContext)
            {
                case SelectionContext.PlacingTower:
                    buttonList.Add(new Button(position + new Point(0, 0), ResourceManager.CancelButton, ButtonType.CancelButton));
                    break;
                case SelectionContext.TowerSelected:
                    buttonList.Add(new Button(position + new Point(0, 0), ResourceManager.UpgradeButton, ButtonType.UpgradeButton));
                    buttonList.Add(new Button(position + new Point(80, 0), ResourceManager.SellButton, ButtonType.SellButton));
                    buttonList.Add(new Button(position + new Point(160, 0), ResourceManager.CancelButton, ButtonType.CancelButton));
                    break;
            }
            buttonList.ForEach(b => b.Update(gameTime, inputHandler));
        }

        internal void Draw(SpriteBatch batch)
        {
            buttonList.ForEach(b => b.Draw(batch));
        }
    }
}
