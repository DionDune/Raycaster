using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaycastGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Random random = new Random();

        public static Texture2D White;
        public static Texture2D Sky;
        public static Texture2D Key;
        List<Keys> Keys_BeingPressed;

        Settings Settings;
        Player Player;

        public static Texture2D Tree;

        public static Color TreeCol = new Color(0, 1, 0);
        public static List<Point> RenderedSpritePositions = new List<Point>();

        public static List<List<Color?>> GameGrid;
        public static int GridScreenDivisor = 2;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1900;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Keys_BeingPressed = new List<Keys>();



            Player = new Player();
            Settings = new Settings();




            //Create Grid
            int GridSlotCountX = _graphics.PreferredBackBufferWidth / GridScreenDivisor;
            int GridSlotCountY = _graphics.PreferredBackBufferHeight / GridScreenDivisor;
            GameGrid = new List<List<Color?>>();
            Grid.GenerateBase(GameGrid, GridSlotCountX, GridSlotCountY);
            Grid.GenerateMaze(GameGrid, 14, null, true);
            Grid.ScatterSprites(GameGrid, 1000);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //Procedurally Creating and Assigning a 1x1 white texture to Color_White
            White = new Texture2D(GraphicsDevice, 1, 1);
            White.SetData(new Color[1] { Color.White });

            Key = Content.Load<Texture2D>("key");
            Sky = Content.Load<Texture2D>("sky");
            Tree = Content.Load<Texture2D>("Tree1");
        }







        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.V) && !Keys_BeingPressed.Contains(Keys.V))
            {
                Settings.RenderAllPoints = !Settings.RenderAllPoints;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        GameGrid[(Mouse.GetState().Y / GridScreenDivisor) + y]
                                [(Mouse.GetState().X / GridScreenDivisor) + x] = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                    }
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                Player.Rotation -= Player.RoationSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Player.Rotation += Player.RoationSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Player.X -= Player.Speed * (float)Math.Cos((Player.Rotation + 135) * (Math.PI / 180));
                Player.Y -= Player.Speed * (float)Math.Sin((Player.Rotation + 135) * (Math.PI / 180));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Player.X += Player.Speed * (float)Math.Cos((Player.Rotation + 135) * (Math.PI / 180));
                Player.Y += Player.Speed * (float)Math.Sin((Player.Rotation + 135) * (Math.PI / 180));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Player.X += Player.Speed * (float)Math.Cos((Player.Rotation + 45) * (Math.PI / 180));
                Player.Y += Player.Speed * (float)Math.Sin((Player.Rotation + 45) * (Math.PI / 180));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Player.X -= Player.Speed * (float)Math.Cos((Player.Rotation + 45) * (Math.PI / 180));
                Player.Y -= Player.Speed * (float)Math.Sin((Player.Rotation + 45) * (Math.PI / 180));
            }

            Keys_BeingPressed = Keyboard.GetState().GetPressedKeys().ToList();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();



            _spriteBatch.Draw(White, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2 + 40), Settings.SkyColor);

            _spriteBatch.Draw(White, new Rectangle(0, _graphics.PreferredBackBufferHeight / 2 + 40, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Settings.GroundColor);


            Raycast.CastRays(_spriteBatch, Player.X, Player.Y, Player.Rotation);



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}