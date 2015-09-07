using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;

namespace Lumberjack.Source.UI
{
    class Screen
    {
        public static List<Screen> screens = new List<Screen>();

        public bool visible;
        public string name;
        public List<Button> buttons;
        public List<Label> labels;

        public Screen(bool visible, string name)
        {
            this.visible = visible;
            this.name = name;
            screens.Add(this);
            this.buttons = new List<Button>();
            this.labels = new List<Label>();
        }

        /// <summary>
        /// makes a screen visible
        /// </summary>
        /// <param name="name"></param>
        public static void showScreen(string name)
        {
            foreach (Screen s in screens)
                if (s.name == name)
                    s.visible = true;
        }

        /// <summary>
        /// makes a screen visible, and hides all others
        /// </summary>
        public static void goToScreen(string name)
        {
            foreach (Screen s in screens)
                if (s.name == name)
                    s.visible = true;
                else
                    s.visible = false;
        }

        /// <summary>
        /// gets if screen is visible
        /// </summary>
        public static bool isScreenVisible(string name)
        {
            bool v = false;
            foreach (Screen s in screens)
                if (s.name == name)
                    v = s.visible;
            return v;
        }

        /// <summary>
        /// guess what this does
        /// </summary>
        public static void Update()
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State == TouchLocationState.Released)
                {
                    // on button press
                    foreach (Screen s in screens)
                    {
                        if (s.visible)
                        {
                            foreach (Button b in s.buttons)
                            {
                                if (b.rect.Contains((int)tl.Position.X, (int)tl.Position.Y))
                                    b.click();
                            }
                        }
                    }
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Screen s in screens)
            {
                if (s.visible)
                {
                    foreach (Button b in s.buttons)
                        spriteBatch.Draw(b.tex, b.pos, null, Color.White, 0f, new Vector2(b.tex.Width, b.tex.Height) * .5f, 1f, SpriteEffects.None, 0f);
                    foreach (Label l in s.labels)
                        spriteBatch.Draw(l.tex, l.pos, null, Color.White, 0f, new Vector2(l.tex.Width, l.tex.Height) * .5f, 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
