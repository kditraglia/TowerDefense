using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class GameHUD
    {
        Button startButton;
        Button upgradeButton;
        ActionCard actionCard;

        internal GameHUD()
        {
            startButton = new Button(new Point(350 + 64, 36), ResourceManager.StartButton);
            upgradeButton = new Button(new Point(Constants.GameSize.X - 160, (int)(Constants.GameSize.Y * .55f)), ResourceManager.UpgradeButton);

            List<Button> buttonList = new List<Button>();
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));

            buttonList.Add(new Button(Point.Zero, ResourceManager.GenericTower));
            buttonList.Add(new Button(Point.Zero, ResourceManager.CannonTower));
            buttonList.Add(new Button(Point.Zero, ResourceManager.BatteryTower));
            buttonList.Add(new Button(Point.Zero, ResourceManager.BlastTower));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Wall));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Portal));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Cheese));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block));

            actionCard = new ActionCard(new Point(0, Constants.MapStart.Y), buttonList);
        }

        internal void Update(GameTime gameTime, InputHandler mouse)
        {
            actionCard.Update(gameTime, mouse);

            //if (upgradeButton.BoundingBox().Contains(mouse.Position))
            //{
            //    mouse.HoveredObject = upgradeButton;
            //    mouse.HoveringContext = upgradeButton.HoveringContext;
            //}
            //else if (startButton.BoundingBox().Contains(mouse.Position))
            //{
            //    mouse.HoveredObject = startButton;
            //    mouse.HoveringContext = startButton.HoveringContext;
            //}
        }

        internal void Draw(SpriteBatch batch)
        {
            startButton.Draw(batch);
            actionCard.Draw(batch);

            //if (mouse.SelectionContext == SelectionContext.TowerSelected)
            //{
            //    Tower t = mouse.SelectedObject as Tower;
            //    t?.ShowStats(batch);
            //    upgradeButton.Draw(batch);
            //}
            //if (mouse.HoveringContext != HoveringContext.None)
            //{
            //    mouse.HoveredObject?.ShowStats(batch);
            //}

            batch.DrawString(ResourceManager.GameFont, "GOLD - " + GameStats.Gold + " $", new Vector2(Constants.GameSize.X * .8f, Constants.GameSize.Y * .1f), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            MessageLog.Draw(batch, ResourceManager.GameFont);
        }
    }
}
