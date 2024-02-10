using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaycastGame
{
    internal class Player
    {
        public const float Speed = 2F;
        public const float RoationSpeed = 1.2F;

        public float X { get; set; }
        public float Y { get; set; }

        public float Rotation { get; set; }

        public Player()
        {
            X = 0;
            Y = 0;
            Rotation = 0;
        }
    }
}
