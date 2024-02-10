﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaycastGame
{
    internal class Settings
    {
        public Color GroundColor = new Color(0, 153, 0);
        public Color SkyColor = new Color(26, 117, 255);
        public Color RayPointColor = Color.White;

        public const float CastRayJumpDistance = 0.5F;
        public const int   CastRayCount = 920;
        public const float CastRayWidth = 2;
        public const float CastRayDistanceStart = 5;
        public const float CastRayDistanceRange = 500;
        public const bool CastDistanceShadow = true;
        public const float DistanceShadowMult = 1.4F;

        public static bool RenderAllPoints = true;
        public const int RayPointSize = 2;
        public const int RayPointHalfSize = RayPointSize / 2;
    }
}