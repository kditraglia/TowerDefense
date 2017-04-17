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
        ConstructionCard constructionCard;
        ActionCard actionCard;
        Action startGame;

        internal GameHUD(Action startGame)
        {
            this.startGame = startGame;
            startButton = new Button(new Point(350 + 64, 36), ResourceManager.StartButton, ButtonType.StartButton);

            List<Button> buttonList = new List<Button>();
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));

            buttonList.Add(new Button(Point.Zero, ResourceManager.GenericTower, ButtonType.GenericTowerButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.CannonTower, ButtonType.CannonTowerButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.BatteryTower, ButtonType.BatteryTowerButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.BlastTower, ButtonType.BlastTowerButton));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Wall, ButtonType.WallButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Portal, ButtonType.PortalButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Cheese, ButtonType.CheeseButton));

            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));
            buttonList.Add(new Button(Point.Zero, ResourceManager.Block, ButtonType.NothingButton));

            constructionCard = new ConstructionCard(new Point(0, Constants.MapStart.Y), buttonList);
            actionCard = new ActionCard(new Point(Constants.MapEnd.X + 100, Constants.GameSize.Y - 200));
        }

        internal void Update(GameTime gameTime, InputHandler inputHandler)
        {
            constructionCard.Update(gameTime, inputHandler);
            actionCard.Update(gameTime, inputHandler);
            if (inputHandler.SelectionOccurring)
            {
                if (startButton.BoundingBox().Contains(inputHandler.Position) && !GameStats.AttackPhase)
                {
                    inputHandler.CancelSelection();
                    startGame();
                }
            }
        }

        internal void Draw(SpriteBatch batch)
        {
            startButton.Draw(batch);
            constructionCard.Draw(batch);
            actionCard.Draw(batch);

            batch.DrawString(ResourceManager.GameFont, "GOLD - " + GameStats.Gold + " $", new Vector2(Constants.GameSize.X * .8f, Constants.GameSize.Y * .1f), Color.Black, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            MessageLog.Draw(batch, ResourceManager.GameFont);
        }
    }
}
