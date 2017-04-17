using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    public enum ButtonType
    {
        GenericTowerButton,
        CannonTowerButton,
        BatteryTowerButton,
        BlastTowerButton,
        WallButton,
        PortalButton,
        CheeseButton,
        StartButton,
        UpgradeButton,
        SellButton,
        CancelButton,
        NothingButton
    }
    class Button : GameObject
    {
        public ButtonType ButtonType { get; set; }

        public Button(Point position, Texture2D tex, ButtonType buttonType) : base(tex, position)
        {
            ButtonType = buttonType;

            //Hack
            if (buttonType == ButtonType.PortalButton)
            {
                SpriteHeight = 32;
                SpriteWidth = 32;
                currentFrame = 0;
                frameCount = 4;
            }
        }

        private GameObject CreateTowerInstance()
        {
            switch (ButtonType)
            {
                case ButtonType.GenericTowerButton:
                    return new GenericTower(Position, Tex);
                case ButtonType.CannonTowerButton:
                    return new CannonTower(Position, Tex);
                case ButtonType.BatteryTowerButton:
                    return new BatteryTower(Position, Tex);
                case ButtonType.BlastTowerButton:
                    return new BlastTower(Position, Tex);
                default:
                    return null;
            }
        }

        public override bool Update(GameTime gameTime, InputHandler inputHandler)
        {
            if (BoundingBox().Contains(inputHandler.Position) && inputHandler.SelectionOccurring && !inputHandler.SelectionHandled && !GameStats.AttackPhase)
            {
                HandleInput(inputHandler);
                inputHandler.SelectionHandled = true;
            }
            return base.Update(gameTime, inputHandler);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Tex, Position.ToVector2(), new Rectangle(new Point(currentFrame * SpriteWidth, 0), new Point(SpriteWidth, SpriteHeight)), Color, 0, new Vector2(SpriteWidth / 2, SpriteHeight / 2), 1, SpriteEffects.None, 0);
        }

        public override void ShowStats(SpriteBatch batch)
        {
            
        }

        public override Rectangle BoundingBox()
        {
            return new Rectangle(Position.X - SpriteWidth / 2, Position.Y - SpriteHeight / 2, SpriteWidth, SpriteHeight);
        }

        public void HandleInput(InputHandler inputHandler)
        {
            inputHandler.CancelSelection();
            switch (ButtonType)
            {
                case ButtonType.GenericTowerButton:
                case ButtonType.CannonTowerButton:
                case ButtonType.BatteryTowerButton:
                case ButtonType.BlastTowerButton:
                    inputHandler.SelectionContext = SelectionContext.PlacingTower;
                    inputHandler.SelectedObject = CreateTowerInstance();
                    break;
                case ButtonType.WallButton:
                    inputHandler.SelectionContext = SelectionContext.PlacingWall;
                    break;
                case ButtonType.PortalButton:
                    inputHandler.SelectionContext = SelectionContext.PlacingPortalEntrance;
                    break;
                case ButtonType.CheeseButton:
                    inputHandler.SelectionContext = SelectionContext.PlacingCheese;
                    break;
                case ButtonType.SellButton:
                    {
                        if (inputHandler.SelectionContext == SelectionContext.TowerSelected)
                        {
                            Tower t = inputHandler.SelectedObject as Tower;
                            t.Sell();
                            inputHandler.CancelSelection();
                        }
                    }
                    break;
                case ButtonType.UpgradeButton:
                    {
                        Tower t = inputHandler.SelectedObject as Tower;
                        if (GameStats.Gold >= t.Cost)
                        {
                            GameStats.Gold = GameStats.Gold - t.Cost;
                            t.upgrade();
                        }
                        else
                        {
                            MessageLog.NotEnoughGold();
                        }
                    }
                    break;
                case ButtonType.CancelButton:
                    inputHandler.CancelSelection();
                    break;
            }
        }

        private void commentedCode()
        {
            //if (inputHandler.SelectionContext == SelectionContext.TowerSelected)
            //{
            //    Node n = inputHandler.SelectedObject as Node;
            //    if (n.wall && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.TogglingWall))
            //    {
            //        GameStats.Gold = GameStats.Gold + 1;
            //        n.wall = false;
            //        n.defaultSet();
            //        ResourceManager.SellSound.Play();
            //    }
            //    else if (n.portal && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.RemovingPortal))
            //    {
            //        n.portal = false;
            //        n.defaultSet();
            //        n.portalsTo.portal = false;
            //        n.portalsTo.portalsTo = null;
            //        n.portalsTo.defaultSet();
            //        n.portalsTo = null;
            //        GameStats.Gold = GameStats.Gold + 20;
            //        ResourceManager.SellSound.Play();
            //    }
            //    else if (n.cheese && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, this, CheckForPathType.TogglingCheese))
            //    {
            //        GameStats.Gold = GameStats.Gold + 20;
            //        n.cheese = false;
            //        n.defaultSet();
            //        ResourceManager.SellSound.Play();
            //    }
            //}
        }

    }
}
