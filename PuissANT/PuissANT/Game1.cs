﻿#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuissANT.Actors;
using PuissANT.Actors.Ants;
using PuissANT.Pheromones;
using PuissANT.ui;
using PuissANT.Util;

#endregion

namespace PuissANT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 Instance;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle GameWindow;

        Texture2D antTexture;

        bool queenPlaced = false;
        int titleOffsetX;
        int titleOffsetY;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.ScreenSize.Y;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            IsMouseVisible = false;
            

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

            // TODO: use this.Content to load your game content here

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent();
            ResourceManager.Instance.LoadContent();
            PhermoneCursor.Instance.LoadContent(Content);

            // Load Cursor Icons
            Texture2D soldierPhermone = Content.Load<Texture2D>("phermones/SoldierPhermone");
            
            int gameWindowVerticalOffset = (int)ScreenManager.Instance.UiManager.PanelList[0].Dimensions.Y;
            int gameWindowHorizontalOffset = (int)ScreenManager.Instance.UiManager.PanelList[1].Dimensions.X;

            GameWindow = new Rectangle(0, gameWindowVerticalOffset,
                (int)ScreenManager.Instance.ScreenSize.X - gameWindowHorizontalOffset,
                (int)ScreenManager.Instance.ScreenSize.Y - gameWindowVerticalOffset);
            ScreenManager.Instance.GameWindow = GameWindow;

            TerrainManager.Initialize(GraphicsDevice, GameWindow);
            World.Init((short)GameWindow.Width, (short)GameWindow.Height, TileInfo.GroundUndug);
            
            // Load the title into the world
            Int32[] buffer = new Int32[28160];
            Image img = new Image();
            img.LoadContent("title/Title_0", String.Empty);
            img.Texture.GetData<Int32>(buffer, 0, 28160);
            titleOffsetX = GameWindow.Width / 2 - 200;
            titleOffsetY = GameWindow.Height / 5;
            for (int y = 0; y < 80; y++)
            {
                for (int x = 0; x < 352; x++)
                {
                    if (buffer[x + y * 352] == -16777216)
                        World.Instance[x + titleOffsetX, y + titleOffsetY - 28] = (short)TileInfo.GroundUndug;
                    else
                        World.Instance[x + titleOffsetX, y + titleOffsetY - 28] = (short)TileInfo.Sky;
                }
            }
            for (int x = 0; x < GameWindow.Width / 2 - 140; x++)
            {
                World.Instance[x, titleOffsetY + 8] = (short)TileInfo.Sky;
                World.Instance[x, titleOffsetY + 9] = (short)TileInfo.Sky;
                World.Instance[x, titleOffsetY + 10] = (short)TileInfo.Sky;
                World.Instance[x, titleOffsetY + 11] = (short)TileInfo.Sky;
                World.Instance[x + GameWindow.Width / 2 + 140, titleOffsetY - 1] = (short)TileInfo.Sky;
                World.Instance[x + GameWindow.Width / 2 + 140, titleOffsetY - 2] = (short)TileInfo.Sky;
                World.Instance[x + GameWindow.Width / 2 + 140, titleOffsetY - 3] = (short)TileInfo.Sky;
                World.Instance[x + GameWindow.Width / 2 + 140, titleOffsetY - 4] = (short)TileInfo.Sky;
            }

            //antTexture = Content.Load<Texture2D>("ants/fireant.png");
            /*antTexture = new Texture2D(graphics.GraphicsDevice, 2, 2);
            Color[] colorBuf = new Color[antTexture.Width * antTexture.Height];
            for (int i = 0; i < colorBuf.Length; i++)
            {
                colorBuf[i] = Color.Blue;
            }
            antTexture.SetData<Color>(colorBuf);*/

            QueenAnt queen = new QueenAnt(new Point(GameWindow.Width / 2, (GameWindow.Height/5)), 6, 6); 
            ActorManager.Instance.Add(queen);

            /*Random r = new Random();
            for (int i = 0; i < 1; i++)
            {
                WorkerAnt ant = new WorkerAnt(
                    new Point(GameWindow.Width / 2, (GameWindow.Height/5)), 1, 1);
                //ant.SetTarget(new Vector2(r.Next(0, GameWindow.Width-1), r.Next(GameWindow.Height/5, GameWindow.Height-1)).ToPoint());
                ActorManager.Instance.Add(ant);
            }*/

            PheromoneManger.Instance.MousePheromoneType = TileInfo.Nest;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            ScreenManager.Instance.UnloadContent();
            ResourceManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            Point mouse = Mouse.GetState().Position;
            Window.Title = "X: " + mouse.X + " Y: " + mouse.Y;

            //Update managers.
            MouseManager.Instance.Update(gameTime);
            ScreenManager.Instance.Update(gameTime);
            ResourceManager.Instance.Update(gameTime);

            //Update user input.
            PhermoneCursor.Instance.Update(gameTime);
            if (MouseManager.Instance.WasJustClicked
                && ScreenManager.Instance.isPointWithinGameWindow(MouseManager.Instance.MousePosition)
                && PheromoneManger.Instance.CanSetPheromone(PheromoneManger.Instance.MousePheromoneType))
            {
                PheromoneManger.Instance.Add(PheromoneManger.Instance.MousePheromoneType, ScreenManager.Instance.getPointWithinGameWindow(MouseManager.Instance.MousePosition));
                
                // If this is the first click in the game, we switch the title image
                if (!queenPlaced)
                {
                    queenPlaced = true;

                    // Switch update screen
                    Int32[] buffer = new Int32[28160];
                    Image img = new Image();
                    img.LoadContent("title/Title_1", String.Empty);
                    img.Texture.GetData<Int32>(buffer, 0, 28160);
                    for (int y = 0; y < GameWindow.Height / 5; y++)
                    {
                        for (int x = 0; x < GameWindow.Width; x++)
                        {
                            World.Instance[x, y] = (short)TileInfo.Sky;
                        }
                    }
                    for (int y = 0; y < 80; y++)
                    {
                        for (int x = 0; x < 352; x++)
                        {
                            if (buffer[x + y * 352] == -16777216)
                                World.Instance[x + 300, y + GameWindow.Height / 5 - 28] = (short)TileInfo.GroundUndug;
                            else
                                if(y > 28)
                                    World.Instance[x + 300, y + GameWindow.Height / 5 - 28] = (short)TileInfo.Sky;
                                else
                                    World.Instance[x + 300, y + GameWindow.Height / 5 - 28] = (short)TileInfo.GroundDug;
                        }
                    }
                    for (int x = 0; x < GameWindow.Width / 2 - 140; x++)
                    {
                        World.Instance[x, titleOffsetY] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 1] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 2] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 3] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 4] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 5] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 6] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 7] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 12] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 13] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 14] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 15] = (short)TileInfo.Sky;
                    }
                }
            }

            foreach (Actor a in ActorManager.Instance.GetAllActors())
                a.Update(gameTime);

            if(isGameOver())
                //handleGameOver

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            TerrainManager.SetTexture();

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            foreach (Actor a in ActorManager.Instance.GetAllActors())
                a.Render(gameTime, spriteBatch);

            TerrainManager.DrawTerrain(spriteBatch);
            ScreenManager.Instance.Draw(spriteBatch);
            PhermoneCursor.Instance.Render(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool isGameOver()
        {
            //return ActorManager.Instance.GetActorsByType<QueenAnt>().First().Health <= 0;
            return false;
        }
    }
}
