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

        Random random = new Random();

        public static Texture2D White;
        public static Texture2D Sky;
        public static Texture2D Key;
        List<Keys> Keys_BeingPressed;

        Settings Settings;
        Player Player;

        public static List<List<Color?>> Grid;
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
            Grid = new List<List<Color?>>();
            for (int y = 0; y < _graphics.PreferredBackBufferHeight / GridScreenDivisor; y++)
            {
                Grid.Add(new List<Color?>());
                for (int x = 0; x < _graphics.PreferredBackBufferWidth / GridScreenDivisor; x++)
                {
                    Grid[y].Add(null);
                }
            }


            CreateMaze(14);

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
        }


        public void CreateMaze(int CubeSize)
        {
            CreateGridSquare(3, 1, CubeSize);
            CreateGridSquare(4, 1, CubeSize);
            CreateGridSquare(5, 1, CubeSize);
            CreateGridSquare(6, 1, CubeSize);

            CreateGridSquare(2, 2, CubeSize);
            CreateGridSquare(7, 2, CubeSize);

            CreateGridSquare(2, 3, CubeSize);
            CreateGridSquare(4, 3, CubeSize);
            CreateGridSquare(5, 3, CubeSize);
            CreateGridSquare(7, 3, CubeSize);

            CreateGridSquare(1, 4, CubeSize);
            CreateGridSquare(4, 4, CubeSize);
            CreateGridSquare(5, 4, CubeSize);
            CreateGridSquare(7, 4, CubeSize);
            CreateGridSquare(8, 4, CubeSize);
            CreateGridSquare(9, 4, CubeSize);

            CreateGridSquare(1, 5, CubeSize);
            CreateGridSquare(4, 5, CubeSize);
            CreateGridSquare(5, 5, CubeSize);
            CreateGridSquare(10, 5, CubeSize);

            CreateGridSquare(1, 6, CubeSize);
            CreateGridSquare(3, 6, CubeSize);
            CreateGridSquare(4, 6, CubeSize);
            CreateGridSquare(7, 6, CubeSize);
            CreateGridSquare(8, 6, CubeSize);
            CreateGridSquare(10, 6, CubeSize);

            CreateGridSquare(1, 7, CubeSize);
            CreateGridSquare(3, 7, CubeSize);
            CreateGridSquare(6, 7, CubeSize);
            CreateGridSquare(7, 7, CubeSize);
            CreateGridSquare(8, 7, CubeSize);
            CreateGridSquare(10, 7, CubeSize);

            CreateGridSquare(1, 8, CubeSize);
            CreateGridSquare(3, 8, CubeSize);
            CreateGridSquare(5, 8, CubeSize);
            CreateGridSquare(6, 8, CubeSize);
            CreateGridSquare(7, 8, CubeSize);
            CreateGridSquare(8, 8, CubeSize);
            CreateGridSquare(11, 8, CubeSize);

            CreateGridSquare(1, 9, CubeSize);
            CreateGridSquare(3, 9, CubeSize);
            CreateGridSquare(5, 9, CubeSize);
            CreateGridSquare(6, 9, CubeSize);
            CreateGridSquare(7, 9, CubeSize);
            CreateGridSquare(8, 9, CubeSize);
            CreateGridSquare(9, 9, CubeSize);
            CreateGridSquare(11, 9, CubeSize);

            CreateGridSquare(1, 10, CubeSize);
            CreateGridSquare(5, 10, CubeSize);
            CreateGridSquare(6, 10, CubeSize);
            CreateGridSquare(7, 10, CubeSize);
            CreateGridSquare(8, 10, CubeSize);
            CreateGridSquare(9, 10, CubeSize);
            CreateGridSquare(11, 10, CubeSize);

            CreateGridSquare(2, 11, CubeSize);
            CreateGridSquare(3, 11, CubeSize);
            CreateGridSquare(4, 11, CubeSize);
            CreateGridSquare(10, 11, CubeSize);

            Grid[34][64] = Color.Gold;
        }
        public void CreateGridSquare(int x, int y, int Size)
        {
            int StartX = x * Size;
            int StartY = y * Size;

            for (int i = 0; i < Size; i++)
            {
                for (int X = 0; X < Size; X++)
                {
                    Grid[StartY + i][StartX + X] = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                }
            }
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
                        Grid[(Mouse.GetState().Y / GridScreenDivisor) + y]
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