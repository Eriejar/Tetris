using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Graph {
        /*
            (row, column)
            [3,1] [3,2] [3,3]
            [2,1] [2,2] [2,3]
            [1,1] [1,2] [1,3]
        */
        int width;
        int height;
        int bufferRows;
        List<GraphLine> graph;

        public Graph(int width = 10, int height = 20, int bufferRows = 3) {
            this.width = width;
            this.height = height;
            this.bufferRows = bufferRows;
            graph = new List<GraphLine>();


        }

        public void Initialize(Vector3 startingPosition, float slotSize) {
            var currentPosition = startingPosition;
            for (int row = 1; row <= height + bufferRows; row++)
            {
                var newGraphLine = new GraphLine(width, row);
                if (row > height) { newGraphLine.IsVisible = false; }
                
                newGraphLine.Initialize(currentPosition, slotSize, Vector3.UnitX * -1);
                graph.Add(newGraphLine);

                currentPosition += new Vector3(0, 0, slotSize);
            }
        }

        public string GetDebugGraph(Type displayType)
        {
            var debugGraph = new List<string>();
            for (int row = height; row > 0; row--)
            {
                var slotsOnLine = new List<string>();
                for (int column = 1; column <= width; column++)
                {
                    string item = " ";
                    if (displayType == typeof(Block)) { item = GetSlot(row, column).HasBlock() ? "B" : "0"; }
                    if (displayType == typeof(Faller)) { item = GetSlot(row, column).HasFallerBlock() ? "F" : "0"; }
                    slotsOnLine.Add(String.Format("[{0}]", item));
                }
                debugGraph.Add(String.Join(" ", slotsOnLine));
            }
            return String.Join("\n", debugGraph);
        }

        public void InsertBlock(Block block, int row, int column)
        {
            GetLine(row).InsertBlock(block, column);
        }

        public void InsertFallerBlock(Block block, int row, int column)
        {
            GetLine(row).InsertFallerBlock(block, column);
        }

        public GraphSlot GetSlot(int row, int column)
        {
            return GetLine(row).GetSlot(column);
        }

        public void Draw(Camera camera)
        {
            foreach (var graphLine in graph)
            {
                graphLine.Draw(camera);
            }
        }

        public void Clear()
        {
            foreach (var graphLine in graph)
            {
                graphLine.DeleteBlocks();
            }
        }

        public GraphLine GetLine(int position)
        {
            return graph[position - 1];
        }

        public void Gravitize()
        {
            // Lines are 
            for (int i = 1; i < height + bufferRows; i++)
            {
                var prev_line = graph[i - 1];
                graph[i].Gravitize(ref prev_line); 
            }
            return;
        }

        public bool BlockBelow(GraphSlot slot)
        {
            if (slot.row - 1 <= 0) { return false; }
            var belowGraphSlot = GetSlot(slot.row - 1, slot.column);
            return (belowGraphSlot.HasBlock() ? true : false);
        }

        public bool GroundBelow(GraphSlot slot)
        {
            if (slot.row - 1 <= 0) { return true; }
            return false;
        }

        public GraphSlot BottomMostOpenSlot(int column)
        {
            for (int i = 1; i < height; i++)
            {
                var slot = GetSlot(i, column);
                if (!slot.HasBlock() && !slot.HasFallerBlock())
                {
                    return slot;
                }
            }

            return null;
        }


    }
}
