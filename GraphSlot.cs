using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Tetris
{
    public class GraphSlot
    {
        Block block;
        Vector3 position;

        public GraphSlot(Vector3 position)
        {
            this.block = null;
            this.position = position;
        }

        public void InsertBlock(Block block, Model model)
        {
            this.block = block;
            block.Initialize(model);
        }

        public void DeleteBlock()
        {
            block = null;
        }

        public void MoveBlockTo(GraphSlot slot)
        {
            slot.block = block;
            DeleteBlock();
        }

        public void Draw(Camera camera)
        {
            if (HasBlock())
            {
                block.Draw(camera, GetWorldMatrix());
            }
        }

        public bool HasBlock()
        {
            if (block != null)
            {
                return true;
            }
            return false;
        }

        Matrix GetWorldMatrix()
        {
            Matrix translationMatrix = Matrix.CreateTranslation(position);
            return translationMatrix;
        }
    }
}
