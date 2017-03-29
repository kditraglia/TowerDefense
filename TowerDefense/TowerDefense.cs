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
        Button startButton;
        Button upgradeButton;
        List<Button> buttonlist = new List<Button>();
        List<Tower> towerlist = new List<Tower>();
        List<Enemy> enemylist = new List<Enemy>();
        List<Projectile> projectilelist = new List<Projectile>();
        List<FloatingText> floatingTextList = new List<FloatingText>();
        Node[,] nodes = new Node[Constants.MapSize.X + 1, Constants.MapSize.Y + 1];
        bool attackPhase = false;
        bool playerLoses = false;
        int level = 0;
        double lastSpawnedTime = 0;

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

            CreateMap();

            mouse = new MouseHandler(Point.Zero, ResourceManager.DefaultCursor);
        }

        //This map stuff should eventually live elsewhere
        private void CreateMap()
        {
            int actualY = Constants.MapStart.Y;
            int actualX = Constants.MapStart.X;
            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                for (int j = 0; j <= Constants.MapSize.Y; j++)
                {
                    nodes[i, j] = new Node(new Point(actualX, actualY), new Point(i, j), ResourceManager.Grass);
                    actualY += Constants.NodeSize.Y;
                }
                actualY = Constants.MapStart.Y;
                actualX += Constants.NodeSize.X;
            }
        }

        private bool MouseInGameBounds()
        {
            int topY = Constants.MapStart.Y;
            int leftX = Constants.MapStart.X;

            return mouse.pos.Y > topY && mouse.pos.X > leftX &&
                        mouse.pos.Y < topY + (Constants.MapSize.Y * Constants.NodeSize.Y) &&
                        mouse.pos.X < leftX + (Constants.MapSize.X * Constants.NodeSize.X);
        }
        //

        protected override void Update(GameTime gameTime)
        {
            HandleMouse();
            mouse.Update();

            if (attackPhase)
            {
                foreach (Enemy e in enemylist)
                {
                    if (!e.spawned && (gameTime.TotalGameTime.TotalSeconds - lastSpawnedTime) > e.spawnRate)
                    {
                        e.spawn();
                        lastSpawnedTime = gameTime.TotalGameTime.TotalSeconds;
                    }
                }
                List<Enemy> temp = new List<Enemy>();
                foreach (Enemy e in enemylist)
                {
                    if (e.dead)
                    {
                        temp.Add(e);
                    }
                    e.move();
                    if (e.lose)
                    {
                        MessageLog.GameOver();
                        playerLoses = true;
                    }
                }
                foreach (Enemy e in temp)
                {
                    enemylist.Remove(e);
                }
                foreach (Tower t in towerlist)
                {
                    projectilelist = t.Attack(enemylist, projectilelist, gameTime.TotalGameTime.TotalSeconds, (d, p) =>
                    {
                        floatingTextList.Add(new FloatingText(p, ResourceManager.GameFont, d.ToString()));
                    });
                }
                projectilelist.RemoveAll(p => p.Move());
            }

            if (enemylist.Count == 0 && attackPhase)
            {
                attackPhase = false;
                projectilelist.Clear();
                MessageLog.LevelComplete(level * 2 + (int)(GameStats.Gold * .05f), level);
                GameStats.Gold = GameStats.Gold + (level * 2 + (int)(GameStats.Gold * .05f));
            }

            floatingTextList.RemoveAll(f => f.Update(gameTime));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            batch.Begin();
            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                for (int j = 0; j <= Constants.MapSize.Y; j++)
                {
                    nodes[i, j].Draw(batch);
                }
            }

            startButton.Draw(batch);
            enemylist.ForEach(e => e.Draw(batch));
            buttonlist.ForEach(b => b.Draw(batch));
            towerlist.ForEach(t => t.Draw(batch));
            projectilelist.ForEach(p => p.Draw(batch));
            floatingTextList.ForEach(f => f.Draw(batch));

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
            if (playerLoses)
            {
                if (mouse.MouseState.LeftButton == ButtonState.Pressed)
                {
                    Exit();
                }
                return;
            }

            if (mouse.MouseState.LeftButton == ButtonState.Pressed && startButton.ContainsPoint(mouse.pos) && !attackPhase && mouse.SelectionContext == SelectionContext.None)
            {
                StartLevel();
            }
            HandleMouseHover();
            if (!attackPhase)
            {
                if (mouse.MouseState.LeftButton == ButtonState.Pressed)
                {
                    //I took this out of the HandleLeftClick method, since it's the only thing I want to allow holding the button down
                    //It doesn't feel right to not be able to drag the mouse when making walls
                    if (mouse.SelectionContext == SelectionContext.PlacingWall && mouse.HoveringContext == HoveringContext.EmptyNode)
                    {
                        Node n = mouse.HoveredObject as Node;
                        if (!CheckForPath(n.simplePos.X, n.simplePos.Y, CheckForPathType.TogglingWall))
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

            enemylist.ForEach(e =>
            {
                e.Hovering = e.BoundingBox().Contains(mouse.pos);
                if (e.Hovering)
                {
                    mouse.HoveredObject = e;
                    mouse.HoveringContext = HoveringContext.Enemy;
                }
            });

            if (!attackPhase)
            {
                foreach (Node n in nodes)
                {
                    n.Hovering = n.BoundingBox().Contains(mouse.pos) && mouse.SelectionContext != SelectionContext.PlacingTower;
                    if (n.Hovering)
                    {
                        mouse.HoveredObject = n;
                        mouse.HoveringContext = n.wall || n.portal || n.cheese ? HoveringContext.FilledNode : HoveringContext.EmptyNode;
                    }
                }

                buttonlist.ForEach(b =>
                {
                    b.Hovering = b.BoundingBox().Contains(mouse.pos);
                    if (b.Hovering)
                    {
                        mouse.HoveredObject = b;
                        mouse.HoveringContext = b.HoveringContext;
                    }
                });
                towerlist.ForEach(t =>
                {
                    t.Hovering = t.BoundingBox().Contains(mouse.pos) && mouse.SelectionContext == SelectionContext.None;
                    if (t.Hovering)
                    {
                        mouse.HoveredObject = t;
                        mouse.HoveringContext = HoveringContext.Tower;
                    }
                });
            }
        }

        private void StartLevel()
        {
            mouse.UpdateTex(ResourceManager.DefaultCursor);
            attackPhase = true;
            level++;
            MessageLog.Level(level);
            Random rand = new Random();
            double num = rand.NextDouble();
            if (num < .3)
            {
                for (int i = 0; i < (15 + level); i++)
                {
                    enemylist.Add(new Enemy(level * 8, 2, ResourceManager.Enemy, nodes, "Malaria", 1.0f, .4f));
                }
            }
            else if (num < .6)
            {
                for (int i = 0; i < (30 + 2 * level); i++)
                {
                    enemylist.Add(new Enemy(level * 4, 2, ResourceManager.Enemy, nodes, "Tuberculosis", .33f, .2f));
                }
            }
            else
            {
                for (int i = 0; i < (5 + level / 2); i++)
                {
                    enemylist.Add(new Enemy(level * 16, 1, ResourceManager.Enemy, nodes, "AIDS", 1.5f, .8f));
                }
            }
        }

        private void HandleLeftClick()
        {
            if (mouse.HoveredObject != null)
            {
                mouse.HoveredObject.HandleLeftClick(mouse);
            }
            if (mouse.SelectionContext == SelectionContext.PlacingTower && MouseInGameBounds())
            {
                Tower t = mouse.SelectedObject as Tower;
                if (GameStats.Gold >= t.Cost)
                {
                    GameStats.Gold = GameStats.Gold - t.Cost;
                    towerlist.Add(t);
                    t.Position = mouse.pos;
                    mouse.SelectedObject = null;
                    mouse.SelectionContext = SelectionContext.None;
                    mouse.UpdateTex(ResourceManager.DefaultCursor);
                    ResourceManager.WallSound.Play();
                }
                else
                {
                    MessageLog.NotEnoughGold();
                }
            }
            else if (mouse.SelectionContext == SelectionContext.PlacingPortalEntrance && mouse.HoveringContext == HoveringContext.EmptyNode)
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
                if (!CheckForPath(portalExit.simplePos.X, portalExit.simplePos.Y, CheckForPathType.AddingPortal))
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
                if (!CheckForPath(n.simplePos.X, n.simplePos.Y, CheckForPathType.TogglingCheese))
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
            if (mouse.SelectionContext == SelectionContext.PlacingPortalExit)
            {
                mouse.PortalEntrance.portal = false;
                mouse.PortalEntrance.defaultSet();
                mouse.PortalEntrance = null;
            }
            else if (mouse.SelectionContext == SelectionContext.TowerSelected)
            {
                Tower t = mouse.SelectedObject as Tower;
                t.Selected = false;
            }
            else if (mouse.HoveringContext == HoveringContext.Tower && mouse.SelectionContext == SelectionContext.None)
            {
                Tower t = mouse.HoveredObject as Tower;
                GameStats.Gold = GameStats.Gold + t.Cost;
                towerlist.Remove(t);
                ResourceManager.SellSound.Play();
            }
            else if (mouse.HoveringContext == HoveringContext.FilledNode && mouse.SelectionContext == SelectionContext.None)
            {
                Node n = mouse.HoveredObject as Node;
                if (n.wall && CheckForPath(n.simplePos.X, n.simplePos.Y, CheckForPathType.TogglingWall))
                {
                    GameStats.Gold = GameStats.Gold + 1;
                    n.wall = false;
                    n.defaultSet();
                    ResourceManager.SellSound.Play();
                }
                else if (n.portal && CheckForPath(n.simplePos.X, n.simplePos.Y, CheckForPathType.RemovingPortal))
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
                else if (n.cheese && CheckForPath(n.simplePos.X, n.simplePos.Y, CheckForPathType.TogglingCheese))
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
        
        enum CheckForPathType
        {
            TogglingWall, TogglingCheese, AddingPortal, RemovingPortal
        }

        bool CheckForPath(int x, int y, CheckForPathType type)
        {
            Node portaledTo = nodes[x, y].portalsTo;

            switch(type)
            {
                case CheckForPathType.AddingPortal:
                    nodes[x, y].portal = true;
                    nodes[x, y].portalsTo = mouse.PortalEntrance;
                    mouse.PortalEntrance.portalsTo = nodes[x, y];
                    break;
                case CheckForPathType.RemovingPortal:
                    nodes[x, y].portal = false;
                    nodes[x, y].portalsTo = null;
                    portaledTo.portalsTo = null;
                    portaledTo.portal = false;
                    break;
                case CheckForPathType.TogglingCheese:
                    nodes[x, y].cheese = !nodes[x, y].cheese;
                    break;
                default:
                    nodes[x, y].wall = !nodes[x, y].wall;
                    break; 
            }

            List<Node> bestPath = PathFinding.findBestPath(nodes);

            switch (type)
            {
                case CheckForPathType.AddingPortal:
                    nodes[x, y].portal = false;
                    nodes[x, y].portalsTo = null;
                    mouse.PortalEntrance.portalsTo = null;
                    break;
                case CheckForPathType.RemovingPortal:
                    nodes[x, y].portal = true;
                    nodes[x, y].portalsTo = portaledTo;
                    portaledTo.portalsTo = nodes[x, y];
                    portaledTo.portal = true;
                    break;
                case CheckForPathType.TogglingCheese:
                    nodes[x, y].cheese = !nodes[x, y].cheese;
                    break;
                default:
                    nodes[x, y].wall = !nodes[x, y].wall;
                    break;
            }

            return bestPath != null;
        }
    }
}
