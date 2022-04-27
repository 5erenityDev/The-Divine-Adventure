using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using System;

namespace TheDivineAdventure
{
    public class Scene
    {
        ///////////////
        ///VARIABLES///
        ///////////////

        // Essential
        protected GraphicsDeviceManager _graphics;
        protected ContentManager Content;
        protected SpriteBatch _spriteBatch;
        protected Game1 parent;
        protected MouseState mouseState;
        public Random rand;


        public Scene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 game, ContentManager content)
        {
            Content = content;
            _graphics = graph;
            _spriteBatch = sb;
            parent = game;
            rand = new Random();
        }

        public virtual void Initialize()
        {
            // Set screen scale to determine size of UI
            ReloadContent();
            LoadContent();
        }

        public virtual void LoadContent()
        {

        }

        public virtual void ReloadContent()
        {
            parent.ReloadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
