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
            switch (ButtonType)
            {
                case ButtonType.GenericTowerButton:
                case ButtonType.CannonTowerButton:
                case ButtonType.BatteryTowerButton:
                case ButtonType.BlastTowerButton:
                    inputHandler.CancelSelection();
                    inputHandler.SelectionContext = SelectionContext.PlacingTower;
                    inputHandler.SelectedObject = CreateTowerInstance();
                    break;
                case ButtonType.WallButton:
                    inputHandler.CancelSelection();
                    inputHandler.SelectionContext = SelectionContext.PlacingWall;
                    break;
                case ButtonType.PortalButton:
                    inputHandler.CancelSelection();
                    inputHandler.SelectionContext = SelectionContext.PlacingPortalEntrance;
                    break;
                case ButtonType.CheeseButton:
                    inputHandler.CancelSelection();
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
                        if (inputHandler.SelectionContext == SelectionContext.NodeSelected)
                        {
                            Node n = inputHandler.SelectedObject as Node;
                            n.Sell();
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
    }
}
