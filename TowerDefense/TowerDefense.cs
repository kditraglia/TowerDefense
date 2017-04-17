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
        Vector2 gameScale;
        GraphicsDeviceManager graphics;
        SpriteBatch batch;

        InputHandler inputHandler;
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            ResourceManager.InitializeTextures(Content);

            inputHandler = new InputHandler();
            gameEngine = new GameEngine();
            gameMap = new GameMap();
            gameHUD = new GameHUD(() => gameEngine.StartLevel(gameMap.GetBestPath()));
        }

        protected override void Update(GameTime gameTime)
        {
            gameScale = new Vector2((float)graphics.GraphicsDevice.Viewport.Width / Constants.GameSize.X, (float)graphics.GraphicsDevice.Viewport.Height / Constants.GameSize.Y);
            inputHandler.Update(gameTime, gameScale);
            gameMap.Update(gameTime, inputHandler);
            gameHUD.Update(gameTime, inputHandler);
            gameEngine.Update(gameTime, inputHandler);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            batch.Begin(transformMatrix: Matrix.CreateScale(new Vector3(gameScale, 1.0f)));

            inputHandler.Draw(batch);
            gameMap.Draw(batch);
            gameEngine.Draw(batch);
            gameHUD.Draw(batch);

            batch.End();

            base.Draw(gameTime);
        }
    }
}
