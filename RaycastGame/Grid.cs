using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaycastGame
{
    internal class Grid
    {
        public static void GenerateBase(List<List<Color?>> Grid, int Width, int Height)
        {
            for (int y = 0; y < Height; y++)
            {
                Grid.Add(new List<Color?>());
                for (int x = 0; x < Width; x++)
                {
                    Grid[y].Add(null);
                }
            }
        }
        public static void GenerateMaze(List<List<Color?>> Grid, int CubeSize, Color? Color, bool RandomColors)
        {
            CreateGridSquare(Grid, 3, 1, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 4, 1, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 5, 1, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 6, 1, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 2, 2, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 2, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 2, 3, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 4, 3, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 5, 3, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 3, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 1, 4, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 4, 4, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 5, 4, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 4, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 8, 4, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 9, 4, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 1, 5, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 4, 5, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 5, 5, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 10, 5, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 1, 6, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 3, 6, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 4, 6, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 6, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 8, 6, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 10, 6, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 1, 7, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 3, 7, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 6, 7, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 7, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 8, 7, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 10, 7, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 1, 8, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 3, 8, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 5, 8, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 6, 8, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 8, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 8, 8, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 11, 8, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 1, 9, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 3, 9, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 5, 9, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 6, 9, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 9, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 8, 9, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 9, 9, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 11, 9, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 1, 10, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 5, 10, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 6, 10, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 7, 10, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 8, 10, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 9, 10, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 11, 10, CubeSize, Color, RandomColors);

            CreateGridSquare(Grid, 2, 11, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 3, 11, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 4, 11, CubeSize, Color, RandomColors);
            CreateGridSquare(Grid, 10, 11, CubeSize, Color, RandomColors);

            Grid[34][64] = new Color(252, 186, 3);
        }

        public static void CreateGridSquare(List<List<Color?>> Grid, int x, int y, int Size, Color? Color, bool RandomColors)
        {
            int StartX = x * Size;
            int StartY = y * Size;

            for (int i = 0; i < Size; i++)
            {
                for (int X = 0; X < Size; X++)
                {
                    if (RandomColors)
                    {
                        Color = new Color(Game1.random.Next(0, 255), Game1.random.Next(0, 255), Game1.random.Next(0, 255));
                    }
                    Grid[StartY + i][StartX + X] = Color;
                }
            }
        }
    }
}
