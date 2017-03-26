using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    class Button : GameObject
    {
        public Button(Point position, Texture2D tex, HoveringContext hoveringContext) : base(tex, position, hoveringContext)
        {

        }

        public override void ShowStats(SpriteBatch batch, SpriteFont font, Viewport viewport)
        {

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
                    {
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
                    }
                default:
                    break;
            }
        }
    }
}
