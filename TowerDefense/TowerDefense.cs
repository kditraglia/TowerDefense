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
        MessageLog MessageLog = new MessageLog();
        Node[,] nodes = new Node[Constants.MapSize.X + 1, Constants.MapSize.Y + 1];
        bool attackPhase = false;
        bool playerLoses = false;
        int level = 0;
        int gold = Constants.StartingGold;
        double lastSpawnedTime = 0;

        public TowerDefense()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 950;
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
                    projectilelist = t.Attack(enemylist, projectilelist, gameTime.TotalGameTime.TotalSeconds);
                }
                projectilelist.RemoveAll(p => p.Move());
            }
            if (enemylist.Count == 0 && attackPhase)
            {
                attackPhase = false;
                projectilelist.Clear();
                MessageLog.LevelComplete(level * 2 + (int)(gold * .05f), level);
                gold = gold + (level * 2 + (int)(gold * .05f));
            }
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

            if (mouse.SelectionContext == SelectionContext.TowerSelected)
            {
                Tower t = mouse.SelectedObject as Tower;
                t?.ShowStats(batch, ResourceManager.GameFont, viewport);
            }
            else if (mouse.HoveringContext == HoveringContext.Tower)
            {
                Tower t = mouse.HoveredObject as Tower;
                t?.ShowStats(batch, ResourceManager.GameFont, viewport);
            }
            else if (mouse.HoveringContext == HoveringContext.Enemy)
            {
                Enemy e = mouse.HoveredObject as Enemy;
                e?.ShowStats(batch, ResourceManager.GameFont, viewport);
            }

            batch.DrawString(ResourceManager.GameFont, "GOLD - " + gold + " $", new Vector2(viewport.Width * .8f, viewport.Height * .1f), Color.Black,
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
                    n.Hovering = n.BoundingBox().Contains(mouse.pos);
                    if (n.Hovering)
                    {
                        mouse.HoveredObject = n;
                        mouse.HoveringContext = n.wall || n.portal ? HoveringContext.FilledNode : HoveringContext.EmptyNode;
                    }
                }

                buttonlist.ForEach(b =>
                {
                    b.Hovering = b.BoundingBox().Contains(mouse.pos) && mouse.SelectionContext == SelectionContext.None;
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
                    case HoveringContext.ButtonUpgrade:
                        {
                            Tower t = mouse.SelectedObject as Tower;
                            gold = gold - t.cost;
                            t.upgrade();
                            break;
                        }
                    case HoveringContext.Tower:
                        {
                            Tower t = mouse.HoveredObject as Tower;
                            mouse.SelectedObject = t;
                            mouse.SelectionContext = SelectionContext.TowerSelected;
                            t.Selected = true;
                            break;
                        }
                    default:
                        break;
                }
            }
            if (mouse.SelectionContext == SelectionContext.PlacingTower && MouseInGameBounds())
            {
                Tower t = mouse.SelectedObject as Tower;
                if (gold >= t.cost)
                {
                    gold = gold - t.cost;
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
            else if (mouse.SelectionContext == SelectionContext.PlacingWall && mouse.HoveringContext == HoveringContext.EmptyNode)
            {
                Node n = mouse.HoveredObject as Node;
                if (!CheckForPath(n.simplePos.X, n.simplePos.Y, false, false))
                {
                    MessageLog.IllegalPosition();
                }
                else if (gold >= 1)
                {
                    n.wall = true;
                    n.UpdateTex(mouse.tex);
                    gold = gold - 1;
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
                if (!CheckForPath(portalExit.simplePos.X, portalExit.simplePos.Y, true, false))
                {
                    MessageLog.IllegalPosition();
                }
                else if (gold >= 20)
                {
                    portalExit.portal = true;
                    portalExit.UpdateTex(mouse.tex);
                    portalExit.portalsTo = portalEntrance;
                    portalEntrance.portalsTo = portalExit;
                    mouse.SelectionContext = SelectionContext.PlacingPortalEntrance;
                    gold = gold - 20;
                }
                else
                {
                    MessageLog.NotEnoughGold();
                }
            }
        }

        private void HandleRightClick()
        {
            if (mouse.HoveringContext == HoveringContext.Tower)
            {
                Tower t = mouse.HoveredObject as Tower;
                gold = gold + t.cost;
                towerlist.Remove(t);
                ResourceManager.SellSound.Play();
            }
            else if (mouse.HoveringContext == HoveringContext.FilledNode)
            {
                Node n = mouse.HoveredObject as Node;
                if (n.wall && CheckForPath(n.simplePos.X, n.simplePos.Y, false, true))
                {
                    gold = gold + 1;
                    n.wall = false;
                    n.defaultSet();
                    ResourceManager.SellSound.Play();
                }
                else if (n.portal && CheckForPath(n.simplePos.X, n.simplePos.Y, true, true))
                {
                    n.portal = false;
                    n.defaultSet();
                    n.portalsTo.portal = false;
                    n.portalsTo.portalsTo = null;
                    n.portalsTo.defaultSet();
                    n.portalsTo = null;
                    gold = gold + 20;
                    ResourceManager.SellSound.Play();
                }
            }
            if (mouse.SelectionContext == SelectionContext.PlacingPortalExit)
            {
                mouse.PortalEntrance.portal = false;
                mouse.PortalEntrance.defaultSet();
                mouse.PortalEntrance = null;
            }
            if (mouse.SelectionContext == SelectionContext.TowerSelected)
            {
                Tower t = mouse.SelectedObject as Tower;
                t.Selected = false;
            }
            mouse.UpdateTex(ResourceManager.DefaultCursor);
            mouse.SelectionContext = SelectionContext.None;
        }

        private static int heuristic(Node current)
        {
            return Constants.MapSize.Y - current.simplePos.Y;
        }

        internal static List<Node> findBestPath(Node[,] nodes)
        {
            List<Node> available = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            for (int i = 0; i <= Constants.MapSize.X; i++)
                for (int j = 0; j <= Constants.MapSize.Y; j++)
                {
                    nodes[i, j].parent = null;
                    nodes[i, j].fScore = int.MaxValue;
                }
            for (int i = 0; i <= Constants.MapSize.X; i++)
            {
                if (!nodes[i, 0].wall)
                {
                    available.Add(nodes[i, 0]);
                    nodes[i, 0].fScore = 0;
                }
            }
            while (available.Count != 0)
            {
                Node current = available.OrderBy(n => n.fScore).First();
                if (current.simplePos.Y == Constants.MapSize.Y)
                {
                    List<Node> bestPath = new List<Node>();
                    while (current.parent != null)
                    {
                        bestPath.Add(current.parent);
                        current = current.parent;
                    }
                    return bestPath;
                }
                available.Remove(current);
                visited.Add(current);
                foreach (Node n in current.getNeighbors(nodes))
                {
                    if (visited.Contains(n))
                    {
                        continue;
                    }
                    int possibleScore = current.gScore + 1;
                    if (!available.Contains(n))
                    {
                        available.Add(n);
                    }
                    else if (possibleScore >= n.gScore)
                    {
                        continue;
                    }
                    n.parent = current;
                    n.gScore = possibleScore;
                    n.fScore = possibleScore + heuristic(n);
                }
            }
            return null;
        }

        public bool CheckForPath(int x, int y, bool portal, bool remove)
        {
            Node portaledTo = nodes[x, y].portalsTo;
            if (portal)
            {
                nodes[x, y].portal = !remove;
                nodes[x, y].portalsTo = remove ? null : mouse.PortalEntrance;
                if (remove)
                {
                    portaledTo.portalsTo = null;
                    portaledTo.portal = false;
                }
                else
                {
                    mouse.PortalEntrance.portalsTo = nodes[x, y];
                }

            }
            else
            {
                nodes[x, y].wall = !remove;
            }
            List<Node> bestPath = findBestPath(nodes);
            if (portal)
            {
                nodes[x, y].portal = remove;
                nodes[x, y].portalsTo = remove ? portaledTo : null;
                if (remove)
                {
                    portaledTo.portalsTo = nodes[x, y];
                    portaledTo.portal = true;
                }
                else
                {
                    mouse.PortalEntrance.portalsTo = null;
                }
            }
            else
            {
                nodes[x, y].wall = remove;
            }
            return bestPath != null;
        }
    }
}
