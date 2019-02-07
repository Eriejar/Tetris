using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Tetris
{
    public class GraphLine
    {
        List<GraphSlot> line;
        int size;

        public GraphLine(int size)
        {
            line = new List<GraphSlot>();
            this.size = size;
        }

        public void Initialize(Vector3 startingPosition, Vector3 slotSize)
        {
            var currentPosition = startingPosition;
            for (int i = 0; i < size; i++)
            {
                line.Add(new GraphSlot(currentPosition));
                currentPosition += slotSize;
            }
        }

        public GraphSlot GetSlot(int position)
        {
            return line[position - 1];
        }

        public void InsertBlock(Block block, Model model, int position)
        {
            line[position - 1].InsertBlock(block, model);
            
        }

        public void DeleteBlock(int position)
        {
            line[position - 1].DeleteBlock();
        }

        public void DeleteBlocks()
        {
            foreach (var graphSlot in line)
            {
                graphSlot.DeleteBlock();
            }
        }

        public void Draw(Camera camera)
        {
            foreach (var graphSlot in line)
            {
                graphSlot.Draw(camera);
            }
        }

    }
}
