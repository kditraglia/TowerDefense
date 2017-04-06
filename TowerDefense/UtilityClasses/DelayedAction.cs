using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class DelayedAction
    {
        public Action Action { get; private set; }
        public float Delay { get; private set; }
        public float TimeRemaining { get; private set; }

        public DelayedAction(Action action, float delay)
        {
            TimeRemaining = delay;
            Action = action;
            Delay = delay;
        }

        public bool Update(GameTime gameTime)
        {
            TimeRemaining -= gameTime.ElapsedGameTime.Milliseconds;

            if (TimeRemaining <= 0)
            {
                Action();
                return true;
            }

            return false;
        }
    }
}
