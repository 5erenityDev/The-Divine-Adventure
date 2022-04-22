using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TheDivineAdventure
{
    public class Scene
    {
        ///////////////
        ///VARIABLES///
        ///////////////

        // Essential
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;
        protected Game1 parent;
        protected MouseState mouseState;
        public Random rand;


        public Scene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 game)
        {
            _graphics = graph;
            _spriteBatch = sb;
            parent = game;
            rand = new Random();
        }

        public virtual void Initialize()
        {
            // Set screen scale to determine size of UI
            ReloadContent();
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
