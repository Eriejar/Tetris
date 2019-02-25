using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Tetris
{
    public class TetrisGameEngine
    {
        public Graph graph;
        Faller faller;
        TetrisGenerator tetGen;

        public void Initialize(TetrisGenerator tetrisGenerator)
        {
            tetGen = tetrisGenerator;

            graph = new Graph();
            graph.Initialize(new Vector3(0, 0, -16), 4);

            faller = SampleFaller();
        }

        public void SecondEvents()
        {
            graph.Gravitize();
            FallerAction();
            faller.Rotate();
        }

        private void SampleBlocks()
        {
            graph.InsertBlock(tetGen.CreateRedBlock(), 6, 1);
            graph.InsertBlock(tetGen.CreateRedBlock(), 5, 1);
            graph.InsertBlock(tetGen.CreateRedBlock(), 8, 1);
            graph.InsertBlock(tetGen.CreateRedBlock(), 1, 2);
            graph.InsertBlock(tetGen.CreateRedBlock(), 2, 2);
        }

        private Faller SampleFaller()
        {
            graph.InsertFallerBlock(tetGen.CreateRedBlock(), 8, 3);
            graph.InsertFallerBlock(tetGen.CreateRedBlock(), 8, 4);
            graph.InsertFallerBlock(tetGen.CreateRedBlock(), 8, 5);
            graph.InsertFallerBlock(tetGen.CreateRedBlock(), 8, 6);

            var slots = new List<GraphSlot>();
            slots.Add(graph.GetSlot(8, 3));
            slots.Add(graph.GetSlot(8, 4));
            slots.Add(graph.GetSlot(8, 5));
            slots.Add(graph.GetSlot(8, 6));

            var faller = new Faller(ref slots, Faller.States.Up, ref graph);
            return faller;
        }

    
        private void FallerAction()
        {
            // Scenarios:
            //  1) block below faller
            //      a) if landed => freezes faller
            //      b) else => sets Landed to true
            //  2) block not below faller
            //      a) sets Landed to false
            //      b) drops faller

            if (faller.ObstacleBelowFaller())
            {
                if (faller.Landed == true) { FreezeFaller(); }
                else { faller.Landed = true; }
            }
            else
            {
                if (faller.Landed == true)
                {
                    faller.Landed = false;                  
                }
                faller.Drop();
            }
        } 

        private void FreezeFaller()
        {
            faller.Freeze();
            faller = SampleFaller();
        }
    }
}
