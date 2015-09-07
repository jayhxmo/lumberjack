using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lumberjack.Source.UI
{
    enum buttonAction
    {
        startgame,
        exitgame
    }
    class Button
    {
        public Texture2D tex;
        public Vector2 pos;
        public buttonAction action;
        public Rectangle rect;

        /// <summary>
        /// Position is the CENTER of the button
        /// </summary>
        public Button(Texture2D tex, Vector2 pos, buttonAction action, Screen screen)
        {
            this.tex = tex;
            this.pos = pos;
            this.action = action;
            screen.buttons.Add(this);
            Vector2 p = this.pos - new Vector2(this.tex.Width, this.tex.Height) * .5f; // translate up and left my 1/2 the size for hitbox
            this.rect = new Rectangle((int)p.X, (int)p.Y, this.tex.Width, this.tex.Height);
        }

        public void click()
        {
            switch (this.action)
            {
                case buttonAction.startgame:
                    {
                        Main.inGame = true;
                        Main.isPaused = false;
                        break;
                    }
                case buttonAction.exitgame:
                    {
                        Main.me.Exit();
                        break;
                    }
            }
        }
    }
}
