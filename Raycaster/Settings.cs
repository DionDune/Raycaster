using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycaster
{
    internal class Settings
    {
        public Color BackgroundColor = Color.Black;
        public Color RayPointColor = Color.White;

        public const float RayJumpDistance = 3;
        public const float RayAngleJump = 0.5F;
        public const float RayLegnth = 500;

        public const float GameRayJumpDistance = 0.5F;

        public const int RayPointSize = 2;
        public const int RayPointHalfSize = RayPointSize / 2;

        public const bool UseOpacity = true;


        // Rendering directly from function, rather than saving them, reduces CPU strain by about 30%
        // Saving each point may be neccessary for other applications though
        public static bool RenderAllPoints = true;
        public static bool RenderCollisionDistances = true;
        public static bool GameRender = false;
    }
}
