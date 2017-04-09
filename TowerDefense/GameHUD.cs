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
        List<Button> buttonlist = new List<Button>();
        Button startButton;
        Button upgradeButton;
        MouseHandler mouse;
        Viewport viewport;

        //TODO nix this viewport nonsense
        internal GameHUD(Viewport viewport, MouseHandler mouse)
        {
            this.viewport = viewport;
            this.mouse = mouse;
            startButton = new Button(new Point(10 + 32, (int)(viewport.Height * .2f - 74)), ResourceManager.StartButton, HoveringContext.ButtonStart);
            upgradeButton = new Button(new Point(viewport.Width - 160, (int)(viewport.Height * .55f)), ResourceManager.UpgradeButton, HoveringContext.ButtonUpgrade);
            buttonlist.Add(new Button(new Point(10, (int)(viewport.Height * .2f)), ResourceManager.GenericTower, HoveringContext.ButtonGenericTower));
            buttonlist.Add(new Button(new Point(10 + 64, (int)(viewport.Height * .2f)), ResourceManager.CannonTower, HoveringContext.ButtonCannonTower));
            buttonlist.Add(new Button(new Point(10, (int)(viewport.Height * .2f) + 64), ResourceManager.BatteryTower, HoveringContext.ButtonBatteryTower));
            buttonlist.Add(new Button(new Point(10 + 64, (int)(viewport.Height * .2f) + 64), ResourceManager.BlastTower, HoveringContext.ButtonBlastTower));
            buttonlist.Add(new Button(new Point(10 + 16, (int)(viewport.Height * .5f)), ResourceManager.Wall, HoveringContext.ButtonWall));
            buttonlist.Add(new Button(new Point(10 + 64, (int)(viewport.Height * .5f)), ResourceManager.Portal, HoveringContext.ButtonPortal));
            buttonlist.Add(new Button(new Point(10 + 16, (int)(viewport.Height * .56f)), ResourceManager.Cheese, HoveringContext.ButtonCheese));
        }

        internal void Update(GameTime gameTime)
        {
            buttonlist.ForEach(b => b.Update(gameTime));
        }

        internal void Draw(SpriteBatch batch)
        {
            startButton.Draw(batch);
            buttonlist.ForEach(b => b.Draw(batch));

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

        internal void HandleMouseHover(MouseHandler mouse)
        {
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

            if (!GameStats.AttackPhase)
            {
                buttonlist.ForEach(b =>
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
    }
}
