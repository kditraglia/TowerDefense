using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    class GameEngine
    {
        List<Tower> towerlist = new List<Tower>();
        List<Enemy> enemylist = new List<Enemy>();
        List<Projectile> projectilelist = new List<Projectile>();
        List<FloatingText> floatingTextList = new List<FloatingText>();

        double lastSpawnedTime = 0;

        internal GameEngine()
        {

        }

        internal void Update(GameTime gameTime)
        {
            if (GameStats.AttackPhase)
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
                        GameStats.PlayerLoses = true;
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
            if (enemylist.Count == 0 && GameStats.AttackPhase)
            {
                GameStats.AttackPhase = false;
                projectilelist.Clear();
                MessageLog.LevelComplete(GameStats.Level * 2 + (int)(GameStats.Gold * .05f), GameStats.Level);
                GameStats.Gold = GameStats.Gold + (GameStats.Level * 2 + (int)(GameStats.Gold * .05f));
            }
            floatingTextList.RemoveAll(f => f.Update(gameTime));
        }

        internal void Draw(SpriteBatch batch)
        {
            enemylist.ForEach(e => e.Draw(batch));
            towerlist.ForEach(t => t.Draw(batch));
            projectilelist.ForEach(p => p.Draw(batch));
            floatingTextList.ForEach(f => f.Draw(batch));
        }

        internal void StartLevel()
        {
            GameStats.AttackPhase = true;
            GameStats.Level++;
            MessageLog.Level(GameStats.Level);
            Random rand = new Random();
            double num = rand.NextDouble();
            if (num < .3)
            {
                for (int i = 0; i < (15 + GameStats.Level); i++)
                {
                    enemylist.Add(new Enemy(GameStats.Level * 8, 2, ResourceManager.Enemy, nodes, "Malaria", 1.0f, .4f));
                }
            }
            else if (num < .6)
            {
                for (int i = 0; i < (30 + 2 * GameStats.Level); i++)
                {
                    enemylist.Add(new Enemy(GameStats.Level * 4, 2, ResourceManager.Enemy, nodes, "Tuberculosis", .33f, .2f));
                }
            }
            else
            {
                for (int i = 0; i < (5 + GameStats.Level / 2); i++)
                {
                    enemylist.Add(new Enemy(GameStats.Level * 16, 1, ResourceManager.Enemy, nodes, "AIDS", 1.5f, .8f));
                }
            }
        }

        internal void HandleMouseHover(MouseHandler mouse)
        {
            enemylist.ForEach(e =>
            {
                e.Hovering = e.BoundingBox().Contains(mouse.pos);
                if (e.Hovering)
                {
                    mouse.HoveredObject = e;
                    mouse.HoveringContext = HoveringContext.Enemy;
                }
            });

            if (!GameStats.AttackPhase)
            {
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

        internal void HandleLeftClick(MouseHandler mouse)
        {
            if (mouse.SelectionContext == SelectionContext.PlacingTower && mouse.MouseInGameBounds())
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
        }

        internal void HandleRightClick(MouseHandler mouse)
        {
            if (mouse.SelectionContext == SelectionContext.TowerSelected)
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
        }
    }
}
