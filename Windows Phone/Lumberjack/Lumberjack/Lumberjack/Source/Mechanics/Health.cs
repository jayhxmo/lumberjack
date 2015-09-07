using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Resources;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Lumberjack.Source.Player;

namespace Lumberjack.Source.Mechanics
{
    public class Health
    {
        Texture2D container;
        Texture2D healthBar;
        Vector2 containerLoc;
        Vector2 containerSize;
        public Vector2 healthBarSize;

        public float timeMS = 50;

        public float life = 100;

        float timer = 0;

        public bool gg = false;

        public Health(Viewport vp, ContentManager content)
        {
            containerSize = new Vector2(vp.Width * 0.5f, (vp.Width * 0.5f) / 5f);
            containerLoc = new Vector2((vp.Width / 2) - (containerSize.X / 2), vp.Height / 10);
            healthBarSize = containerSize;

            container = content.Load<Texture2D>(@"Tap\HealthContainer");
            healthBar = content.Load<Texture2D>(@"Tap\Healthbar");
        }

        public void Update(GameTime gt)
        {
            if (life <= 0)
            {
                Main.me.MainPlayer.score.saveScore();
                Tree.Tree.genNewTree = true;
                Main.me.inGame = false;
                UI.Screen.showScreen("lose");
            }

            timer += (float)gt.ElapsedGameTime.TotalMilliseconds;
            if (timer > timeMS)
            {
                life--;
                timer = 0;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(healthBar, new Rectangle((int)containerLoc.X, (int)containerLoc.Y, (int)((Main.me.viewport.Width * 0.5f) * (life / 100f)), (int)healthBarSize.Y), Color.White);
            spritebatch.Draw(container, new Rectangle((int)containerLoc.X, (int)containerLoc.Y, (int)(Main.me.viewport.Width * 0.5f), (int)containerSize.Y), Color.White);
        }

    }
}
