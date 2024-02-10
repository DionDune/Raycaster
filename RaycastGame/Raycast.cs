using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaycastGame
{
    internal class Raycast
    {
        public static float GetDistanceBetween(Vector2 Point1, Vector2 Point2)
        {
            float XDiff = Math.Abs(Point1.X - Point2.X);
            float YDiff = Math.Abs(Point1.Y - Point2.Y);
            float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

            return Distance;
        }
        public static Color? GetRayCollisionType(float X, float Y)
        {
            int XGridPos = (int)(X / Game1.GridScreenDivisor);
            int YGridPos = (int)(Y / Game1.GridScreenDivisor);

            if (XGridPos < 5 || YGridPos < 5 || XGridPos > Game1.GameGrid[0].Count - 5 || YGridPos > Game1.GameGrid.Count - 5)
            {
                return null;
            }

            return Game1.GameGrid[YGridPos][XGridPos];
        }

        public static void CastRays(SpriteBatch _spritebatch, float X, float Y, float Rotation)
        {
            int RayCount = Settings.CastRayCount;
            float RayAngleJump = 120F / (float)RayCount; // This each pixel

            float CurrentAngle = 0;
            for (int i = 0; i < RayCount; i++)
            {
                CastSingleRay(_spritebatch, X, Y, Settings.CastRayDistanceStart, Settings.CastRayDistanceRange, 
                                                        (Rotation + CurrentAngle) * (float)(Math.PI / 180), (int)(i * Settings.CastRayWidth));

                CurrentAngle += RayAngleJump;
            }
        }
        private static void CastSingleRay(SpriteBatch _spritebatch, float OrigX, float OrigY, float DistanceFromOrig, float Length, float Angle, int ScreenDistance)
        {
            int MaxPoints = (int)(Length / Settings.CastRayJumpDistance);
            float PointsBeforeCheck = DistanceFromOrig / Settings.CastRayJumpDistance;
            float OpacityLoss = 1.2F / MaxPoints;

            float CurrentX = OrigX + (DistanceFromOrig * (float)Math.Cos(Angle));
            float CurrentY = OrigY + (DistanceFromOrig * (float)Math.Sin(Angle));
            for (int i = 0; i < MaxPoints; i++)
            {
                CurrentX += Settings.CastRayJumpDistance * (float)Math.Cos(Angle);
                CurrentY += Settings.CastRayJumpDistance * (float)Math.Sin(Angle);


                Color? CollionType = GetRayCollisionType(CurrentX, CurrentY);
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
                    int CubeHeight = (int)(180F / (GetDistanceBetween(new Vector2(OrigX, OrigY), new Vector2(CurrentX, CurrentY)) / 100));


                    _spritebatch.Draw(Game1.White, new Rectangle(ScreenDistance, 540 - (CubeHeight / 2), (int)Settings.CastRayWidth, CubeHeight), (Color)CollionType);
                    if (Settings.CastDistanceShadow)
                    {
                        _spritebatch.Draw(Game1.White, new Rectangle(ScreenDistance, 540 - (CubeHeight / 2), (int)Settings.CastRayWidth, CubeHeight), Color.Black * ((1 - (1 - (i * OpacityLoss))) * Settings.DistanceShadowMult) );
                    }


                    return;
                }
            }
        }
    }
}
