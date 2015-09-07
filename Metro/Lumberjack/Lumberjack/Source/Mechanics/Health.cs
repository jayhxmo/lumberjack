using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        Viewport viewport;
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
            viewport = vp;

            containerSize = new Vector2(viewport.Width * 0.3f, viewport.Width * 0.025f);
            containerLoc = new Vector2((viewport.Width - containerSize.X) / 2, vp.Height / 10);
            healthBarSize = containerSize;

            container = content.Load<Texture2D>(@"Tap\HealthContainer");
            healthBar = content.Load<Texture2D>(@"Tap\Healthbar");
        }

        public void Update(GameTime gt, ref bool needToSaveScore, ref bool inGame)
        {
            if (life <= 0)
            {
                needToSaveScore = true;
                Tree.Tree.genNewTree = true;
                inGame = false;
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
            spritebatch.Draw(healthBar, new Rectangle((int)containerLoc.X, (int)containerLoc.Y, (int)(containerSize.X * (life / 100f)), (int)healthBarSize.Y), Color.White);
            spritebatch.Draw(container, new Rectangle((int)containerLoc.X, (int)containerLoc.Y, (int)containerSize.X, (int)containerSize.Y), Color.White);
        }

    }
}
