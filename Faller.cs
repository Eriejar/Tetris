using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Tetris {
    public class Faller {
        // l faller implementation
        public enum States { Up, Right, Down, Left };

        List<GraphSlot> slots;
        Graph graph;
        States state;
        List<Block> tempBlocks = new List<Block>();
        public bool Landed = false;
        public bool Frozen = false;

        private int[,] rightDestMatrix = new int[,] {   { 0, 0, 1, 0 },                                                       
                                                        { 0, 0, 1, 0 },
                                                        { 0, 0, 1, 0 },
                                                        { 0, 0, 1, 0 } };

        private int[,] downDestMatrix = new int[,] {    { 0, 0, 0, 0 },
                                                        { 0, 0, 0, 0 },
                                                        { 1, 1, 1, 1 },
                                                        { 0, 0, 0, 0 } };

        private int[,] leftDestMatrix = new int[,] {    { 0, 1, 0, 0 },
                                                        { 0, 1, 0, 0 },
                                                        { 0, 1, 0, 0 },
                                                        { 0, 1, 0, 0 } };

        private int[,] upDestMatrix = new int[,] {      { 0, 0, 0, 0 },
                                                        { 1, 1, 1, 1 },
                                                        { 0, 0, 0, 0 },
                                                        { 0, 0, 0, 0 } };

        public Faller(ref List<GraphSlot> slots, States state, ref Graph graph)
        {
            this.slots = slots;
            this.graph = graph;
            this.state = state;
        }

        public void Drop()
        {
            if (ObstacleBelowFaller())
                return;

            var destSlots = new List<GraphSlot>();
            foreach (var slot in slots)
            {
                destSlots.Add(graph.GetSlot(slot.row - 1, slot.column));

            }
            Map(ref slots, ref destSlots);
            slots = destSlots;
        }

        public void Rotate()
        { 
            if (state == States.Up)
            {
                Rotate(rightDestMatrix, slots[0].row + 1, slots[0].column, States.Right);
            }
            else if (state == States.Right)
            {
                Rotate(downDestMatrix, slots[0].row, slots[0].column - 2, States.Down);
            }
            else if (state == States.Down)
            {
                Rotate(leftDestMatrix, slots[0].row + 2, slots[0].column, States.Left);
            }
            else if (state == States.Left)
            {
                Rotate(upDestMatrix, slots[0].row, slots[0].column - 1, States.Up);
            }
            
        }

        public void Freeze() {
            foreach (var slot in slots)
            {
                var temp = slot.PopFallerBlock();
                slot.InsertBlock(temp);
            }
            Frozen = true;
        }


        public bool ObstacleBelowFaller()
        {
            // Checks if a block or the ground is below faller
            int bottomRow = slots.Min(x => x.row);
            List<GraphSlot> bottomSlots = slots.FindAll(x => x.row == bottomRow);

            foreach (var slot in bottomSlots)
            {
                if (graph.BlockBelow(slot) || graph.GroundBelow(slot))
                    return true;
            }

            return false;
        }

        private bool Rotate(int[,] destMatrix, int topLeftCornerRow, int topLeftCornerColumn, States endState)
        {
            List<GraphSlot> destSlots = GetDestSlots(rightDestMatrix, topLeftCornerRow, topLeftCornerColumn);
            if (AllSlotsOpen(ref destSlots))
            {
                Map(ref slots, ref destSlots);
                slots = destSlots;
                state = endState;
                return true;
            }
            return false;
        }

       

        private List<GraphSlot> GetDestSlots(int[,] destMatrix, int rowTopLeft, int columnTopLeft)
        {
            var destSlots = new List<GraphSlot>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (destMatrix[i, j] == 1)
                    {
                        try
                        {
                            destSlots.Add(graph.GetSlot(rowTopLeft - i, columnTopLeft + j));
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return null;
                        }
                    }                  
                }
            }
            return destSlots;
        }

        private bool AllSlotsOpen(ref List<GraphSlot> slots)
        {
            foreach(var slot in slots)
            {
                if (slot.HasBlock())
                {
                    return false;
                }
            }
            return true;
        }

        private void Map(ref List<GraphSlot> orig, ref List<GraphSlot> dest)
        {
            // Maps blocks from orig to dest, emptying orig blocks. (DOES NOT AFFECT instances slots)

            if (orig.Count != dest.Count) { throw new ArgumentException("orig and dest are not the same size"); }

            for (int i = 0; i < orig.Count; i++)
            {
                tempBlocks.Add(orig[i].PopFallerBlock());
            }

            for (int i = 0; i < dest.Count; i++)
            {
                dest[i].InsertFallerBlock(tempBlocks[i]);
            }

            tempBlocks.Clear();
        }




    }
}
