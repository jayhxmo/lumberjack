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
    public class Score
    {
        Viewport viewport;
        public SpriteFont spriteFont;
        public int score;
        public int highScore;
        public bool display = false;

        public Score (Viewport vp, ContentManager content)
	    {
            viewport = vp;
            spriteFont = content.Load<SpriteFont>("ScoreFont");
            score = 0;
            highScore = getHighScore();
	    }

        public void Draw(SpriteBatch spriteBatch)
        {
            //current score
            Vector2 size = spriteFont.MeasureString(score.ToString());
            #region outline
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(Main.me.viewport.Width * .5f + 3, Main.me.viewport.Height * .5f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(Main.me.viewport.Width * .5f - 3, Main.me.viewport.Height * .5f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(Main.me.viewport.Width * .5f, Main.me.viewport.Height * .5f + 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(Main.me.viewport.Width * .5f, Main.me.viewport.Height * .5f - 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            #endregion
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(Main.me.viewport.Width * .5f, Main.me.viewport.Height * .5f), Color.White, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            // high score
            size = spriteFont.MeasureString("High score: " + highScore);
            if (display)
            {
                #region outline
                spriteBatch.DrawString(spriteFont, "High score: " + highScore, new Vector2(Main.me.viewport.Width * .5f + 3, Main.me.viewport.Height * .4f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spriteFont, "High score: " + highScore, new Vector2(Main.me.viewport.Width * .5f - 3, Main.me.viewport.Height * .4f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spriteFont, "High score: " + highScore, new Vector2(Main.me.viewport.Width * .5f, Main.me.viewport.Height * .4f + 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spriteFont, "High score: " + highScore, new Vector2(Main.me.viewport.Width * .5f, Main.me.viewport.Height * .4f - 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                #endregion

                spriteBatch.DrawString(spriteFont, "High score: " + highScore, new Vector2(Main.me.viewport.Width * .5f, Main.me.viewport.Height * .4f), Color.White, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            }
        }

        public int getHighScore()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            int highscore = 0;

            // read high score
            try
            {
                using (BinaryReader reader = new BinaryReader(new IsolatedStorageFileStream("scores", FileMode.Open, FileAccess.Read, storage)))
                {
                    highscore = reader.ReadInt32();
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return highscore;
        }

        public void saveScore()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            int highscore = getHighScore();

            if (score > highscore)
            {
                highScore = score;
                try
                {
                    using (BinaryWriter writer = new BinaryWriter(new IsolatedStorageFileStream("scores", FileMode.OpenOrCreate, FileAccess.Write, storage)))
                    {
                        writer.Write(score);
                        writer.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
