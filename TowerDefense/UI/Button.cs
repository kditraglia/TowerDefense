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
        NothingButton
    }
    class Button : GameObject
    {
        //This is sort of a hack to get the button to display the button item's stats given how I have that structured
        GameObject instanceOfWhatThisButtonCreates;
        public ButtonType ButtonType { get; set; }

        public Button(Point position, Texture2D tex, ButtonType buttonType) : base(tex, position)
        {
            ButtonType = buttonType;
            switch (ButtonType)
            {
                case ButtonType.GenericTowerButton:
                    instanceOfWhatThisButtonCreates = new GenericTower(position, tex);
                    break;
                case ButtonType.CannonTowerButton:
                    instanceOfWhatThisButtonCreates = new CannonTower(position, tex);
                    break;
                case ButtonType.BatteryTowerButton:
                    instanceOfWhatThisButtonCreates = new BatteryTower(position, tex);
                    break;
                case ButtonType.BlastTowerButton:
                    instanceOfWhatThisButtonCreates = new BlastTower(position, tex);
                    break;
                case ButtonType.WallButton:
                    instanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { wall = true };
                    break;
                case ButtonType.PortalButton:
                    instanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { portal = true };
                    SpriteHeight = 32;
                    SpriteWidth = 32;
                    currentFrame = 0;
                    frameCount = 4;
                    break;
                case ButtonType.CheeseButton:
                    instanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { cheese = true };
                    break;
                default:
                    break;
            }
        }

        public override bool Update(GameTime gameTime, InputHandler inputHandler)
        {

            return base.Update(gameTime, inputHandler);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Tex, Position.ToVector2(), new Rectangle(new Point(currentFrame * SpriteWidth, 0), new Point(SpriteWidth, SpriteHeight)), Color, 0, new Vector2(SpriteWidth / 2, SpriteHeight / 2), 1, SpriteEffects.None, 0);
        }

        public override void ShowStats(SpriteBatch batch)
        {
            instanceOfWhatThisButtonCreates?.ShowStats(batch);
        }

        public override Rectangle BoundingBox()
        {
            return new Rectangle(Position.X - SpriteWidth / 2, Position.Y - SpriteHeight / 2, SpriteWidth, SpriteHeight);
        }

        public void HandleLeftClick(InputHandler inputHandler)
        {
            switch (inputHandler.SelectionContext)
            {
                case SelectionContext.PlacingTower:
                    inputHandler.SelectedObject = instanceOfWhatThisButtonCreates;
                    break;
                case SelectionContext.PlacingWall:
                    break;
                case SelectionContext.PlacingPortalEntrance:
                    break;
                case SelectionContext.PlacingPortalExit:
                    break;
                case SelectionContext.PlacingCheese:

                    break;
                default:
                    break;
            }

            //Tower t = mouse.SelectedObject as Tower;
            //if (GameStats.Gold >= t.Cost)
            //{
            //    GameStats.Gold = GameStats.Gold - t.Cost;
            //    t.upgrade();
            //}
            //else
            //{
            //    MessageLog.NotEnoughGold();
            //}
        }
    }
}
