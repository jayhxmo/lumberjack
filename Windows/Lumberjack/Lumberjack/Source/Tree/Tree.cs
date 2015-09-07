using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;
using Lumberjack;

namespace Lumberjack.Source.Tree
{
    public enum logType
    {
        noBranch,
        branchLeft1,
        branchLeft2,
        branchRight1,
        branchRight2
    }

    public class Tree
    {
        public static bool genNewTree = false;

        Viewport viewport;

        Texture2D baseTex;
        Texture2D logTex;
        Texture2D branch1Tex;
        Texture2D branch2Tex;

        Random rand;

        public List<logType> logs = new List<logType>();
        int treeBottom = 0;
        public bool moving = false;

        public Tree(ContentManager content, Viewport vp)
        {
            viewport = vp;

            baseTex = content.Load<Texture2D>(@"Tree/Base");
            logTex = content.Load<Texture2D>(@"Tree/Log");
            branch1Tex = content.Load<Texture2D>(@"Tree/Branch1");
            branch2Tex = content.Load<Texture2D>(@"Tree/Branch2");

            treeBottom = viewport.Height - 100 - baseTex.Height;

            rand = new Random();
            for (int i = 0; i < 16; i++)
            {
                if (i != 0)
                    chooseNextBranch(i - 1);
                else
                    logs.Add(logType.noBranch);
            }
        }

        public void Chop()
        {
            chooseNextBranch(15);
            logs.RemoveAt(0);

            for (int i = 0; i < 4; i++)
            {
                new Dust(new Vector2(viewport.X + viewport.Width * .5f + rand.Next(-10,10), treeBottom), new Vector2((float)rand.NextDouble() * 20 - 10, (float)rand.NextDouble() * 2.5f - 1.25f) * 2f, MathHelper.ToRadians(rand.Next(-5, 5)), ((float)rand.NextDouble() + .25f) * 1.5f);
            }
            treeBottom -= logTex.Height;

        }

        public void Update(ref bool chopTree, ref bool movingTree, int MainPlayerSide, ref bool saveScore, ref bool inGame)
        {
            if (treeBottom < viewport.Height - 100 - baseTex.Height)
            {
                treeBottom += 50;
                moving = true;
            }

            else if (treeBottom >= viewport.Height - 100 - baseTex.Height)
            {
                treeBottom = viewport.Height - 100 - baseTex.Height;
                if (moving)
                {
                    if ((MainPlayerSide == 0 && (logs[0] == logType.branchLeft1 || logs[0] == logType.branchLeft2)) ||
                        (MainPlayerSide == 1 && (logs[0] == logType.branchRight1 || logs[0] == logType.branchRight2)))
                    {
                        // player lost
                        saveScore = true;
                        genNewTree = true;
                        inGame = false;
                        UI.Screen.showScreen("lose");
                    }
                }

                moving = false;
            }

            if (chopTree)
            {
                chopTree = false;
                Chop();
            }

            movingTree = moving;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            int y = treeBottom;

            float mid = viewport.Width * .5f;
            spritebatch.Draw(baseTex, new Vector2(viewport.X + mid, viewport.Height - 100 - baseTex.Height), null, Color.White, 0f, new Vector2(baseTex.Width * .5f, 0), 1f, SpriteEffects.None, 0f);
            
            foreach (logType i in this.logs)
            {
                spritebatch.Draw(logTex, new Vector2(viewport.X + mid, y), null, Color.White, 0f, new Vector2(logTex.Width * .5f, logTex.Height), 1f, SpriteEffects.None, 0f);
                switch (i)
                {
                    case logType.branchLeft1:
                        {
                            spritebatch.Draw(branch1Tex, new Vector2(viewport.X + mid - logTex.Width * .5f - branch1Tex.Width, y - logTex.Height * .5f + branch1Tex.Height * .5f), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                            break;
                        }
                    case logType.branchRight1:
                        {
                            spritebatch.Draw(branch1Tex, new Vector2(viewport.X + mid + logTex.Width * .5f, y - logTex.Height * .5f + branch1Tex.Height * .5f), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0f);
                            break;
                        }
                    case logType.branchLeft2:
                        {
                            spritebatch.Draw(branch2Tex, new Vector2(viewport.X + mid - logTex.Width * .5f - branch2Tex.Width, y - logTex.Height * .5f + branch2Tex.Height * .5f), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                            break;
                        }
                    case logType.branchRight2:
                        {
                            spritebatch.Draw(branch2Tex, new Vector2(viewport.X + mid + logTex.Width * .5f, y - logTex.Height * .5f + branch2Tex.Height * .5f), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0f);
                            break;
                        }
                }
                y -= logTex.Height;
            }
        }

        private void chooseNextBranch(int prev)
        {
            if (logs[prev] == logType.branchLeft1 || logs[prev] == logType.branchLeft2)
            {
                logType log = logType.branchRight1;
                while (log == logType.branchRight1 || log == logType.branchRight2)
                {
                    int r = rand.Next(0, 5);
                    if (r < 1)
                        log = logType.noBranch;
                    else
                        log = (logType)(r - 2);
                }
                logs.Add(log);
            }
            else if (logs[prev] == logType.branchRight1 || logs[prev] == logType.branchRight2)
            {
                logType log = logType.branchLeft1;
                while (log == logType.branchLeft1 || log == logType.branchLeft2)
                {
                    int r = rand.Next(0, 5);
                    if (r < 1)
                        log = logType.noBranch;
                    else
                        log = (logType)(r - 2);
                }
                logs.Add(log);
            }
            else
                logs.Add((logType)rand.Next(0, 5));
        }
    }
}
