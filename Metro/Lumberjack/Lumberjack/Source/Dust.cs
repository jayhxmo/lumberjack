using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lumberjack.Source
{
    class Dust
    {
        public static List<Dust> dust = new List<Dust>();
        public static Texture2D dustTex;

        public Vector2 pos;
        public Vector2 vel;
        public float rot;
        public float rvel;
        public float life;
        public float scale;
        public float a = 1f;

        public Dust(Vector2 pos, Vector2 vel, float rvel, float scale)
        {
            this.pos = pos;
            this.vel = vel;
            this.scale = scale;
            this.life = 1f;
            this.rvel = rvel;
            dust.Add(this);
        }

        public static void Update(GameTime gameTime)
        {
            List<Dust> remove = new List<Dust>();

            foreach (Dust d in dust)
            {
                d.pos += d.vel;
                d.rot += d.rvel;
                d.vel *= .99f;
                d.life -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                d.a = MathHelper.Clamp(d.life/.25f, 0, 1);

                if (d.life < 0)
                    remove.Add(d);
            }

            foreach (Dust d in remove)
            {
                dust.Remove(d);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Dust d in dust)
            {
                spriteBatch.Draw(dustTex, d.pos, null, Color.White * d.a, d.rot, new Vector2(dustTex.Width, dustTex.Height) * .5f, d.scale, SpriteEffects.None, 0f);
            }
        }
    }
}
