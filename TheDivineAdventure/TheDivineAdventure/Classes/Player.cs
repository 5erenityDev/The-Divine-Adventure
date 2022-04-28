using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
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
        private const int WARRIOR_WIDTH = 0;
        private const int ROGUE_WIDTH = 0;
        private const int MAGE_WIDTH = 0;
        private const int CLERIC_WIDTH = 9;
        private static readonly int[] WIDTHS = { WARRIOR_HEIGHT, ROGUE_HEIGHT, MAGE_HEIGHT, CLERIC_HEIGHT };

        // Info
        public string role;
        private int height, width;
        public List<Attack> projList = new List<Attack>();

        // Movement
        private Vector3 pos;
        private Vector3 rot;
        public float speed, initSpeed;


        // Jumping
        private bool jumping = false;
        private bool falling = false;
        public float jumpSpeed;
        private float maxHeight = 40f;
        private float minHeight;

        // Sound
        private List<SoundEffect> soundEffects;

        // MouseState
        MouseState prevMouseState;
        MouseState curMouseState;

        // KeyboardState
        KeyboardState prevKeyboardState;
        KeyboardState curKeyboardState;

        //Health and secondary stat
        public float health, secondary, secondaryRegenRate;
        public int healthMax, secondaryMax;
        public int attCost, spec1Cost, spec2Cost, spec3Cost;
        private float projSpeed;
        //swaps stamina for mana when true
        private bool isCaster;

        private bool atBoss;

        // Timer
        private float maxAttTime, maxSpec1Time, maxSpec2Time, maxSpec3Time; //time between shots
        private float attTimer, spec1Timer, spec2Timer, spec3Timer;

        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Player(List<SoundEffect> s, string r)
        {
            // Imported values
            soundEffects = s;
            role = r;

            // Set player position
            height = HEIGHTS[Array.IndexOf(ROLES, role)];
            width = WIDTHS[Array.IndexOf(ROLES, role)];
            pos = new Vector3(0, height, 0);
            rot = new Vector3(0, 0, 0);
            minHeight = pos.Y;

            // Prepare mouse state
            prevMouseState = Mouse.GetState();

            attTimer = 0f;
            spec1Timer = 0f;
            spec2Timer = 0f;
            spec3Timer = 0f;

            // Set player stats
            switch (this.role)
            {
                case "WARRIOR":
                    isCaster = false;
                    initSpeed = 5f;
                    speed = initSpeed;
                    jumpSpeed = 15f;
                    healthMax = 300;
                    health = healthMax;
                    secondaryMax = 100;
                    secondary = secondaryMax;
                    secondaryRegenRate = 0.1f;
                    projSpeed = 0f;
                    maxAttTime = 0.5f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 10;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = 50;
                    break;
                case "ROGUE":
                    isCaster = false;
                    initSpeed = 20f;
                    speed = initSpeed;
                    jumpSpeed = 15f;
                    healthMax = 100;
                    health = healthMax;
                    secondaryMax = 300;
                    secondary = secondaryMax;
                    secondaryRegenRate = 0.1f;
                    projSpeed = 0f;
                    maxAttTime = 0.5f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 10;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = 50;
                    break;
                case "MAGE":
                    isCaster = true;
                    initSpeed = 10f;
                    speed = initSpeed;
                    jumpSpeed = 15f;
                    healthMax = 100;
                    health = healthMax;
                    secondaryMax = 300;
                    secondary = secondaryMax;
                    secondaryRegenRate = 0.1f;
                    projSpeed = 5f;
                    maxAttTime = 0.5f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 10;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = 50;
                    break;
                case "CLERIC":
                    isCaster = true;
                    initSpeed = 15f;
                    speed = initSpeed;
                    jumpSpeed = 15f;
                    healthMax = 300;
                    health = healthMax;
                    secondaryMax = 100;
                    secondary = secondaryMax;
                    secondaryRegenRate = 0.1f;
                    projSpeed = 15f;
                    maxAttTime = 0.5f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 10;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = 50;
                    break;
            }

            //accommodate for changes caused by deltaTime
            speed *= 5f;
            jumpSpeed *= 5f;
        }



        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime, Camera cam)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Regular Gameplay
            Move(dt);
            Abilities(dt, cam);

            // Debugging
            //NoClip(dt);
            SwitchRole();
            DebugStats();
        }

        private void Move(float dt)
        {
            // Move Faster
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                speed *= 2f;

            if (this.pos.Z >= 3505)
            {
                atBoss = true;
            }

            // Move forward
            // FOR NOW the player stops at z = 2200 as the current test stage ends there.  
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (this.pos.Z <= 4700)
                    this.pos += new Vector3(0, 0, 1) * speed * dt;
            }

            // Move back
            if (Keyboard.GetState().IsKeyDown(Keys.Back) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (this.pos.Z >= 0 && !atBoss)
                    this.pos -= new Vector3(0, 0, 1) * speed * dt;
                if (this.pos.Z >= 3505 && atBoss)
                    this.pos -= new Vector3(0, 0, 1) * speed * dt;
            }

            // Move left
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (this.pos.X <= 119 - this.width && !atBoss)
                    this.pos += new Vector3(1, 0, 0) * speed * dt;
                if (this.pos.X <= 724 - this.width && atBoss)
                    this.pos += new Vector3(1, 0, 0) * speed * dt;
            }


            // Move right
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (this.pos.X >= -119 + this.width && !atBoss)
                    this.pos -= new Vector3(1, 0, 0) * speed * dt;
                if (this.pos.X >= -724 + this.width && atBoss)
                    this.pos -= new Vector3(1, 0, 0) * speed * dt;
            }

            speed = initSpeed * 5f;

            // Initiate jump
            if (!jumping && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                jumping = true;
            }

            // Calculate jump
            if (jumping)
                Jump(dt);

            //regen Stamina
            if (secondary <= secondaryMax)
            {
                secondary += secondaryRegenRate;
            }
        }

        private void Jump(float dt)
        {
            // Determine if player is still going up, if so, increase jump height
            if (Pos.Y <= maxHeight && !falling)
            {
                pos.Y += jumpSpeed * dt;
            }
            // Determine if player is now going down
            else if (Pos.Y > maxHeight && !falling)
            {
                falling = true;
            }
            // Decrease jump height
            else if (falling && pos.Y > minHeight)
                pos.Y -= jumpSpeed * dt;
            // Once on the ground, stop jumping
            else if (pos.Y <= minHeight)
            {
                pos.Y = minHeight;
                falling = false;
                jumping = false;
            }
        }

        private void Abilities(float dt, Camera cam)
        {
            curMouseState = Mouse.GetState();
            curKeyboardState = Keyboard.GetState();

            if (attTimer > 0)
            {
                attTimer = attTimer - dt;
            }
            else
            {
                if (curMouseState.LeftButton == ButtonState.Pressed
                    && prevMouseState.LeftButton != ButtonState.Pressed
                    && secondary >= attCost)
                {
                    switch (this.isCaster)
                    {
                        case false:
                            AttackPattern.singleMel(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList);
                            break;
                        case true:
                            AttackPattern.singleProj(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList);
                            break;
                    }
                    attTimer = maxAttTime;

                    //expend resource
                    secondary -= attCost;
                }
            }
            if (spec1Timer > 0)
            {
                spec1Timer = spec1Timer - dt;
            }
            else
            {
                if (curMouseState.RightButton == ButtonState.Pressed
                    && prevMouseState.RightButton != ButtonState.Pressed
                    && secondary >= spec1Cost)
                {
                    switch (this.role)
                    {
                        case "WARRIOR":
                            break;
                        case "ROGUE":
                            break;
                        case "MAGE":
                            AttackPattern.quinProj(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList);
                            break;
                        case "CLERIC":
                            AttackPattern.tripleProj(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList);
                            break;
                    }

                    spec1Timer = maxSpec1Time;

                    //expend resource
                    secondary -= spec1Cost;
                }
            }
            if (spec2Timer > 0)
            {
                spec2Timer = spec2Timer - dt;
            }
            else
            {
                if (curKeyboardState.IsKeyDown(Keys.Q)
                    && prevKeyboardState.IsKeyUp(Keys.Q)
                    && secondary >= spec2Cost
                    && health != healthMax)
                {
                    switch (this.role)
                    {
                        case "WARRIOR":
                            break;
                        case "ROGUE":
                            break;
                        case "MAGE":
                            if (health + 25 < healthMax)
                                health += 25;
                            else
                                health = healthMax;
                            break;
                        case "CLERIC":
                            if (health + 50 < healthMax)
                                health += 50;
                            else
                                health = healthMax;
                            break;
                    }

                    spec2Timer = maxSpec2Time;

                    //expend resource
                    secondary -= spec2Cost;
                }
            }
            if (spec3Timer > 0)
            {
                spec3Timer = spec3Timer - dt;
            }
            else
            {
                if (curKeyboardState.IsKeyDown(Keys.E)
                    && prevKeyboardState.IsKeyUp(Keys.E)
                    && secondary >= spec3Cost)
                {
                    switch (this.role)
                    {
                        case "WARRIOR":
                            break;
                        case "ROGUE":
                            break;
                        case "MAGE":
                            if (pos.Z < 4700 - 400)
                                this.pos.Z += 400;
                            break;
                        case "CLERIC":
                            if (pos.Z < 4700 - 200)
                                this.pos.Z += 200;
                            break;
                    }
                    spec3Timer = maxSpec3Time;

                    //expend resource
                    secondary -= spec3Cost;
                }
            }

            foreach (Attack p in projList)
            {
                if (p.TimeToDestroy)
                {
                    projList.Remove(p);
                    break;
                }
            }

            prevMouseState = curMouseState;
            prevKeyboardState = curKeyboardState;
        }



        /////////////////////////
        ///DEBUGGING FUNCTIONS///
        /////////////////////////
        private void NoClip(float dt)
        {
            // Move Faster
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                speed *= 3;

            //Move left, right, forward, and back
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
                this.pos += new Vector3(1, 0, 0) * speed * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
                this.pos -= new Vector3(1, 0, 0) * speed * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
                this.pos += new Vector3(0, 0, 1) * speed * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                this.pos -= new Vector3(0, 0, 1) * speed * dt;

            // Rotate player left and right
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                this.rot += new Vector3(0, 1, 0) * speed * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.E))
                this.rot -= new Vector3(0, 1, 0) * speed * dt;

            // Float Up and Down
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                this.pos += new Vector3(0, 1, 0) * speed * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                this.pos -= new Vector3(0, 1, 0) * speed * dt;

            speed = initSpeed * 5f;
        }

        //update health and stamina bar
        public Rectangle resourceBarUpdate(bool isHealth, Rectangle bar, Vector2 screen, Vector2 scale)
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
                    (int)System.Math.Round(0.044f * screen.Y), nWid, (int)System.Math.Round(bar.Height * scale.Y));
            }
            else
            {
                //set width of bar based on current stat
                nWid = (int)System.Math.Round(secondary / secondaryMax * (0.202f * screen.X));
                //define new rectangle for bar
                newRect = new Rectangle(
                    (int)System.Math.Round(0.088f * screen.X),
                    (int)System.Math.Round(0.099f * screen.Y), nWid, (int)System.Math.Round(bar.Height * scale.Y));
            }

            return newRect;
        }

        private void SwitchRole()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                isCaster = false;
                initSpeed = 5f;
                speed = initSpeed;
                jumpSpeed = 15f;
                healthMax = 300;
                health = healthMax;
                secondaryMax = 100;
                secondary = secondaryMax;
                secondaryRegenRate = 0.1f;
                projSpeed = 0f;
                maxAttTime = 0.5f;
                maxSpec1Time = 0.5f;
                maxSpec2Time = 0.5f;
                maxSpec3Time = 0.5f;
                attCost = 10;
                spec1Cost = 20;
                spec2Cost = 30;
                spec3Cost = 50;
                role = ROLES[0];
                height = HEIGHTS[0];
                minHeight = height;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                isCaster = false;
                initSpeed = 20f;
                speed = initSpeed;
                jumpSpeed = 15f;
                healthMax = 100;
                health = healthMax;
                secondaryMax = 300;
                secondary = secondaryMax;
                secondaryRegenRate = 0.1f;
                projSpeed = 0f;
                maxAttTime = 0.5f;
                maxSpec1Time = 0.5f;
                maxSpec2Time = 0.5f;
                maxSpec3Time = 0.5f;
                attCost = 10;
                spec1Cost = 20;
                spec2Cost = 30;
                spec3Cost = 50;
                role = ROLES[1];
                height = HEIGHTS[1];
                minHeight = height;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                isCaster = true;
                initSpeed = 10f;
                speed = initSpeed;
                jumpSpeed = 15f;
                healthMax = 100;
                health = healthMax;
                secondaryMax = 300;
                secondary = secondaryMax;
                secondaryRegenRate = 0.1f;
                projSpeed = 5f;
                maxAttTime = 0.5f;
                maxSpec1Time = 0.5f;
                maxSpec2Time = 0.5f;
                maxSpec3Time = 0.5f;
                attCost = 10;
                spec1Cost = 20;
                spec2Cost = 30;
                spec3Cost = 50;
                role = ROLES[2];
                height = HEIGHTS[2];
                minHeight = height;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                isCaster = true;
                initSpeed = 15f;
                speed = initSpeed;
                jumpSpeed = 15f;
                healthMax = 300;
                health = healthMax;
                secondaryMax = 100;
                secondary = secondaryMax;
                secondaryRegenRate = 0.1f;
                projSpeed = 15f;
                maxAttTime = 0.5f;
                maxSpec1Time = 0.5f;
                maxSpec2Time = 0.5f;
                maxSpec3Time = 0.5f;
                attCost = 10;
                spec1Cost = 20;
                spec2Cost = 30;
                spec3Cost = 50;
                role = ROLES[3];
                height = HEIGHTS[3];
                minHeight = height;
            }
        }

        private void DebugStats()
        {
            // Test the damage mechanic
            if (Keyboard.GetState().IsKeyDown(Keys.U))
                health -= 3;
            // test the secondary resource mechanic
            if (Keyboard.GetState().IsKeyDown(Keys.P))
                secondary -= 3;
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
