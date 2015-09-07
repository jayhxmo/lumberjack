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
using Windows.Storage;


namespace Lumberjack.Source.Mechanics
{
    public class Score
    {
        Viewport viewport;
        public SpriteFont spriteFont;
        public int score;
        public int highScore = 0;
        public bool display = false;
        int highscore;

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
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(viewport.Width * .5f + 3, viewport.Height * .5f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(viewport.Width * .5f - 3, viewport.Height * .5f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(viewport.Width * .5f, viewport.Height * .5f + 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(viewport.Width * .5f, viewport.Height * .5f - 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            #endregion
            spriteBatch.DrawString(spriteFont, score.ToString(), new Vector2(viewport.Width * .5f, viewport.Height * .5f), Color.White, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            // high score
            size = spriteFont.MeasureString("High score: " + highscore);
            if (display)
            {
                saveScore();
                #region outline
                spriteBatch.DrawString(spriteFont, "High score: " + highscore, new Vector2(viewport.Width * .5f + 3, viewport.Height * .4f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spriteFont, "High score: " + highscore, new Vector2(viewport.Width * .5f - 3, viewport.Height * .4f), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spriteFont, "High score: " + highscore, new Vector2(viewport.Width * .5f, viewport.Height * .4f + 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(spriteFont, "High score: " + highscore, new Vector2(viewport.Width * .5f, viewport.Height * .4f - 3), Color.Black, 0, size * .5f, 1f, SpriteEffects.None, 0f);
                #endregion

                spriteBatch.DrawString(spriteFont, "High score: " + highscore, new Vector2(viewport.Width * .5f, viewport.Height * .4f), Color.White, 0, size * .5f, 1f, SpriteEffects.None, 0f);
            }
        }

        private async void callHS()
        {
            try
            {
                StorageFile sampleFile = await ApplicationData.Current.LocalFolder.GetFileAsync("dataFile.txt");
                highscore = Convert.ToInt32(await FileIO.ReadTextAsync(sampleFile));
            }

            catch (Exception)
            {
            }
        }

        public int getHighScore()
        {
            callHS();
            return highscore;
        }

        public async void saveScore()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            try { StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt"); } //Create if doesn't exist
            catch { }

            int hs = getHighScore();

            if (score > hs)
            {
                StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                await FileIO.WriteTextAsync(sampleFile, score.ToString());
                //await FileIO.WriteTextAsync(sampleFile, "0"); // reset
            }
        }
    }
}
