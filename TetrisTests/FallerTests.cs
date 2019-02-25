using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tetris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Tetris.Tests
{
    [TestClass()]
    public class FallerTests
    {
        Graph graph;
        Faller faller;

        [TestInitialize]
        public void Initialize()
        {
            graph = new Graph();
            graph.Initialize(new Vector3(0, 0, 0), 4);
            graph.InsertFallerBlock(new Block(), 4, 3);
            graph.InsertFallerBlock(new Block(), 4, 4);
            graph.InsertFallerBlock(new Block(), 4, 5);
            graph.InsertFallerBlock(new Block(), 4, 6);

            var slots = new List<GraphSlot>();
            slots.Add(graph.GetSlot(4, 3));
            slots.Add(graph.GetSlot(4, 4));
            slots.Add(graph.GetSlot(4, 5));
            slots.Add(graph.GetSlot(4, 6));

            faller = new Faller(ref slots, Faller.States.Up, ref graph);
        }


        [TestMethod()]
        public void Rotate_WhenInitialized_ShouldBeAbleToRotateFaller()
        {
            Assert.IsTrue(faller.Rotate());
        }

        [TestMethod()]
        public void Rotate_WhenInUpState_ShouldRotateFallerToCorrectPosition()
        {
            Console.WriteLine(graph.GetDebugGraph(typeof(Faller)));
            Console.WriteLine();

            faller.Rotate(); // Up (4,3)(4,4)(4,5)(4,6) => Right (5,5)(4,5)(3,5)(2,5)
            Console.WriteLine("Up => Right");
            Console.WriteLine(graph.GetDebugGraph(typeof(Faller)));
            Console.WriteLine();
            for (int i = 2; i <= 5; i++) { Assert.IsTrue(graph.GetSlot(i, 5).HasFallerBlock()); }

            faller.Rotate(); // Right (5,5)(4,5)(3,5)(2,5) => Down (3,3)(3,4)(3,5)(3,6)
            Console.WriteLine("Right => Down");
            Console.WriteLine(graph.GetDebugGraph(typeof(Faller)));
            Console.WriteLine();
            for (int i = 3; i <= 6; i++) { Assert.IsTrue(graph.GetSlot(3, i).HasFallerBlock()); }

            faller.Rotate(); // Down (3,3)(3,4)(3,5)(3,6) => Left (5,4)(4,4)(3,4)(2,4)
            Console.WriteLine("Down => Left");
            Console.WriteLine(graph.GetDebugGraph(typeof(Faller)));
            Console.WriteLine();
            for (int i = 2; i <= 5; i++) { Assert.IsTrue(graph.GetSlot(i, 4).HasFallerBlock()); }

            faller.Rotate(); // Left (5,4)(4,4)(3,4)(2,4) => Up (4,3)(4,4)(4,5)(4,6)
            Console.WriteLine("Left => Up");
            Console.WriteLine(graph.GetDebugGraph(typeof(Faller)));
            Console.WriteLine();
            for (int i = 3; i <= 6; i++) { Assert.IsTrue(graph.GetSlot(4, i).HasFallerBlock()); }
        }

    }
}