using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Graph
    {
        int width;
        int height;
        List<GraphLine> graph = new List<GraphLine>();

        public Graph(int width = 10, int height = 20)
        {
            this.width = width;
            this.height = height;
        }

        public void Initialize()
        {
        }


    }
}
