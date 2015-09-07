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
using Lumberjack.Source.Tree;
using Lumberjack.Source.UI;
using Lumberjack.Source;
using Lumberjack.Source.Mechanics;

using Microsoft.Advertising.Mobile.Xna;

namespace Lumberjack
{
    public class Main : Game
    {
        private string adUnit = "184477";
        private string appID = "e012c1ac-a21d-4073-9f84-64c15a448a80";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Viewport viewport;

        public Player MainPlayer;
        public Tree Tree;

        Texture2D groundtex;
        Texture2D bgTreetex;

        public bool inGame = false;
        public bool isPaused = false;

        public static Main me;

        public Health playerHealth;

        public SoundEffect chop;

        DrawableAd advertisement;

        public Main()
        {
            me = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        protected override void Initialize()
        {
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            viewport = GraphicsDevice.Viewport;

            int width = viewport.Width;
            int height = viewport.Height;
            
            if (width > height)
            {
                int temp = width;
                width = height;
                height = temp;
            }

            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            viewport = GraphicsDevice.Viewport;

            MainPlayer = new Player(viewport);
            Tree = new Tree(this.Content);

            playerHealth = new Health(viewport, this.Content);

            AdGameComponent.Initialize(this, appID);
            Components.Add(AdGameComponent.Current);

            // Create an actual ad for display.
            CreateAd();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MainPlayer.LoadContent(Content);

            Dust.dustTex = Content.Load<Texture2D>("dust");
            groundtex = Content.Load<Texture2D>("Ground");
            bgTreetex = Content.Load<Texture2D>("Tree/BGTree");

            Texture2D play = Content.Load<Texture2D>("tap/btnPlay");
            Texture2D ranking = Content.Load<Texture2D>("tap/btnRanking");
            Texture2D youlose = Content.Load<Texture2D>("tap/lose");
            Texture2D logo = Content.Load<Texture2D>("tap/logo");
            Texture2D tapLeft = Content.Load<Texture2D>("tap/Left");
            Texture2D tapRight = Content.Load<Texture2D>("tap/Right");

            Screen main = new Screen(true, "main");
            new Button(play, new Vector2(viewport.Width * .5f, viewport.Height * 0.75f), buttonAction.pregame, main);
            new Button(ranking, new Vector2(viewport.Width * .75f, viewport.Height * 0.75f), buttonAction.pregame, main);
            new Label(logo, new Vector2(viewport.Width * .5f, viewport.Height * 0.25f), main);

            Screen pregame = new Screen(false, "pregame");
            new Label(tapLeft, new Vector2((viewport.Width * .5f) - (100), viewport.Height * 0.75f), pregame);
            new Label(tapRight, new Vector2((viewport.Width * .5f) + (100), viewport.Height * 0.75f), pregame);
            new Button(play, new Vector2(viewport.Width * .5f, viewport.Height * 0.75f), buttonAction.startgame, pregame);

            Screen lose = new Screen(false, "lose");
            new Label(youlose, new Vector2(viewport.Width * .5f, viewport.Height * 0.25f), lose);
            new Button(play, new Vector2(viewport.Width * .5f, viewport.Height * 0.75f), buttonAction.startgame, lose);
            new Button(ranking, new Vector2(viewport.Width * .75f, viewport.Height * 0.75f), buttonAction.startgame, lose);

            chop = Content.Load<SoundEffect>("chop");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Screen.isScreenVisible("pregame"))
            {
                MainPlayer.ScoreUpdate(false);
                TouchCollection touchCollection = TouchPanel.GetState();
                foreach (TouchLocation tl in touchCollection)
                {
                    if (tl.State == TouchLocationState.Released)
                    {
                        resetGame();
                    }
                }
                MainPlayer.Update(gameTime);
                MainPlayer.ScoreUpdate(false);
                Tree.Update();
            }

            if (inGame)
            {
                if (isPaused)
                {
                    // code for paused state?
                }
                else
                {
                    TouchCollection touchCollection = TouchPanel.GetState();
                    foreach (TouchLocation tl in touchCollection)
                    {
                        if (tl.State == TouchLocationState.Pressed)
                        {
                            MainPlayer.TapUpdate(tl.Position);
                        }

                        else if (tl.State == TouchLocationState.Released)
                        {
                            MainPlayer.prevTapState = false;
                        }
                    }
                    MainPlayer.Update(gameTime);
                    MainPlayer.ScoreUpdate(false);
                    Tree.Update();
                    playerHealth.Update(gameTime);
                }
            }
            else
            {
                // code for not in game state?
                MainPlayer.ScoreUpdate(true);
            }
            Dust.Update(gameTime);
            Screen.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            spriteBatch.Begin();
            #region backgroundTrees
            drawBGTree(10, 64, 0.2f);
            drawBGTree(90, 50, 0.3f);
            drawBGTree(200, 35, 0.5f);
            drawBGTree(250, 98, 0f);
            drawBGTree(380, 60, 0.25f);
            drawBGTree(470, 20, .5f);
            #endregion

            for (int x = 0; x < viewport.Width; x += groundtex.Width) // tile ground texture
                spriteBatch.Draw(groundtex, new Rectangle(x, viewport.Height - 132, groundtex.Width, 132), Color.White);

            Tree.Draw(spriteBatch);
            MainPlayer.Draw(spriteBatch);
            Dust.Draw(spriteBatch);
            playerHealth.Draw(spriteBatch);
            Screen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawBGTree(int x, int w, float b)
        {
            for (int y = 0; y < viewport.Height; y += bgTreetex.Height)
                spriteBatch.Draw(bgTreetex, new Rectangle(x, y, w, bgTreetex.Height), Color.Lerp(Color.White, Color.Black, b));
        }

        public void resetGame()
        {
            if (Tree.genNewTree)
                Tree = new Tree(this.Content);
            MainPlayer.score.score = 0;
            playerHealth.life = 50;
            Screen.goToScreen("");
            Main.me.inGame = true;
            Main.me.isPaused = false;
        }

        /// <summary>
        /// Create a DrawableAd with desired properties.
        /// </summary>
        private void CreateAd()
        {
            // Create a banner ad for the game.
            int width = 320;
            int height = 50;
            int x = (GraphicsDevice.Viewport.Bounds.Width - width) / 2; // centered on the display
            int y = 0;

            advertisement = AdGameComponent.Current.CreateAd(adUnit, new Rectangle(x, y, width, height), true);
        }
    }
}
