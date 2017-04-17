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
        string actionContext = "";

        internal ActionCard(Point position)
        {
            this.position = position;
            buttonList.Add(new Button(position + new Point(0, 100), ResourceManager.UpgradeButton, ButtonType.UpgradeButton));
            buttonList.Add(new Button(position + new Point(80, 100), ResourceManager.SellButton, ButtonType.SellButton));
            buttonList.Add(new Button(position + new Point(160, 100), ResourceManager.CancelButton, ButtonType.CancelButton));
        }

        internal void Update(GameTime gameTime, InputHandler inputHandler)
        {
            buttonList.Clear();
            switch(inputHandler.SelectionContext)
            {
                case SelectionContext.PlacingTower:
                    buttonList.Add(new Button(position + new Point(0, 100), ResourceManager.CancelButton, ButtonType.CancelButton));
                    actionContext = string.Format("Placing a {0}.", (inputHandler.SelectedObject as Tower).Name);
                    break;
                case SelectionContext.TowerSelected:
                    buttonList.Add(new Button(position + new Point(0, 100), ResourceManager.UpgradeButton, ButtonType.UpgradeButton));
                    buttonList.Add(new Button(position + new Point(80, 100), ResourceManager.SellButton, ButtonType.SellButton));
                    buttonList.Add(new Button(position + new Point(160, 100), ResourceManager.CancelButton, ButtonType.CancelButton));
                    actionContext = string.Format("{0} selected.", (inputHandler.SelectedObject as Tower).Name);
                    break;
                default:
                    actionContext = "";
                    break;
            }
            buttonList.ForEach(b => b.Update(gameTime, inputHandler));
        }

        internal void Draw(SpriteBatch batch)
        {
            batch.DrawString(ResourceManager.GameFont, actionContext, position.ToVector2(), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            buttonList.ForEach(b => b.Draw(batch));
        }
    }
}
