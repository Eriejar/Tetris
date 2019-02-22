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
        List<GraphSlot> slots;
        int size;
        
        int row;

        public bool IsVisible;
        public bool IsEmpty { get => slots.TrueForAll(x => x.IsEmpty); }

        public GraphLine(int size, int row, bool visible = true)
        {
            slots = new List<GraphSlot>();
            this.size = size;
            this.IsVisible = visible;
            this.row = row;
        }

        public void Initialize(Vector3 startingPosition, float slotSize, Vector3 axis)
        {
            var currentPosition = startingPosition;
            for (int column = 1; column <= size; column++)
            {
                slots.Add(new GraphSlot(currentPosition, row, column));
                currentPosition += slotSize * axis;
            }
        }

        public GraphSlot GetSlot(int position)
        {
            return slots[position - 1];
        }

        public void InsertBlock(Block block, int position)
        {
            slots[position - 1].InsertBlock(block);            
        }

        public void InsertFallerBlock(Block block, int position)
        {
            slots[position - 1].InsertFallerBlock(block);
        }

        public void MoveBlocksTo(ref GraphLine line)
        {
            if (size != line.size) { throw new ArgumentException("Lines of not same size"); }
            for (int i = 0; i < size; i++)
            {
                if (slots[i].HasBlock()) { slots[i].MoveBlockTo(line.GetSlot(i + 1)); }
            }
        }

        public void DeleteBlock(int position)
        {
            slots[position - 1].DeleteBlock();
        }

        public void DeleteBlocks()
        {
            foreach (var graphSlot in slots)
            {
                graphSlot.DeleteBlock();
            }
        }

        public void Draw(Camera camera)
        {
            foreach (var graphSlot in slots)
            {
                graphSlot.Draw(camera);
            }
        }

        public bool AllFilled()
        {
            foreach (var graphSlot in slots)
            {
                if (!graphSlot.HasBlock())
                {
                    return false;
                }
            }
            return true;
        }

        public bool Gravitize(ref GraphLine lineBelow)
        {
            bool gravitized = false;
            if (lineBelow.IsEmpty == true)
            {
                MoveBlocksTo(ref lineBelow);
                DeleteBlocks();
                gravitized = true;
            }
            return gravitized;
        }

    }
}
