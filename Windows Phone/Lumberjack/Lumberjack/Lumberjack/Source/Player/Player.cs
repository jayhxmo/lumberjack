using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Lumberjack;
using Lumberjack.Source.Mechanics;

namespace Lumberjack.Source.Player
{
    public class Player
    {
        Texture2D texture;
        Rectangle src;
        bool flip;
        Vector2 location;
        Vector2 size;

        Vector2 locations;

        Viewport viewport;
        public bool prevTapState = false;

        float timer = 0f;

        public Score score;

        public int side = 0;

        public Player(Viewport vp)
        {
            Initialize(vp);
        }

        private void Initialize(Viewport vp)
        {
            viewport = vp;
           
            size = new Vector2(viewport.Width * 0.35f);

            float xSize = viewport.Width * 0.25f;
            float sizeY = xSize * 1.56f;

            location.Y = viewport.Height - size.Y - (xSize * 0.1125f * 7);

            int center = viewport.Width / 2;
            locations = new Vector2(center - (center / 10) - size.X, center + (center / 10));
            location.X = locations.X;
            
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"Player/player");
            src = new Rectangle(0, 0, 200, 200);
            score = new Score(viewport, content);
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > .1f)
            {
                if (location.X == locations.X)
                {
                    src.X = 0;
                    flip = false;
                    side = 0;
                }

                else if (location.X == locations.Y)
                {
                    src.X = 0;
                    flip = true;
                    side = 1;
                }

                prevTapState = false;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            SpriteEffects fx = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spritebatch.Draw(texture, new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), src, Color.White, 0f, Vector2.Zero, fx, 0f);
            score.Draw(spritebatch);
        }

        public void TapUpdate(Vector2 tapLocation)
        {
            if (!prevTapState)
            {
                if (!Main.me.Tree.moving)
                {
                    if (tapLocation.X < viewport.Width / 2)
                    {
                        src.X = 200;
                        flip = false;
                        location.X = locations.X;
                        side = 0;
                    }
                    else
                    {
                        src.X = 200;
                        flip = true;
                        location.X = locations.Y;
                        side = 1;
                    }
                    Main.me.Tree.Chop();

                    timer = 0f;
                    prevTapState = true;
                    score.score++;
                    Main.me.playerHealth.life += 2.5f;
                    if (Main.me.playerHealth.life > 100)
                        Main.me.playerHealth.life = 100;
                    // difficulty curve
                    Main.me.playerHealth.timeMS = 70 + (1f - (float)(score.score / 100)) * 10;
                    Main.me.chop.Play();
                }
            }
        }

        public void ScoreUpdate(bool highScore)
        {
            score.display = highScore;
        }
    }
}
