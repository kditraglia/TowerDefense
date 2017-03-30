using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerDefense
{
    public class TowerDefense : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch batch;
        MouseHandler mouse;
        Viewport viewport;

        GameEngine gameEngine = new GameEngine();
        GameMap gameMap = new GameMap();
        List<Button> buttonlist = new List<Button>();
        Button startButton;
        Button upgradeButton;

        public TowerDefense()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1020;
            graphics.PreferredBackBufferHeight = 800;
        }


        protected override void Initialize()
        {
            viewport = graphics.GraphicsDevice.Viewport;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            ResourceManager.InitializeTextures(Content);

            startButton = new Button(new Point(10 + 32, (int)(viewport.Height * .2f - 74)), ResourceManager.StartButton, HoveringContext.ButtonStart);
            upgradeButton = new Button(new Point(viewport.Width - 160, (int)(viewport.Height * .55f)), ResourceManager.UpgradeButton, HoveringContext.ButtonUpgrade);
            buttonlist.Add(new Button(new Point(10, (int)(viewport.Height * .2f)), ResourceManager.GenericTower, HoveringContext.ButtonGenericTower));
            buttonlist.Add(new Button(new Point(10 + 64, (int)(viewport.Height * .2f)), ResourceManager.CannonTower, HoveringContext.ButtonCannonTower));
            buttonlist.Add(new Button(new Point(10, (int)(viewport.Height * .2f) + 64), ResourceManager.BatteryTower, HoveringContext.ButtonBatteryTower));
            buttonlist.Add(new Button(new Point(10 + 64, (int)(viewport.Height * .2f) + 64), ResourceManager.BlastTower, HoveringContext.ButtonBlastTower));
            buttonlist.Add(new Button(new Point(10 + 16, (int)(viewport.Height * .5f)), ResourceManager.Wall, HoveringContext.ButtonWall));
            buttonlist.Add(new Button(new Point(10 + 64, (int)(viewport.Height * .5f)), ResourceManager.Portal, HoveringContext.ButtonPortal));
            buttonlist.Add(new Button(new Point(10 + 16, (int)(viewport.Height * .56f)), ResourceManager.Cheese, HoveringContext.ButtonCheese));

            mouse = new MouseHandler(Point.Zero, ResourceManager.DefaultCursor);
        }

        protected override void Update(GameTime gameTime)
        {
            HandleMouse();
            mouse.Update();

            gameEngine.Update(gameTime);
           base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            batch.Begin();

            gameMap.Draw(batch);
            startButton.Draw(batch);
            buttonlist.ForEach(b => b.Draw(batch));
            gameEngine.Draw(batch);

            if (mouse.SelectionContext == SelectionContext.TowerSelected)
            {
                Tower t = mouse.SelectedObject as Tower;
                t?.ShowStats(batch, viewport);
                upgradeButton.Draw(batch);
            }
            else if (mouse.HoveringContext != HoveringContext.None)
            {
                mouse.HoveredObject?.ShowStats(batch, viewport);
            }

            batch.DrawString(ResourceManager.GameFont, "GOLD - " + GameStats.Gold + " $", new Vector2(viewport.Width * .8f, viewport.Height * .1f), Color.Black,
                    0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            MessageLog.Draw(batch, ResourceManager.GameFont, viewport);

            batch.Draw(ResourceManager.TopBanner, new Vector2(viewport.Width / 2 - ResourceManager.TopBanner.Width / 2, 0), null, Color.White);
            batch.Draw(ResourceManager.BottomBanner, new Vector2(viewport.Width / 2 - ResourceManager.TopBanner.Width / 4, viewport.Height - ResourceManager.BottomBanner.Height), null, Color.White);
            mouse.Draw(batch);

            batch.End();

            base.Draw(gameTime);
        }

        private void HandleMouse()
        {
            if (GameStats.PlayerLoses)
            {
                if (mouse.MouseState.LeftButton == ButtonState.Pressed)
                {
                    //Exit();
                }
                return;
            }

            if (mouse.MouseState.LeftButton == ButtonState.Pressed && startButton.ContainsPoint(mouse.pos) && !GameStats.AttackPhase && mouse.SelectionContext == SelectionContext.None)
            {
                mouse.UpdateTex(ResourceManager.DefaultCursor);
                gameEngine.StartLevel();
            }
            HandleMouseHover();
            if (!GameStats.AttackPhase)
            {
                if (mouse.MouseState.LeftButton == ButtonState.Pressed)
                {
                    //I took this out of the HandleLeftClick method, since it's the only thing I want to allow holding the button down
                    //It doesn't feel right to not be able to drag the mouse when making walls
                    if (mouse.SelectionContext == SelectionContext.PlacingWall && mouse.HoveringContext == HoveringContext.EmptyNode)
                    {
                        Node n = mouse.HoveredObject as Node;
                        if (!gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, mouse, CheckForPathType.TogglingWall))
                        {
                            MessageLog.IllegalPosition();
                        }
                        else if (GameStats.Gold >= 1)
                        {
                            n.wall = true;
                            n.UpdateTex(mouse.tex);
                            GameStats.Gold = GameStats.Gold - 1;
                            ResourceManager.WallSound.Play();
                        }
                        else
                        {
                            MessageLog.NotEnoughGold();
                        }
                    }
                }
                if (mouse.MouseState.LeftButton == ButtonState.Pressed && !mouse.MouseClicked)
                {
                    HandleLeftClick();
                    mouse.MouseClicked = true;
                }
                if (mouse.MouseState.RightButton == ButtonState.Pressed && !mouse.MouseClicked)
                {
                    HandleRightClick();
                    mouse.MouseClicked = true;
                }
                if (mouse.MouseState.LeftButton == ButtonState.Released && mouse.MouseState.RightButton == ButtonState.Released)
                {
                    mouse.MouseClicked = false;
                }
            }
        }

        private void HandleMouseHover()
        {
            mouse.HoveringContext = HoveringContext.None;
            mouse.HoveredObject = null;

            if (upgradeButton.BoundingBox().Contains(mouse.pos))
            {
                mouse.HoveredObject = upgradeButton;
                mouse.HoveringContext = upgradeButton.HoveringContext;
            }

            gameEngine.HandleMouseHover(mouse);
            gameMap.HandleMouseHover(mouse);

            if (!GameStats.AttackPhase)
            {
                buttonlist.ForEach(b =>
                {
                    b.Hovering = b.BoundingBox().Contains(mouse.pos);
                    if (b.Hovering)
                    {
                        mouse.HoveredObject = b;
                        mouse.HoveringContext = b.HoveringContext;
                    }
                });
            }
        }

        private void HandleLeftClick()
        {
            if (mouse.HoveredObject != null)
            {
                mouse.HoveredObject.HandleLeftClick(mouse);
            }
            gameEngine.HandleLeftClick(mouse);

            if (mouse.SelectionContext == SelectionContext.PlacingPortalEntrance && mouse.HoveringContext == HoveringContext.EmptyNode)
            {
                Node n = mouse.HoveredObject as Node;
                n.portal = true;
                n.UpdateTex(mouse.tex);
                mouse.PortalEntrance = n;
                mouse.SelectionContext = SelectionContext.PlacingPortalExit;
            }
            else if (mouse.SelectionContext == SelectionContext.PlacingPortalExit && mouse.HoveringContext == HoveringContext.EmptyNode)
            {
                Node portalExit = mouse.HoveredObject as Node;
                Node portalEntrance = mouse.PortalEntrance;
                if (!gameMap.CheckForPath(portalExit.simplePos.X, portalExit.simplePos.Y, mouse, CheckForPathType.AddingPortal))
                {
                    MessageLog.IllegalPosition();
                }
                else if (GameStats.Gold >= 20)
                {
                    portalExit.portal = true;
                    portalExit.UpdateTex(mouse.tex);
                    portalExit.portalsTo = portalEntrance;
                    portalEntrance.portalsTo = portalExit;
                    mouse.SelectionContext = SelectionContext.PlacingPortalEntrance;
                    GameStats.Gold = GameStats.Gold - 20;
                }
                else
                {
                    MessageLog.NotEnoughGold();
                }
            }
            else if (mouse.SelectionContext == SelectionContext.PlacingCheese && mouse.HoveringContext == HoveringContext.EmptyNode)
            {
                Node n = mouse.HoveredObject as Node;
                if (!gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, mouse, CheckForPathType.TogglingCheese))
                {
                    MessageLog.IllegalPosition();
                }
                else if (GameStats.Gold >= 20)
                {
                    n.cheese = true;
                    n.UpdateTex(mouse.tex);
                    GameStats.Gold = GameStats.Gold - 20;
                    ResourceManager.WallSound.Play();
                }
                else
                {
                    MessageLog.NotEnoughGold();
                }
            }
        }

        private void HandleRightClick()
        {
            gameEngine.HandleRightClick(mouse);

            if (mouse.SelectionContext == SelectionContext.PlacingPortalExit)
            {
                mouse.PortalEntrance.portal = false;
                mouse.PortalEntrance.defaultSet();
                mouse.PortalEntrance = null;
            }
            else if (mouse.HoveringContext == HoveringContext.FilledNode && mouse.SelectionContext == SelectionContext.None)
            {
                Node n = mouse.HoveredObject as Node;
                if (n.wall && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, mouse, CheckForPathType.TogglingWall))
                {
                    GameStats.Gold = GameStats.Gold + 1;
                    n.wall = false;
                    n.defaultSet();
                    ResourceManager.SellSound.Play();
                }
                else if (n.portal && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, mouse, CheckForPathType.RemovingPortal))
                {
                    n.portal = false;
                    n.defaultSet();
                    n.portalsTo.portal = false;
                    n.portalsTo.portalsTo = null;
                    n.portalsTo.defaultSet();
                    n.portalsTo = null;
                    GameStats.Gold = GameStats.Gold + 20;
                    ResourceManager.SellSound.Play();
                }
                else if (n.cheese && gameMap.CheckForPath(n.simplePos.X, n.simplePos.Y, mouse, CheckForPathType.TogglingCheese))
                {
                    GameStats.Gold = GameStats.Gold + 20;
                    n.cheese = false;
                    n.defaultSet();
                    ResourceManager.SellSound.Play();
                }
            }

            mouse.UpdateTex(ResourceManager.DefaultCursor);
            mouse.SelectionContext = SelectionContext.None;
        }
    }
}
