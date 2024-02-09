using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    public class Cast
    {
        private static void CastSingleRay(SpriteBatch _spritebatch, int X, int Y, float Angle, List<RayPoint> Points)
        {
            int MaxPoints = (int)(Settings.RayLegnth / Settings.RayJumpDistance);
            float OpacityLoss = 1F / MaxPoints;


            float CurrentOpacity = 1;
            float CurrentX = X;
            float CurrentY = Y;
            for (int i = 0; i < MaxPoints; i++)
            {
                CurrentX += Settings.RayJumpDistance * (float)Math.Cos(Angle);
                CurrentY += Settings.RayJumpDistance * (float)Math.Sin(Angle);
                CurrentOpacity -= OpacityLoss;

                // Is colliding with square
                if (!CheckRayCollision(CurrentX, CurrentY))
                {

                    if (Settings.RenderFromFunction)
                    {
                        _spritebatch.Draw(Game1.White, new Rectangle((int)CurrentX - Settings.RayPointHalfSize,
                                                           (int)CurrentY - Settings.RayPointHalfSize,
                                                           Settings.RayPointSize, Settings.RayPointSize), Color.White * CurrentOpacity);
                    }
                    else
                    {
                        Points.Add(new RayPoint()
                        {
                            X = (int)CurrentX,
                            Y = (int)CurrentY,
                            Opacity = CurrentOpacity
                        });
                    }

                }
                else
                {
                    return;
                }
            }
        }
        public static void CastRaysFrom(SpriteBatch _spritebatch, int X, int Y, List<RayPoint> Points)
        {
            int RayCount = (int)(360F / Settings.RayAngleJump);

            float CurrentAngle = 0;
            for (int i = 0; i < RayCount; i++)
            {
                CastSingleRay(_spritebatch, X, Y, CurrentAngle * (float)(Math.PI / 180), Points);

                CurrentAngle += Settings.RayAngleJump;
            }
        }
        public static List<RayPoint> GetCastPoints(SpriteBatch _spritebatch, int CentreX, int CentreY)
        {
            List<RayPoint> rayPoints = new List<RayPoint>();

            CastRaysFrom(_spritebatch, CentreX, CentreY, rayPoints);

            return rayPoints;
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
    }

    public class RayPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        
        public float Opacity { get; set; }


        public RayPoint()
        {
            X = 0; 
            Y = 0;
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
    }
}
