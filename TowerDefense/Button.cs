using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Button : GameObject
    {
        //This is sort of a hack to get the button to display the button item's stats given how I have that structured
        GameObject InstanceOfWhatThisButtonCreates;

        public Button(Point position, Texture2D tex, HoveringContext hoveringContext) : base(tex, position, hoveringContext)
        {
            switch (HoveringContext)
            {
                case HoveringContext.ButtonGenericTower:
                    InstanceOfWhatThisButtonCreates = new GenericTower(position, tex);
                    break;
                case HoveringContext.ButtonCannonTower:
                    InstanceOfWhatThisButtonCreates = new CannonTower(position, tex);
                    break;
                case HoveringContext.ButtonBatteryTower:
                    InstanceOfWhatThisButtonCreates = new BatteryTower(position, tex);
                    break;
                case HoveringContext.ButtonBlastTower:
                    InstanceOfWhatThisButtonCreates = new BlastTower(position, tex);
                    break;
                case HoveringContext.ButtonWall:
                    InstanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { wall = true };
                    break;
                case HoveringContext.ButtonPortal:
                    InstanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { portal = true };
                    break;
                case HoveringContext.ButtonCheese:
                    InstanceOfWhatThisButtonCreates = new Node(Point.Zero, Point.Zero, tex) { cheese = true };
                    break;
                case HoveringContext.ButtonUpgrade:
                    break;
                default:
                    break;
            }
        }

        public override void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport)
        {
            InstanceOfWhatThisButtonCreates?.ShowStats(batch, font, viewport);
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
