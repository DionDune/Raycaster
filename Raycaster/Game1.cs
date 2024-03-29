﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        public static List<Object_Square> Squares;

        public static float PlayerRotation = 180;
        public static float PlayerX = 900;
        public static float PlayerY = 500;
        public static float PlayerSpeed = 2F;

        public static Texture2D White;
        Settings Settings;

        #region Initialize

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
            GameTick = 0;
            Keys_BeingPressed = new List<Keys>();
            Settings = new Settings();

            RayPoints = new List<RayPoint>();
            Square = new Object_Square() { X = 0, Y = 0 };
            Squares = new List<Object_Square>();


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

        public static void DrawLine(SpriteBatch _spriteBatch, Vector2 point, float Length, float Angle, Color Color, float Thickness)
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
                Settings.RenderAllPoints = !Settings.RenderAllPoints;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.C) && !Keys_BeingPressed.Contains(Keys.C))
            {
                Settings.RenderCollisionDistances = !Settings.RenderCollisionDistances;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X) && !Keys_BeingPressed.Contains(Keys.X))
            {
                Settings.GameRender = !Settings.GameRender;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && !Keys_BeingPressed.Contains(Keys.P))
            {
                Squares.Add(new Object_Square()
                {
                    X = Mouse.GetState().X,
                    Y = Mouse.GetState().Y,
                    CurrentColor = Color.Turquoise
                });
            }

            if (Settings.GameRender)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    PlayerRotation -= 0.5F;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    PlayerRotation += 0.5F;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    PlayerX -= PlayerSpeed * (float)Math.Cos((PlayerRotation + 135) * (Math.PI / 180));
                    PlayerY -= PlayerSpeed * (float)Math.Sin((PlayerRotation + 135) * (Math.PI / 180));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    PlayerX += PlayerSpeed * (float)Math.Cos((PlayerRotation + 135) * (Math.PI / 180));
                    PlayerY += PlayerSpeed * (float)Math.Sin((PlayerRotation + 135) * (Math.PI / 180));
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    PlayerX += PlayerSpeed * (float)Math.Cos((PlayerRotation + 45) * (Math.PI / 180));
                    PlayerY += PlayerSpeed * (float)Math.Sin((PlayerRotation + 45) * (Math.PI / 180));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    PlayerX -= PlayerSpeed * (float)Math.Cos((PlayerRotation + 45) * (Math.PI / 180));
                    PlayerY -= PlayerSpeed * (float)Math.Sin((PlayerRotation + 45) * (Math.PI / 180));
                }
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

            if (!Settings.GameRender)
            {
                // Ray points
                Cast.CastRaysObjectDistanceFocus(_spriteBatch, 1000, 600);
                Cast.CastRaysObjectDistanceFocus(_spriteBatch, 1100, 300);
                Cast.CastRaysFrom(_spriteBatch, 500, 500);
            }
            else
            {
                Cast.CastScreenRays(_spriteBatch, (int)PlayerX, (int)PlayerY);

                foreach (Object_Square Square in Squares)
                {
                    _spriteBatch.Draw(White, new Rectangle(Square.X,
                                                           Square.Y,
                                                            Object_Square.DefaultWidth, Object_Square.DefaultHeight), Square.CurrentColor);
                }
            }


            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}