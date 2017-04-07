using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense
{
    class GameEngine
    {
        List<Tower> towerlist = new List<Tower>();
        List<Enemy> enemylist = new List<Enemy>();
        List<Projectile> projectilelist = new List<Projectile>();
        List<FloatingText> floatingTextList = new List<FloatingText>();
        List<DelayedAction> delayedActions = new List<DelayedAction>();

        internal GameEngine()
        {

        }

        internal void Update(GameTime gameTime)
        {
            delayedActions.RemoveAll(d => d.Update(gameTime));
            if (GameStats.AttackPhase)
            {
                enemylist.RemoveAll(e => e.Update(gameTime));
                towerlist.ForEach(t =>
                {
                    projectilelist = t.Attack(enemylist, projectilelist, gameTime.TotalGameTime.TotalSeconds, (d, p) =>
                    {
                        floatingTextList.Add(new FloatingText(p, ResourceManager.GameFont, d.ToString()));
                    });
                });
                projectilelist.RemoveAll(p => p.Move());
            }
            if (enemylist.Count == 0 && delayedActions.Count == 0 && GameStats.AttackPhase)
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

        internal void StartLevel(List<Node> bestPath)
        {
            GameStats.AttackPhase = true;
            GameStats.Level++;
            MessageLog.Level(GameStats.Level);
            Random random = new Random();

            double num = random.NextDouble();
            if (num < .3)
            {
                for (int i = 0; i < (30 + GameStats.Level * 2); i++)
                {
                    delayedActions.Add(new DelayedAction(() => enemylist.Add(new Enemy(GameStats.Level * 4, 4, ResourceManager.Enemy, new List<Node>(bestPath), "Malaria")), i * 400));
                }
            }
            else if (num < .6)
            {
                for (int i = 0; i < (20 + GameStats.Level * 1.5); i++)
                {
                    delayedActions.Add(new DelayedAction(() => enemylist.Add(new Enemy(GameStats.Level * 8, 2, ResourceManager.Enemy, new List<Node>(bestPath), "Tuberculosis")), i * 400));
                }
            }
            else
            {
                for (int i = 0; i < (20 + GameStats.Level); i++)
                {
                    delayedActions.Add(new DelayedAction(() => enemylist.Add(new Enemy(GameStats.Level * 16, 1, ResourceManager.Enemy, new List<Node>(bestPath), "AIDS")), i * 1000));
                }
            }
        }

        internal void HandleMouseHover(MouseHandler mouse)
        {
            enemylist.ForEach(e =>
            {
                e.Hovering = e.BoundingBox().Contains(mouse.Position);
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
                    t.Hovering = t.BoundingBox().Contains(mouse.Position) && mouse.SelectionContext == SelectionContext.None;
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
                    t.Position = mouse.Position;
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
