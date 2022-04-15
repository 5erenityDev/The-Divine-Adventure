using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    class Player
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constants
        private const int clericHeight = 13;

        // Movement
        private Vector3 pos;
        private float speed = 1f;
        

        //jumping
        private bool jumping = false;
        private bool falling = false;
        private float jumpSpeed = 1f;
        private float maxHeight = 40f;
        private float minHeight;

        // Sound
        private List<SoundEffect> soundEffects;

        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Player(List<SoundEffect> s)
        {
            soundEffects = s;
            pos = new Vector3(0, clericHeight, 0);
            minHeight = clericHeight;
        }

        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime)
        {
            // Comment out whichever one you DON'T want to do
            NoClip(gameTime);
            //Move(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            // Move forward
            pos.Z++;

            // Move left
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
                if (pos.X <= 95)
                    pos.X++;

            // Move right
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
                if (pos.X >= -95)
                    pos.X--;

            // Initiate jump
            if (!jumping && Keyboard.GetState().IsKeyDown(Keys.Space))
                jumping = true;


            // Calculate jump
            if (jumping)
                Jump(gameTime);
        }

        private void Jump(GameTime gameTime)
        {
            // Determine if player is still going up, if so, increase jump height
            if (Pos.Y <= maxHeight && !falling)
            {
                pos.Y += jumpSpeed;
            }
            // Determine if player is now going down
            else if (Pos.Y > maxHeight && !falling)
            {
                falling = true;
            }
            // Decrease jump height
            else if (falling && pos.Y > minHeight)
                pos.Y -= jumpSpeed;
            // Once on the ground, stop jumping
            else if (pos.Y <= minHeight)
            {
                pos.Y = minHeight;
                falling = false;
                jumping = false;
            }
        }

        private void NoClip(GameTime gameTime)
        {
            // FOR DEBUG PURPOSES
            // Move Faster
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                speed = 3f;

            //Move left, right, forward, and back
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
                this.pos += new Vector3(1, 0, 0) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
                this.pos -= new Vector3(1, 0, 0) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
                this.pos += new Vector3(0, 0, 1) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                this.pos -= new Vector3(0, 0, 1) * speed;

            // Float Up and Down
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                this.pos += new Vector3(0, 1, 0) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                this.pos -= new Vector3(0, 1, 0) * speed;

            speed = 1f;
        }

        ////////////////////
        ///GETTER/SETTERS///
        ////////////////////
        public Vector3 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
    }
}
