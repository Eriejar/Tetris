using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Tetris
{
    public class TetrisGenerator
    {

        ContentManager content;

        public TetrisGenerator(ContentManager contentManager)
        {
            content = contentManager;
        }

        public Block CreateRedBlock()
        {
            var model = content.Load<Model>("Red_block");
            var newBlock = new Block();
            newBlock.Initialize(model);
            return newBlock;
        }
    }
}
