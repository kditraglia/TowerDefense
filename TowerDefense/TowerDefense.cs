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
        MouseHandler mouse;
        Viewport viewport;

        GameHUD gameHUD;
        GameEngine gameEngine;
        GameMap gameMap;

        public Vector2 VirtualSize { get; set; }

        public TowerDefense()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            VirtualSize = new Vector2(1020, 800);

            graphics.PreferredBackBufferWidth = 1020;
            graphics.PreferredBackBufferHeight = 800;
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

            mouse = new MouseHandler(Point.Zero, ResourceManager.DefaultCursor);
            gameHUD = new GameHUD(viewport, mouse);
            gameEngine = new GameEngine();
            gameMap = new GameMap();
        }

        protected override void Update(GameTime gameTime)
        {
            mouse.Update(gameTime, gameEngine, gameMap);
            gameMap.Update(gameTime);
            gameHUD.Update(gameTime);
            gameMap.HandleMouseHover(mouse);
            gameHUD.HandleMouseHover(mouse);
            gameEngine.HandleMouseHover(mouse);

            gameEngine.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            var scaleX = viewport.Width / VirtualSize.X;
            var scaleY = viewport.Height / VirtualSize.Y;
            var scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

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
