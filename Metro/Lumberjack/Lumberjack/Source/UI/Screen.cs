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
using Lumberjack;
using Lumberjack.Source.Player;
using Lumberjack.Source.Tree;
using Lumberjack.Source;
using Lumberjack.Source.Mechanics;

namespace Lumberjack.Source.UI
{
    class Screen
    {
        public static List<Screen> screens = new List<Screen>();

        public bool visible;
        public string name;
        public List<Button> buttons;
        public List<Label> labels;
        public static bool prevClick = false;

        public static MouseState mouse;

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
        public static void Update(ref bool inGame, ref bool isPaused, ref bool reset, ref bool exit)
        {
            mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                prevClick = true;
            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                if (prevClick)
                {
                    foreach (Screen s in screens)
                    {
                        if (s.visible)
                        {
                            foreach (Button b in s.buttons)
                            {
                                if (b.rect.Contains(mouse.X, mouse.Y))
                                    b.click(ref inGame, ref isPaused, ref reset, ref exit);
                            }
                        }
                    }
                }
                prevClick = false;
            }

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space) ||
                state.IsKeyDown(Keys.Enter))
            {
                foreach (Screen s in screens)
                {
                    if (s.visible)
                    {
                        foreach (Button b in s.buttons)
                        {
                            b.click(ref inGame, ref isPaused, ref reset, ref exit);
                        }
                    }
                }
            }

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
                                    b.click(ref inGame, ref isPaused, ref reset, ref exit);
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
