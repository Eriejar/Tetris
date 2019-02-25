using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Tetris {
    public class Faller {
        // l faller implementation
        public enum States { Up, Right, Down, Left };
        public bool Landed = false;
        public bool Frozen = false;

        List<GraphSlot> slots;
        Graph graph;
        States state;
        List<Block> tempBlocks = new List<Block>();

        int[,] rightDestMatrix = new int[,] {   { 0, 0, 1, 0 },                                                       
                                                { 0, 0, 1, 0 },
                                                { 0, 0, 1, 0 },
                                                { 0, 0, 1, 0 } };

        int[,] downDestMatrix = new int[,] {    { 0, 0, 0, 0 },
                                                { 0, 0, 0, 0 },
                                                { 1, 1, 1, 1 },
                                                { 0, 0, 0, 0 } };

        int[,] leftDestMatrix = new int[,] {    { 0, 1, 0, 0 },
                                                { 0, 1, 0, 0 },
                                                { 0, 1, 0, 0 },
                                                { 0, 1, 0, 0 } };

        int[,] upDestMatrix = new int[,] {  { 0, 0, 0, 0 },
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

        public bool Rotate()
        {
            bool rotated = false;
            var topLeftMatrixCoordinate = GetMatrixBoxCoordinate();
            
            if (state == States.Up)
            {
                rotated = Rotate(rightDestMatrix, topLeftMatrixCoordinate, States.Right);
            }
            else if (state == States.Right)
            {
                rotated = Rotate(downDestMatrix, topLeftMatrixCoordinate, States.Down);
            }
            else if (state == States.Down)
            {
                rotated = Rotate(leftDestMatrix, topLeftMatrixCoordinate, States.Left);
            }
            else if (state == States.Left)
            {
                rotated = Rotate(upDestMatrix, topLeftMatrixCoordinate, States.Up);
            }

            return rotated;
            
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

        private bool Rotate(int[,] destMatrix, Tuple<int,int> matrixTopLeftCoordinate , States endState)
        {
            List<GraphSlot> destSlots = GetDestSlots(destMatrix, matrixTopLeftCoordinate);
            if (destSlots == null) { return false; }

            if (AllSlotsOpen(ref destSlots))
            {
                Map(ref slots, ref destSlots);
                slots = destSlots;
                state = endState;
                return true;
            }
            return false;
        }

       

        private List<GraphSlot> GetDestSlots(int[,] destMatrix, Tuple<int,int> matrixTopLeftCoordinate)
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
                            destSlots.Add(graph.GetSlot(matrixTopLeftCoordinate.Item1 - i, matrixTopLeftCoordinate.Item2 + j));
                        }
                        catch (ArgumentOutOfRangeException) // Handles out of graph scenarios
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
            // Maps blocks from orig to dest, emptying orig blocks. (DOES NOT AFFECT graph instance slots)

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

        private Tuple<int, int> GetMatrixBoxCoordinate()
        {
            int? row = null;
            int? col = null;
            var topLeftSlot = slots.OrderByDescending(x => x.row).ThenBy(x => x.column).First();
            if (state == States.Up)
            {
                row = topLeftSlot.row + 1;
                col = topLeftSlot.column;
            }
            else if (state == States.Right)
            {
                row = topLeftSlot.row;
                col = topLeftSlot.column - 2;
            }
            else if (state == States.Down)
            {
                row = topLeftSlot.row + 2;
                col = topLeftSlot.column;
            }
            else if (state == States.Left)
            {
                row = topLeftSlot.row;
                col = topLeftSlot.column - 1;
            }

            var coordinate = new Tuple<int, int>((int)row, (int)col);
            return coordinate;            
        }


    }
}
