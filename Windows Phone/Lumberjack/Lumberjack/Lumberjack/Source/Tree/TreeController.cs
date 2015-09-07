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
    public class TreeController
    {
        Viewport viewport;
        Vector2 size;
        int StandardX;

        Texture2D baseTreeTxtr;
        Texture2D[] sides = new Texture2D[3];
        /// <summary>
        /// IMPORTANT - MUST READ
        /// [0] - TREE WITH BRANCH ON LEFT
        /// [1] - TREE WITH BRANCH ON RIGHT
        /// [2] - TREE WITH NO BRANCHES
        /// </summary>

        float[] locations = new float[2];

        List<Tree> Trees;
        float DownRate;
        ContentManager content;

        public TreeController(Viewport vp, ContentManager content)
        {
            Trees = new List<Tree>();

            baseTreeTxtr = content.Load<Texture2D>(@"Tree/Base");
            sides[0] = content.Load<Texture2D>(@"Tree/LogLeft");
            sides[1] = content.Load<Texture2D>(@"Tree/LogRight");
            sides[2] = content.Load<Texture2D>(@"Tree/LogNEU");

            viewport = vp;
            float xSize = viewport.Width * 0.5f;
            size = new Vector2(xSize, xSize * 0.7407f);
            StandardX = Convert.ToInt32(viewport.Width / 2);

            DownRate = size.Y;
            float sizeX = viewport.Width * 0.25f;

            Vector2 baseSize = new Vector2(sizeX, sizeX * 0.1125f);
            Vector2 baseLocation = new Vector2(StandardX, viewport.Height - (baseSize.Y * 7));
            Tree baseLog = new Tree(baseLocation, baseSize, baseTreeTxtr);

            locations[0] = xSize / 4;
            locations[1] = StandardX - (xSize / 4);
            Trees.Add(baseLog); 

            for (int i = 1; i < 12; i++)
            {
                Tree prevT = Trees[i - 1];
                Tree t = new Tree(new Vector2(locations[i % 2], prevT.location.Y - size.Y), size, sides[i % 2]);
                Trees.Add(t);
            }
        }

        public void Update()
        {
            for (int i = 1; i < Trees.Count; i++)
            {
                Trees[i].Update(DownRate);
            }

            Trees.RemoveAt(1);
            Tree prevT = Trees[Trees.Count - 1];
            Tree t = new Tree(new Vector2(locations[0], prevT.location.Y - size.Y), size, sides[0]);
            Trees.Add(t);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < Trees.Count; i++)
            {
                Trees[i].Draw(spritebatch);
            }
        }
    }
}
