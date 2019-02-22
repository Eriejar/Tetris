using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Tetris
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;


        TetrisGameEngine engine;

        VertexPositionTexture[] floorVerts;
        BasicEffect effect;

        Camera camera;

        Vector3 cameraPosition = new Vector3(0, 120, 10);

        double secondTimer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            floorVerts = new VertexPositionTexture[6];
            floorVerts[0].Position = new Vector3(-50, -50, 0);
            floorVerts[1].Position = new Vector3(-50, 50, 0);
            floorVerts[2].Position = new Vector3(50, -50, 0);
            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(50, 50, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            effect = new BasicEffect(graphics.GraphicsDevice);

            var gen = new TetrisGenerator(Content);
            engine = new TetrisGameEngine();
            engine.Initialize(gen);

            camera = new Camera(graphics.GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            secondTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (secondTimer >= 1)
            {
                secondTimer -= 1;
                SecondTimerEvents();
            }


            base.Update(gameTime);
        }

        private void SecondTimerEvents()
        {
            engine.SecondEvents();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // DrawGround();
            
            engine.graph.Draw(camera);

            base.Draw(gameTime);
        }

        void DrawGround()
        {
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                                                           floorVerts,
                                                           0,
                                                           2);

            }

        }

    }


}
