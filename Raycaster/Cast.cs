using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    public class Cast
    {
        private static void CastSingleRay(SpriteBatch _spritebatch, int OrigX, int OrigY, float Angle)
        {
            int MaxPoints = (int)(Settings.RayLegnth / Settings.RayJumpDistance);
            float OpacityLoss = 1F / MaxPoints;


            float CurrentOpacity = 1;
            float CurrentX = OrigX;
            float CurrentY = OrigY;
            for (int i = 0; i < MaxPoints; i++)
            {
                CurrentX += Settings.RayJumpDistance * (float)Math.Cos(Angle);
                CurrentY += Settings.RayJumpDistance * (float)Math.Sin(Angle);
                CurrentOpacity -= OpacityLoss;

                // Is colliding with square
                if (!CheckRayCollision(CurrentX, CurrentY))
                {
                    if (Settings.RenderAllPoints)
                    {
                        _spritebatch.Draw(Game1.White, new Rectangle((int)CurrentX - Settings.RayPointHalfSize,
                                                        (int)CurrentY - Settings.RayPointHalfSize,
                                                        Settings.RayPointSize, Settings.RayPointSize), Color.White * CurrentOpacity);
                    }
                }
                else if (Settings.RenderCollisionDistances)
                {
                    Game1.DrawLine(_spritebatch, new Vector2(OrigX, OrigY), i * Settings.RayJumpDistance, Angle, Color.White, 1);

                    return;
                }
                else
                {
                    return;
                }
            }
        }
        private static void CastSingleRay(SpriteBatch _spritebatch, int OrigX, int OrigY, float DistanceFromOrig, float Length, float Angle)
        {
            int MaxPoints = (int)(Length / Settings.RayJumpDistance);
            float PointsBeforeCheck = DistanceFromOrig / Settings.RayJumpDistance;
            float OpacityLoss = 1F / MaxPoints;


            float CurrentOpacity = 1;
            float CurrentX = OrigX + (DistanceFromOrig * (float)Math.Cos(Angle));
            float CurrentY = OrigY + (DistanceFromOrig * (float)Math.Sin(Angle));
            for (int i = 0; i < MaxPoints; i++)
            {
                CurrentX += Settings.RayJumpDistance * (float)Math.Cos(Angle);
                CurrentY += Settings.RayJumpDistance * (float)Math.Sin(Angle);
                CurrentOpacity -= OpacityLoss;

                // Is colliding with square
                if (!CheckRayCollision(CurrentX, CurrentY))
                {
                    if (Settings.RenderAllPoints)
                    {
                        _spritebatch.Draw(Game1.White, new Rectangle((int)CurrentX - Settings.RayPointHalfSize,
                                                        (int)CurrentY - Settings.RayPointHalfSize,
                                                        Settings.RayPointSize, Settings.RayPointSize), Color.White * CurrentOpacity);
                    }
                }
                else if (Settings.RenderCollisionDistances)
                {
                    Game1.DrawLine(_spritebatch, new Vector2(OrigX, OrigY), (i + PointsBeforeCheck) * Settings.RayJumpDistance, Angle, Color.White, 1);

                    return;
                }
                else
                {
                    return;
                }
            }
        }
        public static void CastRaysFrom(SpriteBatch _spritebatch, int X, int Y)
        {
            int RayCount = (int)(360F / Settings.RayAngleJump);

            float CurrentAngle = 0;
            for (int i = 0; i < RayCount; i++)
            {
                CastSingleRay(_spritebatch, X, Y, CurrentAngle * (float)(Math.PI / 180));

                CurrentAngle += Settings.RayAngleJump;
            }
        }

        public static void CastRaysObjectDistanceFocus(SpriteBatch _spritebatch, int X, int Y)
        // This allows *FAR* more detail in the cast. Serves no graphical purpose.
        {
            List<Vector2> BlockPoints = new List<Vector2>()
            {
                new Vector2(Game1.Square.X, Game1.Square.Y),
                new Vector2(Game1.Square.X + Object_Square.DefaultWidth, Game1.Square.Y),
                new Vector2(Game1.Square.X + Object_Square.DefaultWidth, Game1.Square.Y + Object_Square.DefaultHeight),
                new Vector2(Game1.Square.X, Game1.Square.Y + Object_Square.DefaultHeight)
            };

            float DistanceMin = GetDistanceBetween(new Vector2(X, Y), BlockPoints[0]);
            float DistanceMax = 0;
            foreach (Vector2 Pos in BlockPoints)
            {
                float Distance = GetDistanceBetween(new Vector2(X, Y), Pos);
                if (Distance < DistanceMin)
                {
                    DistanceMin = Distance;
                }
                else if (Distance > DistanceMax)
                {
                    DistanceMax = Distance;
                }
            }
            float DistanceRange = DistanceMax - DistanceMin;



            int RayCount = (int)(360F / Settings.RayAngleJump);

            float CurrentAngle = 0;
            for (int i = 0; i < RayCount; i++)
            {
                CastSingleRay(_spritebatch, X, Y, DistanceMin, DistanceRange, CurrentAngle * (float)(Math.PI / 180));

                CurrentAngle += Settings.RayAngleJump;
            }
        }

        public static bool CheckRayCollision(float X, float Y)
        {
            if (X >= Game1.Square.X && X <= Game1.Square.X + Object_Square.DefaultWidth && 
                Y >= Game1.Square.Y && Y <= Game1.Square.Y + Object_Square.DefaultHeight)
            {
                return true;
            }

            return false;
        }


        public static Point GetPositionFrom(int CentreX, int CentreY, int Distance, float Angle)
        {
            float angle = Angle * (float)(Math.PI / 180);

            int x = Convert.ToInt32((Distance * Math.Cos(angle)) + CentreX);
            int y = Convert.ToInt32((Distance * Math.Sin(angle)) + CentreY);

            return new Point(x, y);
        }
        public static float GetDistanceBetween(Vector2 Point1, Vector2 Point2)
        {
            float XDiff = Math.Abs(Point1.X - Point2.X);
            float YDiff = Math.Abs(Point1.Y - Point2.Y);
            float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

            return Distance;
        }





        public static Color? GetScreenRayCollisionType(float X, float Y)
        {
            if (X >= Game1.Square.X && X <= Game1.Square.X + Object_Square.DefaultWidth &&
                Y >= Game1.Square.Y && Y <= Game1.Square.Y + Object_Square.DefaultHeight)
            {
                return Color.Red;
            }
            foreach (Object_Square Square in Game1.Squares)
            {
                if (X >= Square.X && X <= Square.X + Object_Square.DefaultWidth &&
                    Y >= Square.Y && Y <= Square.Y + Object_Square.DefaultHeight)
                {
                    return Square.CurrentColor;
                }
            }

            return null;
        }

        public static void CastScreenRays(SpriteBatch _spritebatch, int X, int Y)
        {
            _spritebatch.Draw(Game1.White, new Rectangle((int)Game1.PlayerX - 3,
                                                         (int)Game1.PlayerY - 3,
                                                         6, 6), Color.Blue);

            /*
            List<Vector2> BlockPoints = new List<Vector2>()
            {
                new Vector2(Game1.Square.X, Game1.Square.Y),
                new Vector2(Game1.Square.X + Object_Square.DefaultWidth, Game1.Square.Y),
                new Vector2(Game1.Square.X + Object_Square.DefaultWidth, Game1.Square.Y + Object_Square.DefaultHeight),
                new Vector2(Game1.Square.X, Game1.Square.Y + Object_Square.DefaultHeight)
            };

            // Idk why this 32 is neccissarry. The cube should be rendered even if the pos is within it
            float DistanceMin = GetDistanceBetween(new Vector2(X, Y), BlockPoints[0]) - 32; 
            float DistanceMax = 0;
            foreach (Vector2 Pos in BlockPoints)
            {
                float Distance = GetDistanceBetween(new Vector2(X, Y), Pos);
                if (Distance < DistanceMin)
                {
                    DistanceMin = Distance;
                }
                else if (Distance > DistanceMax)
                {
                    DistanceMax = Distance;
                }
            }
            float DistanceRange = DistanceMax - DistanceMin;

            */

            int RayCount = 1920;
            float RayAngleJump = 120F / 1920F; // This each pixel

            float CurrentAngle = 0;
            for (int i = 0; i < RayCount; i++)
            {
                //                                      Distance, DistanceRange, instead of 0, 500 for QUALITY
                CastSingleScreenRay(_spritebatch, X, Y, 0, 500, (Game1.PlayerRotation + CurrentAngle) * (float)(Math.PI / 180), i);

                CurrentAngle += RayAngleJump;
            }
            int a = 0;
        }
        private static void CastSingleScreenRay(SpriteBatch _spritebatch, int OrigX, int OrigY, float DistanceFromOrig, float Length, float Angle, int ScreenDistance)
        {
            int MaxPoints = (int)(Length / Settings.GameRayJumpDistance);
            float PointsBeforeCheck = DistanceFromOrig / Settings.GameRayJumpDistance;
            float OpacityLoss = 1.2F / MaxPoints;

            float CurrentX = OrigX + (DistanceFromOrig * (float)Math.Cos(Angle));
            float CurrentY = OrigY + (DistanceFromOrig * (float)Math.Sin(Angle));
            for (int i = 0; i < MaxPoints; i++)
            {
                CurrentX += Settings.GameRayJumpDistance * (float)Math.Cos(Angle);
                CurrentY += Settings.GameRayJumpDistance * (float)Math.Sin(Angle);


                Color? CollionType = GetScreenRayCollisionType(CurrentX, CurrentY);
                // Is not colliding with square
                if (CollionType == null)
                {
                    if (Settings.RenderAllPoints)
                    {
                        _spritebatch.Draw(Game1.White, new Rectangle((int)CurrentX - Settings.RayPointHalfSize,
                                                        (int)CurrentY - Settings.RayPointHalfSize,
                                                        Settings.RayPointSize, Settings.RayPointSize), Color.White * 0.05F);
                    }
                }
                else
                {
                    _spritebatch.Draw(Game1.White, new Rectangle(ScreenDistance, 0, 1, 1080), (Color)CollionType * (1 - (i * OpacityLoss)));

                    return;
                }
            }
        }

    }

    public class RayPoint
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float OriginX { get; set; }
        public float OriginY { get; set; }
        public float AngleFromOrigin { get; set; }
        public float DistanceFromOrigin { get; set; }
        
        public float Opacity { get; set; }


        public RayPoint()
        {
            X = 0; 
            Y = 0;
            OriginX = float.NaN;
            OriginY = float.NaN;
            AngleFromOrigin = float.NaN;
            DistanceFromOrigin = float.NaN;
            Opacity = 1f;
        }
    }
    public class Object_Square
    {
        public const int DefaultWidth = 20;
        public const int DefaultHeight = 20;

        public static Color Color = Color.Red;


        public int X { get; set; }
        public int Y { get; set; }

        public float Opacity { get; set; }

        public Color CurrentColor {get; set;}
    }
}
