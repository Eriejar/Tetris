using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Tetris
{
    public class GraphSlot
    {
        Block block = null;
        Block fallerBlock = null;
        public Vector3 position;
        public int row;
        public int column;
        public bool IsEmpty { get => (block == null) ? true : false; }

        public GraphSlot(Vector3 position, int row, int column)
        {
            this.block = null;
            this.position = position;
            this.row = row;
            this.column = column;
        }

        public void InsertBlock(Block block)
        {
            if (fallerBlock != null)
                throw new InvalidOperationException("Faller block present inside GraphSlot");
            this.block = block;
        }

        public void InsertFallerBlock(Block block)
        {
            if (this.block != null)
                throw new InvalidOperationException("Regular block present inside slot");

            fallerBlock = block;
        }

        public void DeleteBlock()
        {
            block = null;
        }

        public Block PopBlock()
        {
            var poppedBlock = block;
            block = null;
            return poppedBlock;
        }

        public Block PopFallerBlock()
        {
            var poppedBlock = fallerBlock;
            fallerBlock = null;
            return poppedBlock;
        }

        public void MoveBlockTo(GraphSlot slot)
        {
            slot.InsertBlock(block);
            DeleteBlock();
        }

        public void Draw(Camera camera)
        {
            if (HasBlock())
            {
                block.Draw(camera, GetWorldMatrix());
            }
            if (HasFallerBlock())
            {
                fallerBlock.Draw(camera, GetWorldMatrix());
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

        public bool HasFallerBlock()
        {
            if (fallerBlock != null)
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
