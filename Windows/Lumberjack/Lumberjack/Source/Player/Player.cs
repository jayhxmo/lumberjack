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
        Viewport actualViewport;

        public bool prevTapState = false;

        float timer = 0f;

        public Score score;

        public int side = 0;

        public Player(Viewport vp, Viewport acv)
        {
            Initialize(vp, acv);
        }

        private void Initialize(Viewport vp, Viewport avp)
        {
            viewport = vp;
            actualViewport = avp;
            size = new Vector2(viewport.Height * 0.2f);

            float xSize = viewport.Width * 0.25f;
            float sizeY = xSize * 1.56f;

            location.Y = viewport.Height - size.Y - (xSize * 0.1125f * 2);

            int center = viewport.Width / 2;
            locations = new Vector2(center - (center / 20) - size.X, center + (center / 20));
            location.X = locations.X;
            
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"Player/player");
            src = new Rectangle(0, 0, 200, 200);
            score = new Score(actualViewport, content);
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
            spritebatch.Draw(texture, new Rectangle((int)location.X + viewport.X, (int)location.Y, (int)size.X, (int)size.Y), src, Color.White, 0f, Vector2.Zero, fx, 0f);
            score.Draw(spritebatch);
        }

        public void TapUpdate(Vector2 tapLocation, bool movingTree, ref bool chopTree, ref float PlayerHealth, ref float timeMS, ref bool playChop)
        {
            if (!prevTapState)
            {
                if (!movingTree)
                {
                    if (tapLocation.X < actualViewport.Width / 2)
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

                    chopTree = true;

                    timer = 0f;
                    prevTapState = true;
                    score.score++;
                    PlayerHealth += 2.5f;
                    if (PlayerHealth > 100)
                        PlayerHealth = 100;

                    // difficulty curve
                    timeMS = 70 + (1f - (float)(score.score / 100)) * 10;
                    playChop = true;
                }
            }
        }

        public void ScoreUpdate(bool highScore)
        {
            score.display = highScore;
        }
    }
}
