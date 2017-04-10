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
        MouseHandler mouse;
        Viewport viewport;
        ActionCard actionCard;

        internal GameHUD(Viewport viewport, MouseHandler mouse)
        {
            this.viewport = viewport;
            this.mouse = mouse;
            startButton = new Button(new Point(10 + 32, (int)(viewport.Height * .2f - 74)), ResourceManager.StartButton, HoveringContext.ButtonStart);
            upgradeButton = new Button(new Point(viewport.Width - 160, (int)(viewport.Height * .55f)), ResourceManager.UpgradeButton, HoveringContext.ButtonUpgrade);

            List<Button> buttonList = new List<Button>();
            buttonList.Add(new Button(Point.Zero, ResourceManager.GenericTower, HoveringContext.ButtonGenericTower));
            buttonList.Add(new Button(Point.Zero, ResourceManager.CannonTower, HoveringContext.ButtonCannonTower));
            buttonList.Add(new Button(Point.Zero, ResourceManager.BatteryTower, HoveringContext.ButtonBatteryTower));
            buttonList.Add(new Button(Point.Zero, ResourceManager.BlastTower, HoveringContext.ButtonBlastTower));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, HoveringContext.None));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, HoveringContext.None));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Wall, HoveringContext.ButtonWall));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Portal, HoveringContext.ButtonPortal));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Cheese, HoveringContext.ButtonCheese));

            actionCard = new ActionCard(new Point(0, 170), buttonList);
        }

        internal void Update(GameTime gameTime, MouseHandler mouse)
        {
            actionCard.Update(gameTime, mouse);

            if (upgradeButton.BoundingBox().Contains(mouse.Position))
            {
                mouse.HoveredObject = upgradeButton;
                mouse.HoveringContext = upgradeButton.HoveringContext;
            }
            else if (startButton.BoundingBox().Contains(mouse.Position))
            {
                mouse.HoveredObject = startButton;
                mouse.HoveringContext = startButton.HoveringContext;
            }
        }

        internal void Draw(SpriteBatch batch)
        {
            startButton.Draw(batch);
            actionCard.Draw(batch);

            if (mouse.SelectionContext == SelectionContext.TowerSelected)
            {
                Tower t = mouse.SelectedObject as Tower;
                t?.ShowStats(batch, viewport);
                upgradeButton.Draw(batch);
            }
            if (mouse.HoveringContext != HoveringContext.None)
            {
                mouse.HoveredObject?.ShowStats(batch, viewport);
            }

            batch.DrawString(ResourceManager.GameFont, "GOLD - " + GameStats.Gold + " $", new Vector2(viewport.Width * .8f, viewport.Height * .1f), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            MessageLog.Draw(batch, ResourceManager.GameFont, viewport);
        }
    }
}
