using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TheDivineAdventure
{
    class Player
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constant / Readonly
        // Make sure that the role and height have the same index
        // (EX: WARRIOR is at index 0 of ROLES, while WARRIOR_HEIGHT is also at index 0 of HEIGHTS)
        public static readonly string[] ROLES = { "WARRIOR", "ROGUE", "MAGE", "CLERIC" };
        private const int WARRIOR_HEIGHT = 23;
        private const int ROGUE_HEIGHT = 0;
        private const int MAGE_HEIGHT = 0;
        private const int CLERIC_HEIGHT = 13;
        private static readonly int[] HEIGHTS = { WARRIOR_HEIGHT, ROGUE_HEIGHT, MAGE_HEIGHT, CLERIC_HEIGHT };

        // Info
        public string role;
        private int height;

        // Movement
        private Vector3 pos;
        private Vector3 rot;
        private float speed = 1f;


        // Jumping
        private bool jumping = false;
        private bool falling = false;
        private float jumpSpeed = 1f;
        private float maxHeight = 40f;
        private float minHeight;

        // Sound
        private List<SoundEffect> soundEffects;

        // MouseState
        MouseState prevMouseState;
        MouseState curMouseState;



        //Health and secondary stat
        private float health, secondary, secondaryRegenRate;
        private int healthMax, secondaryMax;
        //swaps stamina for mana when true
        private bool isCaster;

        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Player(List<SoundEffect> s, string r)
        {
            soundEffects = s;
            role = r;
            height = HEIGHTS[Array.IndexOf(ROLES, role)];
            pos = new Vector3(0, height, 0);
            rot = new Vector3(0, 0, 0);
            minHeight = pos.Y;
            prevMouseState = Mouse.GetState();
            pos = new Vector3(0, CLERIC_HEIGHT, 0);
            minHeight = CLERIC_HEIGHT;

            //set health and secondary bar (will later be set by character, or perhaps altered by powerups)
            healthMax = 100;
            health = 100;
            secondaryMax = 100;
            secondary = 100;
            if(role.Equals("CLERIC") || role.Equals("MAGE"))
                isCaster = true;
        }



        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime)
        {
            // Regular Gameplay
            Move(gameTime);
            Shoot(gameTime);

            // Debugging
            //NoClip(gameTime);
            //SwitchRole();
        }

        private void Move(GameTime gameTime)
        {
            // Move forward
            // FOR NOW the player stops at z = 2200 as the current test stage ends there.
            if (pos.Z < 2200)
                pos.Z++;
            else
                pos.Z = 2200;

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
            {
                jumping = true;
            }

            // Calculate jump
            if (jumping)
                Jump(gameTime);

            //regen Stamina
            if (secondary <= secondaryMax)
            {
                secondary += secondaryRegenRate;
            }
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

        private void Shoot(GameTime gameTime)
        {
            curMouseState = Mouse.GetState();

            if (curMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed)
            {
                Debug.WriteLine("Shoot Projectile");
            }

            prevMouseState = curMouseState;
        }



        /////////////////////////
        ///DEBUGGING FUNCTIONS///
        /////////////////////////
        private void NoClip(GameTime gameTime)
        {
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

            // Rotate player left and right
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                this.rot += new Vector3(0, 1, 0) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.E))
                this.rot -= new Vector3(0, 1, 0) * speed;

            // Float Up and Down
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                this.pos += new Vector3(0, 1, 0) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                this.pos -= new Vector3(0, 1, 0) * speed;

            // Test the damage mechanic
            if (Keyboard.GetState().IsKeyDown(Keys.U))
                health -= 3;
            // test the secondary resource mechanic
            if (Keyboard.GetState().IsKeyDown(Keys.P))
                secondary -= 3;

            speed = 1f;
        }

        //update health and stamina bar
        public Rectangle resourceBarUpdate(bool isHealth, Rectangle bar, Vector2 screen, float scale)
        {
            int nWid;
            Rectangle newRect;

            //get length of bar proportional to desired bar and current screen width
            if (isHealth)
            {
                //set width of bar based on current stat
                nWid = (int)System.Math.Round(health / healthMax * (0.202f * screen.X));
                //define new rectangle for bar
                newRect = new Rectangle(
                    (int)System.Math.Round(0.099f * screen.X),
                    (int)System.Math.Round(0.044f * screen.Y), nWid, (int)System.Math.Round(bar.Height * scale));
            }
            else
            {
                //set width of bar based on current stat
                nWid = (int)System.Math.Round(secondary / secondaryMax * (0.202f * screen.X));
                //define new rectangle for bar
                newRect = new Rectangle(
                    (int)System.Math.Round(0.088f * screen.X),
                    (int)System.Math.Round(0.099f * screen.Y), nWid, (int)System.Math.Round(bar.Height * scale));
            }

            return newRect;
        }

        private void SwitchRole()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                role = ROLES[0];
                height = HEIGHTS[0];
                minHeight = height;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                role = ROLES[1];
                height = HEIGHTS[1];
                minHeight = height;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                role = ROLES[2];
                height = HEIGHTS[2];
                minHeight = height;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                role = ROLES[3];
                height = HEIGHTS[3];
                minHeight = height;
            }
        }



        ////////////////////
        ///GETTER/SETTERS///
        ////////////////////
        public Vector3 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public Vector3 Rot
        {
            get { return rot; }
            set { rot = value; }
        }

        public bool IsCaster
        {
            get { return isCaster; }
            set { isCaster = value; }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }
    }
}
