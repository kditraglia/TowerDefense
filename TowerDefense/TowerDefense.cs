using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    public class TowerDefense : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch batch;
        SpriteFont text;
        MouseHandler ourMouse;
        Texture2D defaultMouse;
        Texture2D enemy;
        Texture2D banner;
        Texture2D banner2;
        Texture2D[] proj;
        SoundEffect[] sound;
        Viewport viewport;
        Button startButton;
        Button upgradeButton;
        List < Button >buttonlist; //list of buttons
        List< Tower > towerlist; //list of towers
        List< Enemy > enemylist; //list of active enemies
        List < Projectile > projectilelist; //list of active enemies
        MouseState mouseState;
        Constants CONSTANT = new Constants();
        MessageLog MessageLog = new MessageLog();
        Node[,] nodes = new Node[17,21];
        bool attackPhase = false;
        bool playerLoses = false;
        int level = 0;
        int counter = 0;
        int counter2 = 0;
        int enemyID = 0;
        int towerID = 100;
        int gold;

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
            text = Content.Load<SpriteFont>("text");
            startButton = new Button(new Vector2(10 + 32, viewport.Height * .2f - 74), Content.Load<Texture2D>(@"start"), false, 0);
            upgradeButton = new Button(new Vector2(viewport.Width - 160, viewport.Height * .55f), Content.Load<Texture2D>(@"upgrade"), false, 0);
            enemy = Content.Load<Texture2D>(@"enemy");
            banner = Content.Load<Texture2D>(@"banner");
            banner2 = Content.Load<Texture2D>(@"banner2");
            this.buttonlist = new List<Button>();
            this.buttonlist.Add(new Button(new Vector2(10, viewport.Height * .2f), Content.Load<Texture2D>(@"generic tower"), false, 1));
            this.buttonlist.Add(new Button(new Vector2(10 + 64, viewport.Height * .2f), Content.Load<Texture2D>(@"cannon tower"), false, 2));
            this.buttonlist.Add(new Button(new Vector2(10, (viewport.Height * .2f) + 64), Content.Load<Texture2D>(@"battery tower"), false, 3));
            this.buttonlist.Add(new Button(new Vector2(10 + 64, (viewport.Height * .2f) + 64), Content.Load<Texture2D>(@"blast tower"), false, 4));
            this.buttonlist.Add(new Button(new Vector2(10 + 16, (viewport.Height * .5f)), Content.Load<Texture2D>(@"wall"), true, 5));
            this.buttonlist.Add(new Button(new Vector2(10 + 64, (viewport.Height * .5f)), Content.Load<Texture2D>(@"portal"), true, 6));

            this.towerlist = new List<Tower>();
            this.projectilelist = new List<Projectile>();
            this.enemylist = new List<Enemy>();

            this.proj = new Texture2D[4];
            proj[0] = Content.Load<Texture2D>(@"bullet");
            proj[1] = Content.Load<Texture2D>(@"cannon ball");
            proj[2] = Content.Load<Texture2D>(@"lightning bolt");
            proj[3] = Content.Load<Texture2D>(@"blast");

            this.sound = new SoundEffect[20];
            sound[0] = Content.Load<SoundEffect>(@"generic attack");
            sound[1] = Content.Load<SoundEffect>(@"cannon attack");
            sound[2] = Content.Load<SoundEffect>(@"battery attack");
            sound[3] = Content.Load<SoundEffect>(@"blast attack");
            sound[4] = Content.Load<SoundEffect>(@"damaged");
            sound[5] = Content.Load<SoundEffect>(@"sell");
            sound[6] = Content.Load<SoundEffect>(@"wallsound");
            sound[7] = Content.Load<SoundEffect>(@"portalsound");

            gold = CONSTANT.STARTINGGOLD;

            int actualY = 64;
            int actualX = 148;
            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    nodes[i, j] = new Node(new Vector2(actualX, actualY), new Vector2(i, j), Content.Load<Texture2D>(@"grass"));

                    actualY = actualY + 32;
                    if (actualY > 704)
                    {
                        actualY = 64;
                        actualX = actualX + 32;
                    }
                }
            }


            //Handle the cursor creation
            defaultMouse = Content.Load<Texture2D>(@"cursor");
            ourMouse = new MouseHandler(new Vector2(0, 0), defaultMouse);
        }


        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            HandleMouse(); //Check clicking
            ourMouse.Update(); //Update the mouse's position.
            if (attackPhase)
            {
                foreach (Enemy e in enemylist)
                {
                    if (!e.spawned && counter >= e.spawnRate)
                    {
                        e.spawn();
                        counter = 0;
                    }
                }
                List<Enemy> temp = new List<Enemy>();
                foreach (Enemy e in enemylist)
                {
                    if (e.dead)
                        temp.Add(e);
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
                    projectilelist = t.Attack(enemylist, projectilelist, counter2);
                }
                List<Projectile> temp2 = new List<Projectile>();
                foreach (Projectile p in projectilelist)
                {
                    if ( p.Move() )
                        temp2.Add(p);
                    
                }
                foreach (Projectile p in temp2)
                {
                    projectilelist.Remove(p);
                }
            }
            if (enemylist.Count == 0 && attackPhase)
            {
                attackPhase = false;
                projectilelist.Clear();
                MessageLog.LevelComplete(level * 2 + (int)(gold * .05f), level);
                gold = gold + (level * 2 + (int)(gold * .05f));
            }
            counter++;
            counter2++;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            batch.Begin();
            for (int i = 0; i < 17; i++)
                for( int j = 0; j < 21; j++ )
                    nodes[i,j].Draw(batch);

            startButton.Draw(batch);
            
            foreach (Enemy e in enemylist) //For every enemy run through the code between the {}
            {
                //For every enemy in our list, draw it
                e.Draw(batch);

            }
            foreach (Button b in buttonlist) //For every button run through the code between the {}
            {
                //For every button in our list, draw it
                b.Draw(batch);
            }
            foreach (Tower t in towerlist) //For every tower run through the code between the {}
            {
                //For every tower in our list, draw it
                t.Draw(batch);
            }
            foreach (Projectile p in projectilelist) //For every projectile run through the code between the {}
            {
                //For every projectile in our list, draw it
                p.Draw(batch);
            }
            if (ourMouse.towerSelected != null)
                ourMouse.towerSelected.ShowStats(batch, text, viewport);
            else if (ourMouse.towerClicked != null)
            {
                upgradeButton.Draw(batch);
                ourMouse.towerClicked.ShowStats(batch, text, viewport);
            }

            if (ourMouse.enemyHovered != null)
                ourMouse.enemyHovered.ShowStats(batch, text, viewport);

            batch.DrawString(text, "GOLD - " + gold + " $", new Vector2(viewport.Width *.8f, viewport.Height *.1f), Color.Black,
                    0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            MessageLog.Draw(batch, text, viewport);

            batch.Draw(banner, new Vector2(viewport.Width / 2 - banner.Width / 2, 0), null, Color.White);
            batch.Draw(banner2, new Vector2(viewport.Width / 2 - banner.Width / 4, viewport.Height - banner2.Height), null, Color.White);
            //Draw the mouse.
            ourMouse.Draw(batch);

            batch.End();

            base.Draw(gameTime);

        }



        private void HandleMouse()
        {
            mouseState = Mouse.GetState(); //Get the current state of the mouse
            ourMouse.enemyHovered = null;
            if (!playerLoses)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && ourMouse.ButtonClick(startButton) && !attackPhase)
                {
                    ourMouse.UpdateTex(defaultMouse);
                    ourMouse.towerClicked = null;
                    ourMouse.towerSelected = null;
                    attackPhase = true;
                    level++;
                    MessageLog.Level(level);
                    Random rand = new Random();
                    double num = rand.NextDouble();
                    if (num < .3)
                    {
                        for (int i = 0; i < (15 + level); i++)
                        {
                            enemylist.Add(new Enemy(level * 5, 1, enemy, nodes, "Malaria", 1.0f, 25, sound[4], sound[7], enemyID));
                            enemyID++;
                        }
                    }
                    else if (num < .6)
                    {
                        for (int i = 0; i < (30 + 2 * level); i++)
                        {
                            enemylist.Add(new Enemy(level * 3, 2, enemy, nodes, "tuberculosis", .75f, 10, sound[4], sound[7], enemyID));
                            enemyID++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < (5 + level / 2); i++)
                        {
                            enemylist.Add(new Enemy(level * 20, 1, enemy, nodes, "AIDS", 1.25f, 50, sound[4], sound[7], enemyID));
                            enemyID++;
                        }
                    }

                }

                if (attackPhase)
                {
                    ourMouse.hovering = false;
                    ourMouse.enemyHovered = null;
                    foreach (Enemy e in enemylist)
                    {
                        if (ourMouse.EnemyClick(e))
                        {
                            e.hovering = true;
                            ourMouse.hovering = true;
                            ourMouse.enemyHovered = e;
                        }
                        else
                        {
                            e.hovering = false;
                            e.color = Color.White;
                        }
                    }
                    if (ourMouse.enemyHovered != null)
                        ourMouse.enemyHovered.color = Color.Yellow;

                }



                ourMouse.hovering = false;
                ourMouse.buttonHovered = null;
                ourMouse.towerHovered = null;
                ourMouse.nodeHovered = null;

                foreach (Button b in buttonlist)
                {
                    if (ourMouse.ButtonClick(b))
                    {
                        b.hovering = true;
                        ourMouse.hovering = true;
                        ourMouse.buttonHovered = b;
                    }
                    else
                    {
                        b.hovering = false;
                        b.color = Color.White;
                    }

                }
                foreach (Tower t in towerlist)
                {
                    if (ourMouse.TowerClick(t) && !ourMouse.hovering)
                    {
                        t.hovering = true;
                        ourMouse.hovering = true;
                        ourMouse.towerHovered = t;
                    }
                    else
                    {
                        t.hovering = false;
                        t.color = Color.White;
                    }
                }

                for (int i = 0; i < 17; i++)
                {
                    for (int j = 0; j < 21; j++)
                    {
                        if (ourMouse.NodeClick(nodes[i, j]))
                        {
                            nodes[i, j].hovering = true;
                            ourMouse.hovering = true;
                            ourMouse.nodeHovered = nodes[i, j];
                        }
                        else
                        {
                            nodes[i, j].hovering = false;
                            nodes[i, j].color = Color.White;
                        }
                    }
                    if (ourMouse.enemyHovered != null)
                        ourMouse.enemyHovered.color = Color.Green;
                }
                if (!attackPhase)
                {
                    if (ourMouse.buttonHovered != null)
                    {
                        ourMouse.buttonHovered.color = Color.Green;
                    }
                    if (ourMouse.towerClicked != null)
                    {
                        ourMouse.towerClicked.color = Color.Green;
                    }
                    else if (ourMouse.highlight && ourMouse.towerSelected == null && ourMouse.towerHovered != null)
                    {
                        ourMouse.towerHovered.color = Color.Green;
                    }
                    else if (ourMouse.highlight && ourMouse.wallClicked && ourMouse.nodeHovered != null && !ourMouse.nodeHovered.portal && !ourMouse.nodeHovered.wall && CheckForPath((int)ourMouse.nodeHovered.simplePos.X, (int)ourMouse.nodeHovered.simplePos.Y ))
                    {
                        ourMouse.nodeHovered.color = Color.Green;
                    }
                    else if (ourMouse.highlight && !ourMouse.wallClicked && ourMouse.nodeHovered != null && !ourMouse.nodeHovered.portal && !ourMouse.nodeHovered.wall)
                    {
                        ourMouse.nodeHovered.color = Color.Green;
                    }
                    else if (ourMouse.highlight && ourMouse.nodeHovered != null && ourMouse.nodeHovered.portal && !ourMouse.nodeHovered.wall)
                    {
                        ourMouse.nodeHovered.color = Color.Green;
                        if (ourMouse.nodeHovered.portalsTo != null)
                            ourMouse.nodeHovered.portalsTo.color = Color.Green;
                    }
                    else if (ourMouse.highlight && ourMouse.nodeHovered != null && !ourMouse.nodeHovered.portal && ourMouse.nodeHovered.wall)
                    {
                        ourMouse.nodeHovered.color = Color.Red;
                    }
                    else if (ourMouse.highlight && ourMouse.nodeHovered != null && !CheckForPath((int)ourMouse.nodeHovered.simplePos.X, (int)ourMouse.nodeHovered.simplePos.Y ))
                    {
                        ourMouse.nodeHovered.color = Color.Red;
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if ( ourMouse.ButtonClick(upgradeButton) && ourMouse.towerClicked != null && gold >= ourMouse.towerClicked.cost && !ourMouse.clicking )
                        {
                            gold = gold - ourMouse.towerClicked.cost;
                            ourMouse.towerClicked.upgrade();
                            ourMouse.clicking = true;
                        }
                        if (ourMouse.buttonHovered != null)
                        {
                            ourMouse.highlight = ourMouse.buttonHovered.highlight;
                            ourMouse.towerID = ourMouse.buttonHovered.ID;
                            ourMouse.UpdateTex(ourMouse.buttonHovered.tex);
                            switch (ourMouse.towerID)
                            {
                                case 1:
                                    ourMouse.towerSelected = new GenericTower(ourMouse.pos, ourMouse.tex, proj[0], towerID, sound[0]);
                                    break;
                                case 2:
                                    ourMouse.towerSelected = new CannonTower(ourMouse.pos, ourMouse.tex, proj[1], towerID, sound[1]);
                                    break;
                                case 3:
                                    ourMouse.towerSelected = new BatteryTower(ourMouse.pos, ourMouse.tex, proj[2], towerID, sound[2]);
                                    break;
                                case 4:
                                    ourMouse.towerSelected = new BlastTower(ourMouse.pos, ourMouse.tex, proj[3], towerID, sound[3]);
                                    break;
                                case 5:
                                    ourMouse.wallClicked = true;
                                    break;
                                case 6:
                                    ourMouse.portalClicked = true;
                                    break;
                                default:
                                    break;
                            }
                            towerID++;
                            ourMouse.towerClicked = null;
                        }
                        else if (ourMouse.nodeHovered != null && !ourMouse.highlight && ourMouse.towerSelected != null && !ourMouse.clicking && ourMouse.pos.X <= 641 && ourMouse.pos.Y <= 679 )
                        {
                            if (gold >= ourMouse.towerSelected.cost)
                            {
                                gold = gold - ourMouse.towerSelected.cost;
                                towerlist.Add(ourMouse.towerSelected);
                                ourMouse.towerSelected.position = ourMouse.pos;
                                ourMouse.towerSelected = null;
                                ourMouse.UpdateTex(defaultMouse);
                                ourMouse.highlight = true;
                                ourMouse.towerClicked = null;
                                sound[6].Play();
                            }
                            else MessageLog.NotEnoughGold();
                        }
                        else if (ourMouse.nodeHovered != null && !ourMouse.nodeHovered.wall && !ourMouse.nodeHovered.portal && ourMouse.highlight && ourMouse.wallClicked && CheckForPath((int)ourMouse.nodeHovered.simplePos.X, (int)ourMouse.nodeHovered.simplePos.Y ))
                        {
                            if (gold >= 1)
                            {
                                ourMouse.nodeHovered.wall = true;
                                ourMouse.nodeHovered.UpdateTex(ourMouse.tex);
                                gold = gold - 1;
                                sound[6].Play();
                            }
                            else MessageLog.NotEnoughGold();

                        }
                        else if (ourMouse.nodeHovered != null && ourMouse.portalComplete && !ourMouse.nodeHovered.wall && !ourMouse.nodeHovered.portal && ourMouse.highlight && ourMouse.portalClicked )
                        {
                            ourMouse.nodeHovered.portal = true;
                            ourMouse.nodeHovered.UpdateTex(ourMouse.tex);
                            ourMouse.portalLocation = ourMouse.nodeHovered;
                            ourMouse.portalComplete = false;
                        }
                        else if (ourMouse.nodeHovered != null && !ourMouse.portalComplete && !ourMouse.nodeHovered.wall && !ourMouse.nodeHovered.portal && ourMouse.highlight && ourMouse.portalClicked )
                        {
                            if (gold >= 20)
                            {
                                ourMouse.nodeHovered.portal = true;
                                ourMouse.nodeHovered.UpdateTex(ourMouse.tex);
                                ourMouse.nodeHovered.portalsTo = ourMouse.portalLocation;
                                ourMouse.portalLocation.portalsTo = ourMouse.nodeHovered;
                                ourMouse.portalComplete = true;
                                gold = gold - 20;
                            }
                            else MessageLog.NotEnoughGold();
                        }
                        else if (ourMouse.towerHovered != null && ourMouse.highlight && ourMouse.towerSelected == null && !ourMouse.clicking)
                        {
                            ourMouse.towerClicked = ourMouse.towerHovered;
                        }
                        ourMouse.clicking = true;
                    }
                    if (mouseState.RightButton == ButtonState.Pressed && !ourMouse.rClicking && ourMouse.nodeHovered != null)
                    {
                        if (ourMouse.towerHovered != null  )
                        {
                            gold = gold + ourMouse.towerHovered.cost;
                            towerlist.Remove(ourMouse.towerHovered);
                            sound[5].Play();
                        }
                        else if (ourMouse.nodeHovered.wall )
                        {
                            gold = gold + 1;
                            ourMouse.nodeHovered.wall = false;
                            ourMouse.nodeHovered.defaultSet();
                            sound[6].Play();
                        }
                        else if (ourMouse.nodeHovered.portal)
                        {
                            ourMouse.nodeHovered.portal = false;
                            ourMouse.nodeHovered.defaultSet();
                            if (ourMouse.nodeHovered.portalsTo != null)
                            {
                                ourMouse.nodeHovered.portalsTo.portal = false;
                                ourMouse.nodeHovered.portalsTo.portalsTo = null;
                                ourMouse.nodeHovered.portalsTo.defaultSet();
                                ourMouse.nodeHovered.portalsTo = null;
                                gold = gold + 20;
                            }
                            else
                                ourMouse.portalComplete = true;

                        }
                        ourMouse.UpdateTex(defaultMouse);
                        ourMouse.wallClicked = false;
                        ourMouse.portalClicked = false;
                        ourMouse.towerClicked = null;
                        ourMouse.towerSelected = null;
                        ourMouse.highlight = true;
                        ourMouse.rClicking = true;
                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        ourMouse.clicking = false;
                    }
                    if (mouseState.RightButton == ButtonState.Released)
                    {
                        ourMouse.rClicking = false;
                    }
                }
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    this.Exit();

            }
        }


        public bool CheckForPath( int x, int y )
        {
            nodes[x, y].wall = true;

            bool status = false;
            List<Node> available = new List<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            List<Node> temp = new List<Node>();
            for (int i = 0; i < 17; i++)
            {
                if (!nodes[i, 0].wall)
                {
                    available.Add(nodes[i, 0]);
                    visited.Add(nodes[i, 0]);
                }
            }
            while (available.Count != 0)
            {
                foreach (Node n in available)
                {
                    if ( n.simplePos.Y == 20 )
                    {
                        status = true;
                        break;
                    }
                    if (n.portal)
                    {
                        temp.Add(n.portalsTo);
                    }
                    if (((int)n.simplePos.Y + 1) < 21 && !visited.Contains(nodes[(int)n.simplePos.X, (int)n.simplePos.Y + 1]) && !nodes[(int)n.simplePos.X, (int)n.simplePos.Y + 1].wall && !temp.Contains(nodes[(int)n.simplePos.X, (int)n.simplePos.Y + 1]))
                        temp.Add(nodes[(int)n.simplePos.X, (int)n.simplePos.Y + 1]);
                    if (((int)n.simplePos.Y - 1) >= 0 && !visited.Contains(nodes[(int)n.simplePos.X, (int)n.simplePos.Y - 1]) && !nodes[(int)n.simplePos.X, (int)n.simplePos.Y - 1].wall && !temp.Contains(nodes[(int)n.simplePos.X, (int)n.simplePos.Y - 1]))
                        temp.Add(nodes[(int)n.simplePos.X, (int)n.simplePos.Y - 1]);
                    if (((int)n.simplePos.X + 1) < 17 && !visited.Contains(nodes[(int)n.simplePos.X + 1, (int)n.simplePos.Y]) && !nodes[(int)n.simplePos.X + 1, (int)n.simplePos.Y].wall && !temp.Contains(nodes[(int)n.simplePos.X + 1, (int)n.simplePos.Y]))
                        temp.Add(nodes[(int)n.simplePos.X + 1, (int)n.simplePos.Y]);
                    if (((int)n.simplePos.X - 1) >= 0 && !visited.Contains(nodes[(int)n.simplePos.X - 1, (int)n.simplePos.Y]) && !nodes[(int)n.simplePos.X - 1, (int)n.simplePos.Y].wall && !temp.Contains(nodes[(int)n.simplePos.X - 1, (int)n.simplePos.Y]))
                        temp.Add(nodes[(int)n.simplePos.X - 1, (int)n.simplePos.Y]);
                    visited.Add(n);
                }
                if ( status ) break;

                available.AddRange(temp);
                available.RemoveAll(a => visited.Contains(a));
                temp.Clear();
            }
            nodes[x, y].wall = false;
            return status;
        }
    }
}
