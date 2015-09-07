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
using MicrosoftAdvertising;
using MicrosoftAdvertising.Shared;
using MicrosoftAdvertising.Shared.WinRT;

namespace Lumberjack
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private string adUnitLeft = "184485";
        private string adUnitRight = "184484";
        private string appID = "71657e60-ed07-44b1-aa50-c8f4c2121012";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Viewport viewport;
        public Viewport displayPort;

        public Player MainPlayer;
        public Tree Tree;

        Texture2D groundtex;
        Texture2D bgTreetex;

        public bool inGame = false;
        public bool isPaused = false;
        public bool reset = false;
        public bool exit = false;
        public bool saveScore = false;
        public bool movingTree = false;
        public bool chopTree = false;
        public bool playChop = false;
        public bool leftDown = false;
        public bool rightDown = false;
        public bool prevClick = false;

        MouseState mouse;

        public static Game1 me;
        public Health playerHealth;

        public SoundEffect chop;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            viewport = GraphicsDevice.Viewport;
            displayPort = viewport;
            displayPort.Width -= 320;
            displayPort.X = 160;

            MainPlayer = new Player(displayPort, viewport);
            Tree = new Tree(this.Content, displayPort);

            playerHealth = new Health(viewport, this.Content);

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MainPlayer.LoadContent(Content);

            Dust.dustTex = Content.Load<Texture2D>("dust");
            groundtex = Content.Load<Texture2D>("Ground");
            bgTreetex = Content.Load<Texture2D>("Tree/BGTree");

            Texture2D play = Content.Load<Texture2D>("tap/btnPlay");
            Texture2D youlose = Content.Load<Texture2D>("tap/lose");
            Texture2D logo = Content.Load<Texture2D>("tap/logo");
            Texture2D tapLeft = Content.Load<Texture2D>("tap/Left");
            Texture2D tapRight = Content.Load<Texture2D>("tap/Right");

            Screen main = new Screen(true, "main");
            new Button(play, new Vector2(viewport.Width * .5f, viewport.Height * 0.75f), buttonAction.pregame, main);
            new Label(logo, new Vector2(viewport.Width * .5f, viewport.Height * 0.25f), main);

            Screen pregame = new Screen(false, "pregame");
            new Label(tapLeft, new Vector2((viewport.Width * .5f) - (100), viewport.Height * 0.75f), pregame);
            new Label(tapRight, new Vector2((viewport.Width * .5f) + (100), viewport.Height * 0.75f), pregame);
            new Button(play, new Vector2(viewport.Width * .5f, viewport.Height * 0.75f), buttonAction.startgame, pregame);

            Screen lose = new Screen(false, "lose");
            new Label(youlose, new Vector2(viewport.Width * .5f, viewport.Height * 0.25f), lose);
            new Button(play, new Vector2(viewport.Width * .5f, viewport.Height * 0.75f), buttonAction.startgame, lose);

            chop = Content.Load<SoundEffect>("chop");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            KeyboardState state = Keyboard.GetState();
            mouse = Mouse.GetState();

            if (Screen.isScreenVisible("pregame"))
            {
                MainPlayer.ScoreUpdate(false);

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    prevClick = true;
                }

                if (mouse.LeftButton == ButtonState.Released)
                {
                    if (prevClick)
                        resetGame();
                }

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
                Tree.Update(ref chopTree, ref movingTree, MainPlayer.side, ref saveScore, ref inGame);
            }

            if (inGame)
            {
                if (isPaused)
                {
                    // code for paused state?
                }

                else
                {
                    if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
                    {
                        if (!leftDown)
                            MainPlayer.TapUpdate(new Vector2(0, 0), movingTree, ref chopTree, ref playerHealth.life, ref playerHealth.timeMS, ref playChop);

                        leftDown = true;
                    }

                    if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
                    {
                        if (!rightDown)
                            MainPlayer.TapUpdate(new Vector2(viewport.Width, 0), movingTree, ref chopTree, ref playerHealth.life, ref playerHealth.timeMS, ref playChop);

                        rightDown = true;
                    }

                    if (state.IsKeyUp(Keys.A) && state.IsKeyUp(Keys.Left))
                    {
                        leftDown = false;
                    }

                    if (state.IsKeyUp(Keys.D) && state.IsKeyUp(Keys.Right))
                    {
                        rightDown = false;
                    }


                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        MainPlayer.TapUpdate(new Vector2(mouse.X, mouse.Y), movingTree, ref chopTree, ref playerHealth.life, ref playerHealth.timeMS, ref playChop);
                    }

                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        MainPlayer.prevTapState = false;
                    }

                    TouchCollection touchCollection = TouchPanel.GetState();
                    foreach (TouchLocation tl in touchCollection)
                    {
                        if (tl.State == TouchLocationState.Pressed)
                        {
                            MainPlayer.TapUpdate(tl.Position, movingTree, ref chopTree, ref playerHealth.life, ref playerHealth.timeMS, ref playChop);
                        }

                        else if (tl.State == TouchLocationState.Released)
                        {
                            MainPlayer.prevTapState = false;
                        }
                    }
                    MainPlayer.Update(gameTime);
                    MainPlayer.ScoreUpdate(false);
                    Tree.Update(ref chopTree, ref movingTree, MainPlayer.side, ref saveScore, ref inGame);
                    playerHealth.Update(gameTime, ref saveScore, ref inGame);
                }
            }
            
            else
            {
                // code for not in game state?
                MainPlayer.ScoreUpdate(true);
            }

            Dust.Update(gameTime);
            Screen.Update(ref inGame, ref isPaused, ref reset, ref exit);

            if (reset)
            {
                resetGame();
                reset = false;
            }

            if (exit)
            {
                exit = false;
                Exit();
            }

            if (playChop)
            {
                playChop = false;
                chop.Play();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
                #region backgroundTrees
                drawBGTree(10, 64, 0.2f);
                drawBGTree(90, 50, 0.3f);
                drawBGTree(200, 35, 0.5f);
                drawBGTree(250, 98, 0f);
                drawBGTree(380, 60, 0.25f);
                drawBGTree(470, 20, .5f);
                drawBGTree(520, 64, 0.2f);
                drawBGTree(570, 50, 0.3f);
                drawBGTree(660, 35, 0.5f);
                drawBGTree(730, 98, 0f);
                drawBGTree(900, 60, 0.25f);
                drawBGTree(1000, 20, .5f);
                drawBGTree(1080, 64, 0.2f);
                drawBGTree(1280, 50, 0.3f);
                drawBGTree(1350, 35, 0.5f);
                drawBGTree(1460, 98, 0f);
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
                spriteBatch.Draw(bgTreetex, new Rectangle(displayPort.X + x, y, w, bgTreetex.Height), Color.Lerp(Color.White, Color.Black, b));
        }

        public void resetGame()
        {
            if (Tree.genNewTree)
                Tree = new Tree(this.Content, displayPort);
            MainPlayer.score.score = 0;
            playerHealth.life = 50;
            Screen.goToScreen("");
            inGame = true;
            isPaused = false;
        }
    }
}
