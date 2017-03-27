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

        public override void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport)
        {
            instanceOfWhatThisButtonCreates?.ShowStats(batch, font, viewport);
        }

        public override void HandleLeftClick(MouseHandler mouse)
        {
            switch (mouse.HoveringContext)
            {
                case HoveringContext.ButtonGenericTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new GenericTower(mouse.pos, mouse.tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonCannonTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new CannonTower(mouse.pos, mouse.tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonBatteryTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new BatteryTower(mouse.pos, mouse.tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonBlastTower:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectedObject = new BlastTower(mouse.pos, mouse.tex);
                    mouse.SelectionContext = SelectionContext.PlacingTower;
                    break;
                case HoveringContext.ButtonWall:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingWall;
                    break;
                case HoveringContext.ButtonPortal:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingPortalEntrance;
                    break;
                case HoveringContext.ButtonCheese:
                    mouse.UpdateTex(mouse.HoveredObject.Tex);
                    mouse.SelectionContext = SelectionContext.PlacingCheese;
                    break;
                case HoveringContext.ButtonUpgrade:
                    Tower t = mouse.SelectedObject as Tower;
                    if (GameStats.Gold >= t.cost)
                    {
                        GameStats.Gold = GameStats.Gold - t.cost;
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
