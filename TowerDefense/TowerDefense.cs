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
        Node[,] nodes = new Node[Constants.X + 1, Constants.Y + 1];
        bool attackPhase = false;
        bool playerLoses = false;
        int level = 0;
        int gold = Constants.STARTINGGOLD;
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

            startButton = new Button(new Vector2(10 + 32, viewport.Height * .2f - 74), ResourceManager.StartButton, false, 0);
            upgradeButton = new Button(new Vector2(viewport.Width - 160, viewport.Height * .55f), ResourceManager.UpgradeButton, false, 0);
            buttonlist.Add(new Button(new Vector2(10, viewport.Height * .2f), ResourceManager.GenericTower, false, 1));
            buttonlist.Add(new Button(new Vector2(10 + 64, viewport.Height * .2f), ResourceManager.CannonTower, false, 2));
            buttonlist.Add(new Button(new Vector2(10, (viewport.Height * .2f) + 64), ResourceManager.BatteryTower, false, 3));
            buttonlist.Add(new Button(new Vector2(10 + 64, (viewport.Height * .2f) + 64), ResourceManager.BlastTower, false, 4));
            buttonlist.Add(new Button(new Vector2(10 + 16, (viewport.Height * .5f)), ResourceManager.Wall, true, 5));
            buttonlist.Add(new Button(new Vector2(10 + 64, (viewport.Height * .5f)), ResourceManager.Portal, true, 6));

            CreateMap();

            mouse = new MouseHandler(Vector2.Zero, ResourceManager.DefaultCursor);
        }

        private void CreateMap()
        {
            int actualY = 64;
            int actualX = 148;
            for (int i = 0; i <= Constants.X; i++)
            {
                for (int j = 0; j <= Constants.Y; j++)
                {
                    nodes[i, j] = new Node(new Vector2(actualX, actualY), new Vector2(i, j), ResourceManager.Grass);
                    actualY += 32;
                }
                actualY = 64;
                actualX += 32;
            }
        }

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
            for (int i = 0; i <= Constants.X; i++)
            {
                for (int j = 0; j <= Constants.Y; j++)
                {
                    nodes[i, j].Draw(batch);
                }
            }

            startButton.Draw(batch);
            enemylist.ForEach(e => e.Draw(batch));
            buttonlist.ForEach(b => b.Draw(batch));
            towerlist.ForEach(t => t.Draw(batch));
            projectilelist.ForEach(p => p.Draw(batch));

            mouse.towerSelected?.ShowStats(batch, ResourceManager.GameFont, viewport);

            if (mouse.towerClicked != null)
            {
                upgradeButton.Draw(batch);
                mouse.towerClicked.ShowStats(batch, ResourceManager.GameFont, viewport);
            }

            mouse.enemyHovered?.ShowStats(batch, ResourceManager.GameFont, viewport);

            batch.DrawString(ResourceManager.GameFont, "GOLD - " + gold + " $", new Vector2(viewport.Width *.8f, viewport.Height *.1f), Color.Black,
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
                if (mouse.mouseState.LeftButton == ButtonState.Pressed)
                {
                    Exit();
                }
                return;
            }

            if (mouse.mouseState.LeftButton == ButtonState.Pressed && startButton.ContainsPoint(mouse.pos) && !attackPhase && !mouse.portalClicked)
            {
                StartLevel();
            }

            if (attackPhase)
            {
                HandleAttackPhase();
            }
            else
            { 
                HandleMouseHover();
                if (mouse.mouseState.LeftButton == ButtonState.Pressed)
                {
                    HandleLeftClick();
                }
                if (mouse.mouseState.RightButton == ButtonState.Pressed && !mouse.rClicking && mouse.nodeHovered != null)
                {
                    HandleRightClick();
                }
                if (mouse.mouseState.LeftButton == ButtonState.Released)
                {
                    mouse.clicking = false;
                }
                if (mouse.mouseState.RightButton == ButtonState.Released)
                {
                    mouse.rClicking = false;
                }
            }
        }

        private void HandleAttackPhase()
        {
            mouse.hovering = false;
            mouse.enemyHovered = null;
            foreach (Enemy e in enemylist)
            {
                if (e.ContainsPoint(mouse.pos))
                {
                    e.hovering = true;
                    mouse.hovering = true;
                    mouse.enemyHovered = e;
                }
                else
                {
                    e.hovering = false;
                    e.color = Color.White;
                }
            }
            if (mouse.enemyHovered != null)
            {
                mouse.enemyHovered.color = Color.Yellow;
            }
        }

        private void HandleMouseHover()
        {
            mouse.hovering = false;
            mouse.buttonHovered = null;
            mouse.towerHovered = null;
            mouse.nodeHovered = null;

            foreach (Button b in buttonlist)
            {
                if (b.ContainsPoint(mouse.pos))
                {
                    b.hovering = true;
                    mouse.hovering = true;
                    mouse.buttonHovered = b;
                }
                else
                {
                    b.hovering = false;
                    b.color = Color.White;
                }

            }
            foreach (Tower t in towerlist)
            {
                if (t.ContainsPoint(mouse.pos) && !mouse.hovering)
                {
                    t.hovering = true;
                    mouse.hovering = true;
                    mouse.towerHovered = t;
                }
                else
                {
                    t.hovering = false;
                    t.color = Color.White;
                }
            }

            for (int i = 0; i <= Constants.X; i++)
            {
                for (int j = 0; j <= Constants.Y; j++)
                {
                    if (nodes[i, j].ContainsPoint(mouse.pos))
                    {
                        nodes[i, j].hovering = true;
                        mouse.hovering = true;
                        mouse.nodeHovered = nodes[i, j];
                    }
                    else
                    {
                        nodes[i, j].hovering = false;
                        nodes[i, j].color = Color.White;
                    }
                }
                if (mouse.enemyHovered != null)
                {
                    mouse.enemyHovered.color = Color.Green;
                }
            }
            if (mouse.buttonHovered != null)
            {
                mouse.buttonHovered.color = Color.Green;
            }
            if (mouse.towerClicked != null)
            {
                mouse.towerClicked.color = Color.Green;
            }
            else if (mouse.highlight && mouse.towerSelected == null && mouse.towerHovered != null)
            {
                mouse.towerHovered.color = Color.Green;
            }
            else if (mouse.highlight && mouse.wallClicked && mouse.nodeHovered != null && !mouse.nodeHovered.portal && !mouse.nodeHovered.wall && CheckForPath((int)mouse.nodeHovered.simplePos.X, (int)mouse.nodeHovered.simplePos.Y, false, false))
            {
                mouse.nodeHovered.color = Color.Green;
            }
            else if (mouse.highlight && !mouse.wallClicked && mouse.nodeHovered != null && !mouse.nodeHovered.portal && !mouse.nodeHovered.wall)
            {
                mouse.nodeHovered.color = Color.Green;
            }
            else if (mouse.highlight && mouse.nodeHovered != null && mouse.nodeHovered.portal && !mouse.nodeHovered.wall)
            {
                mouse.nodeHovered.color = Color.Green;
                if (mouse.nodeHovered.portalsTo != null)
                    mouse.nodeHovered.portalsTo.color = Color.Green;
            }
            else if (mouse.highlight && mouse.nodeHovered != null && !mouse.nodeHovered.portal && mouse.nodeHovered.wall)
            {
                mouse.nodeHovered.color = Color.Red;
            }
            else if (mouse.highlight && mouse.nodeHovered != null && !CheckForPath((int)mouse.nodeHovered.simplePos.X, (int)mouse.nodeHovered.simplePos.Y, false, false))
            {
                mouse.nodeHovered.color = Color.Red;
            }
        }

        private void StartLevel()
        {
            mouse.UpdateTex(ResourceManager.DefaultCursor);
            mouse.towerClicked = null;
            mouse.towerSelected = null;
            attackPhase = true;
            level++;
            MessageLog.Level(level);
            Random rand = new Random();
            double num = rand.NextDouble();
            if (num < .3)
            {
                for (int i = 0; i < (15 + level); i++)
                {
                    enemylist.Add(new Enemy(level * 5, 1, ResourceManager.Enemy, nodes, "Malaria", 1.0f, .25));
                }
            }
            else if (num < .6)
            {
                for (int i = 0; i < (30 + 2 * level); i++)
                {
                    enemylist.Add(new Enemy(level * 3, 2, ResourceManager.Enemy, nodes, "Tuberculosis", .75f, .10));
                }
            }
            else
            {
                for (int i = 0; i < (5 + level / 2); i++)
                {
                    enemylist.Add(new Enemy(level * 20, 1, ResourceManager.Enemy, nodes, "AIDS", 1.25f, .50));
                }
            }
        }

        private void HandleLeftClick()
        {
            if (upgradeButton.ContainsPoint(mouse.pos) && mouse.towerClicked != null && gold >= mouse.towerClicked.cost && !mouse.clicking)
            {
                gold = gold - mouse.towerClicked.cost;
                mouse.towerClicked.upgrade();
            }
            if (mouse.buttonHovered != null && !mouse.clicking)
            {
                mouse.highlight = mouse.buttonHovered.highlight;
                mouse.towerID = mouse.buttonHovered.ID;
                mouse.UpdateTex(mouse.buttonHovered.tex);
                switch (mouse.towerID)
                {
                    case 1:
                        mouse.towerSelected = new GenericTower(mouse.pos, mouse.tex);
                        break;
                    case 2:
                        mouse.towerSelected = new CannonTower(mouse.pos, mouse.tex);
                        break;
                    case 3:
                        mouse.towerSelected = new BatteryTower(mouse.pos, mouse.tex);
                        break;
                    case 4:
                        mouse.towerSelected = new BlastTower(mouse.pos, mouse.tex);
                        break;
                    case 5:
                        mouse.wallClicked = true;
                        break;
                    case 6:
                        mouse.portalClicked = true;
                        break;
                    default:
                        break;
                }
                mouse.towerClicked = null;
            }
            else if (mouse.nodeHovered != null && !mouse.highlight && mouse.towerSelected != null && !mouse.clicking && mouse.pos.X <= 641 && mouse.pos.Y <= 679)
            {
                if (gold >= mouse.towerSelected.cost)
                {
                    gold = gold - mouse.towerSelected.cost;
                    towerlist.Add(mouse.towerSelected);
                    mouse.towerSelected.position = mouse.pos;
                    mouse.towerSelected = null;
                    mouse.UpdateTex(ResourceManager.DefaultCursor);
                    mouse.highlight = true;
                    mouse.towerClicked = null;
                    ResourceManager.WallSound.Play();
                }
                else
                {
                    MessageLog.NotEnoughGold();
                }
            }
            else if (mouse.nodeHovered != null && !mouse.nodeHovered.wall && !mouse.nodeHovered.portal && mouse.highlight && mouse.wallClicked && CheckForPath((int)mouse.nodeHovered.simplePos.X, (int)mouse.nodeHovered.simplePos.Y, false, false))
            {
                if (gold >= 1)
                {
                    mouse.nodeHovered.wall = true;
                    mouse.nodeHovered.UpdateTex(mouse.tex);
                    gold = gold - 1;
                    ResourceManager.WallSound.Play();
                }
                else
                {
                    MessageLog.NotEnoughGold();
                }

            }
            else if (mouse.nodeHovered != null && mouse.portalComplete && !mouse.nodeHovered.wall && !mouse.nodeHovered.portal && mouse.highlight && mouse.portalClicked)
            {
                mouse.nodeHovered.portal = true;
                mouse.nodeHovered.UpdateTex(mouse.tex);
                mouse.portalLocation = mouse.nodeHovered;
                mouse.portalComplete = false;
            }
            else if (mouse.nodeHovered != null && !mouse.portalComplete && !mouse.nodeHovered.wall && !mouse.nodeHovered.portal && mouse.highlight && mouse.portalClicked && CheckForPath((int)mouse.nodeHovered.simplePos.X, (int)mouse.nodeHovered.simplePos.Y, true, false))
            {
                if (gold >= 20)
                {
                    mouse.nodeHovered.portal = true;
                    mouse.nodeHovered.UpdateTex(mouse.tex);
                    mouse.nodeHovered.portalsTo = mouse.portalLocation;
                    mouse.portalLocation.portalsTo = mouse.nodeHovered;
                    mouse.portalComplete = true;
                    gold = gold - 20;
                }
                else
                {
                    MessageLog.NotEnoughGold();
                }
            }
            else if (mouse.towerHovered != null && mouse.highlight && mouse.towerSelected == null && !mouse.clicking)
            {
                mouse.towerClicked = mouse.towerHovered;
            }
            mouse.clicking = true;
        }

        private void HandleRightClick()
        {
            if (mouse.towerHovered != null)
            {
                gold = gold + mouse.towerHovered.cost;
                towerlist.Remove(mouse.towerHovered);
                ResourceManager.SellSound.Play();
            }
            else if (mouse.nodeHovered.wall && CheckForPath((int)mouse.nodeHovered.simplePos.X, (int)mouse.nodeHovered.simplePos.Y, false, true))
            {
                gold = gold + 1;
                mouse.nodeHovered.wall = false;
                mouse.nodeHovered.defaultSet();
                ResourceManager.SellSound.Play();
            }
            else if (mouse.nodeHovered.portal && CheckForPath((int)mouse.nodeHovered.simplePos.X, (int)mouse.nodeHovered.simplePos.Y, true, true))
            {
                mouse.nodeHovered.portal = false;
                mouse.nodeHovered.defaultSet();
                if (mouse.nodeHovered.portalsTo != null)
                {
                    mouse.nodeHovered.portalsTo.portal = false;
                    mouse.nodeHovered.portalsTo.portalsTo = null;
                    mouse.nodeHovered.portalsTo.defaultSet();
                    mouse.nodeHovered.portalsTo = null;
                    gold = gold + 20;
                }
                else
                {
                    mouse.portalComplete = true;
                }
            }
            if (!mouse.portalComplete)
            {
                mouse.portalLocation.portal = false;
                mouse.portalLocation.defaultSet();
                mouse.portalLocation = null;
                mouse.portalComplete = true;
            }
            mouse.UpdateTex(ResourceManager.DefaultCursor);
            mouse.wallClicked = false;
            mouse.portalClicked = false;
            mouse.towerClicked = null;
            mouse.towerSelected = null;
            mouse.highlight = true;
            mouse.rClicking = true;
        }

        private static int heuristic(Node current)
        {
            return Constants.Y - (int)current.simplePos.Y;
        }

        internal static List<Node> findBestPath(Node[,] nodes)
        {
            List<Node> available = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            for (int i = 0; i <= Constants.X; i++)
                for (int j = 0; j <= Constants.Y; j++)
                {
                    nodes[i, j].parent = null;
                    nodes[i, j].fScore = int.MaxValue;
                }
            for (int i = 0; i <= Constants.X; i++)
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
                if (current.simplePos.Y == Constants.Y)
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

        public bool CheckForPath( int x, int y, bool portal, bool remove)
        {
            Node portaledTo = nodes[x, y].portalsTo;
            if (portal)
            {
                nodes[x, y].portal = !remove;
                nodes[x, y].portalsTo = remove ? null : mouse.portalLocation;
                if (remove)
                {
                    portaledTo.portalsTo = null;
                    portaledTo.portal = false;
                }
                else
                {
                    mouse.portalLocation.portalsTo = nodes[x, y];
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
                    mouse.portalLocation.portalsTo = null;
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
