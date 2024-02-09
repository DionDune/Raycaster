using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Raycaster
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        uint GameTick;
        List<Keys> Keys_BeingPressed;


        public static List<RayPoint> RayPoints;
        public static Object_Square Square;


        public static Texture2D White;
        Settings Settings;

        #region Initialize

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            GameTick = 0;
            Keys_BeingPressed = new List<Keys>();
            Settings = new Settings();

            RayPoints = new List<RayPoint>();
            Square = new Object_Square() { X = 0, Y = 0 };


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //Procedurally Creating and Assigning a 1x1 white texture to Color_White
            White = new Texture2D(GraphicsDevice, 1, 1);
            White.SetData(new Color[1] { Color.White });
        }

        #endregion

        /////////////////////////////////////////



        /////////////////////////////////////////

        #region Fundamentals

        void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(Length, Thickness);

            _spriteBatch.Draw(White, point, null, Color, Angle, origin, scale, SpriteEffects.None, 0);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            Square.X = Mouse.GetState().X;
            Square.Y = Mouse.GetState().Y;


            if (Keyboard.GetState().IsKeyDown(Keys.V) && !Keys_BeingPressed.Contains(Keys.V))
            {
                Settings.RenderFromFunction = !Settings.RenderFromFunction;
            }
            Keys_BeingPressed = Keyboard.GetState().GetPressedKeys().ToList();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Settings.BackgroundColor);

            _spriteBatch.Begin();


            // Square
            _spriteBatch.Draw(White, new Rectangle(Square.X, 
                                                   Square.Y, 
                                                   Object_Square.DefaultWidth, Object_Square.DefaultHeight), Object_Square.Color);

            //Ray points
            List<RayPoint> rayPoints = new List<RayPoint>();
            Cast.CastRaysFrom(_spriteBatch, 1000, 600, rayPoints);
            Cast.CastRaysFrom(_spriteBatch, 1100, 300, rayPoints);
            Cast.CastRaysFrom(_spriteBatch, 500, 500, rayPoints);


            if (!Settings.RenderFromFunction && Settings.RenderPoints)
            {
                foreach (RayPoint RPoint in rayPoints)
                {
                    _spriteBatch.Draw(White, new Rectangle((int)RPoint.X - Settings.RayPointHalfSize,
                                                           (int)RPoint.Y - Settings.RayPointHalfSize,
                                                           Settings.RayPointSize, Settings.RayPointSize), Settings.RayPointColor * RPoint.Opacity);
                }
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}