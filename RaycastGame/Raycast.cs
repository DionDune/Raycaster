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

        public static void CastRays(SpriteBatch _spritebatch, float X, float Y, float Rotation, Settings Settings)
        {
            // Sprite/Player distance, Screen position, Render Size, Opacity
            List<(bool IsSprite, float, Vector2, Vector2, Color, float)> SpritesToRender = new List<(bool, float, Vector2, Vector2, Color, float)>();

            int RayCount = Settings.CastRayCount;
            float RayAngleJump = 120F / (float)RayCount; // This each pixel

            float CurrentAngle = 0;
            for (int i = 0; i < RayCount; i++)
            {
                CastSingleRay(_spritebatch, X, Y, Settings.CastRayDistanceStart, Settings.CastRayDistanceRange, 
                                                        (Rotation + CurrentAngle) * (float)(Math.PI / 180), (int)(i * Settings.CastRayWidth), SpritesToRender, Settings);

                CurrentAngle += RayAngleJump;
            }

            foreach((bool, float, Vector2, Vector2, Color, float) Sprite in SpritesToRender)
            {
                if (Sprite.Item1)
                {
                    _spritebatch.Draw(Game1.Tree, new Rectangle((int)Sprite.Item3.X, (int)Sprite.Item3.Y, (int)Sprite.Item4.X, (int)Sprite.Item4.Y), Sprite.Item5);

                    if (Settings.CastDistanceShadow)
                    {
                        _spritebatch.Draw(Game1.Tree, new Rectangle((int)Sprite.Item3.X, (int)Sprite.Item3.Y, (int)Sprite.Item4.X, (int)Sprite.Item4.Y), Color.Black * Sprite.Item6);
                    }
                }
                else
                {
                    _spritebatch.Draw(Game1.White, new Rectangle((int)Sprite.Item3.X, (int)Sprite.Item3.Y, (int)Sprite.Item4.X, (int)Sprite.Item4.Y), Sprite.Item5);

                    if (Settings.CastDistanceShadow)
                    {
                        _spritebatch.Draw(Game1.White, new Rectangle((int)Sprite.Item3.X, (int)Sprite.Item3.Y, (int)Sprite.Item4.X, (int)Sprite.Item4.Y), Color.Black * Sprite.Item6);
                    }
                }
            }

            Game1.RenderedSpritePositions.Clear();
        }
        private static void CastSingleRay(SpriteBatch _spritebatch, float OrigX, float OrigY, float DistanceFromOrig, float Length, float Angle, int ScreenDistance, List<(bool, float, Vector2, Vector2, Color, float)> SpritesToRender, Settings Settings)
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



                    if (CollionType == Game1.TreeCol)
                    {
                        Point GridPos = new Point((int)(CurrentX / Game1.GridScreenDivisor), (int)(CurrentY / Game1.GridScreenDivisor));

                        if (!Game1.RenderedSpritePositions.Contains(GridPos) && SpritesToRender != null)
                        {
                            //Sorting sprites in order of distance from camera
                            float SpritePlayerDistance = GetDistanceBetween(new Vector2(OrigX, OrigY), new Vector2(CurrentX, CurrentY));

                            (bool ,float, Vector2, Vector2, Color, float) Sprite = (true, SpritePlayerDistance,
                                                                    new Vector2(ScreenDistance - CubeHeight, 540 - (int)(CubeHeight * 1.5F)),
                                                                    new Vector2((int)(CubeHeight * 2F), (int)(CubeHeight * 2F)),
                                                                    Color.White,
                                                                    (1 - (1 - (i * OpacityLoss))) * Settings.DistanceShadowMult);

                            if (SpritesToRender.Count > 0)
                            {
                                for (int x = 0; x < SpritesToRender.Count; x++)
                                {
                                    if (SpritePlayerDistance > SpritesToRender[x].Item2 || x == SpritesToRender.Count - 1)
                                    {
                                        SpritesToRender.Insert(x, Sprite);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                SpritesToRender.Add(Sprite);
                            }
                            
                            
                            Game1.RenderedSpritePositions.Add(GridPos);
                        }
                    }
                    else 
                    {
                        float SpritePlayerDistance = GetDistanceBetween(new Vector2(OrigX, OrigY), new Vector2(CurrentX, CurrentY));

                        (bool, float, Vector2, Vector2, Color, float) WallPeice = (false, SpritePlayerDistance,
                                                                            new Vector2(ScreenDistance, 540 - (CubeHeight / 2)),
                                                                            new Vector2((int)Settings.CastRayWidth, CubeHeight),
                                                                            (Color)CollionType,
                                                                            ((1 - (1 - (i * OpacityLoss))) * Settings.DistanceShadowMult));

                        if (SpritesToRender.Count > 0)
                        {
                            for (int x = 0; x < SpritesToRender.Count; x++)
                            {
                                if (SpritePlayerDistance > SpritesToRender[x].Item2 || x == SpritesToRender.Count - 1)
                                {
                                    SpritesToRender.Insert(x, WallPeice);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            SpritesToRender.Add(WallPeice);
                        }



                        return;
                    }
                    
                }
            }
        }
    }
}
