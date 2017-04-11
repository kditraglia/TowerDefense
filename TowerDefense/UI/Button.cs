using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Button : GameObject
    {
        //This is sort of a hack to get the button to display the button item's stats given how I have that structured
        GameObject instanceOfWhatThisButtonCreates;
        public HoveringContext HoveringContext { get; set; }

        public Button(Point position, Texture2D tex, HoveringContext hoveringContext) : base(tex, position)
        {
            HoveringContext = hoveringContext;
            switch (hoveringContext)
            {
                case HoveringContext.ButtonGenericTower:
                    instanceOfWhatThisButtonCreates = new GenericTower(position, tex);
                    break;
                case HoveringContext.ButtonCannonTower:
                    instanceOfWhatThisButtonCreates = new CannonTower(position, tex);
                    break;
                case HoveringContext.ButtonBatteryTower:
                    instanceOfWhatThisButtonCreates = new BatteryTower(position, tex);
                    break;
                case HoveringContext.ButtonBlastTower:
                    instanceOfWhatThisButtonCreates = new BlastTower(position, tex);
                    break;
                case HoveringContext.ButtonWall:
                    instanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { wall = true };
                    break;
                case HoveringContext.ButtonPortal:
                    instanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { portal = true };
                    SpriteHeight = 32;
                    SpriteWidth = 32;
                    currentFrame = 0;
                    frameCount = 4;
                    break;
                case HoveringContext.ButtonCheese:
                    instanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { cheese = true };
                    break;
                case HoveringContext.ButtonUpgrade:
                    break;
                default:
                    break;
            }
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

        public override void HandleLeftClick(MouseHandler mouse)
        {
            switch (mouse.HoveringContext)
            {
                case HoveringContext.ButtonGenericTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new GenericTower(mouse.Position, mouse.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonCannonTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new CannonTower(mouse.Position, mouse.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonBatteryTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new BatteryTower(mouse.Position, mouse.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonBlastTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new BlastTower(mouse.Position, mouse.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonWall:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingWall;
                    break;
                case HoveringContext.ButtonPortal:
                    mouse.UpdateTex(mouse.HoveredObject.Tex, 32, 32, 4);
                    mouse.SelectionContext = SelectionContext.PlacingPortalEntrance;
                    break;
                case HoveringContext.ButtonCheese:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingCheese;
                    break;
                case HoveringContext.ButtonUpgrade:
                    Tower t = mouse.SelectedObject as Tower;
                    if (GameStats.Gold >= t.Cost)
                    {
                        GameStats.Gold = GameStats.Gold - t.Cost;
                        t.upgrade();
                    }
                    else
                    {
                        MessageLog.NotEnoughGold();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
