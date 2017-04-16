using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerDefense
{
    public class TowerDefense : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch batch;
        InputHandler mouse;
        Viewport viewport;

        GameHUD gameHUD;
        GameEngine gameEngine;
        GameMap gameMap;

        public TowerDefense()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Constants.GameSize.X;
            graphics.PreferredBackBufferHeight = Constants.GameSize.Y;
        }


        protected override void Initialize()
        {
            viewport = graphics.GraphicsDevice.Viewport;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            ResourceManager.InitializeTextures(Content);
            float scaleX = (float)viewport.Width / Constants.GameSize.X;
            float scaleY = (float)viewport.Height / Constants.GameSize.Y;
            mouse = new InputHandler(new Vector2(scaleX, scaleY));
            gameHUD = new GameHUD();
            gameEngine = new GameEngine();
            gameMap = new GameMap();
        }

        protected override void Update(GameTime gameTime)
        {
            mouse.Update(gameTime, gameEngine, gameMap);
            gameMap.Update(gameTime, mouse);
            gameHUD.Update(gameTime, mouse);
            gameEngine.Update(gameTime, mouse);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            float scaleX = (float)viewport.Width / Constants.GameSize.X;
            float scaleY = (float)viewport.Height / Constants.GameSize.Y;
            Matrix scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            batch.Begin(transformMatrix: scaleMatrix);

            gameMap.Draw(batch);
            gameEngine.Draw(batch);
            gameHUD.Draw(batch);
            mouse.Draw(batch);

            batch.End();

            base.Draw(gameTime);
        }
    }
}
