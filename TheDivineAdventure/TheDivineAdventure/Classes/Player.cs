using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    class Player
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constant / Readonly
        // Make sure that the role, height, and width have the same index
        // (EX: WARRIOR is at index 0 of ROLES, while WARRIOR_HEIGHT is also at index 0 of HEIGHTS)
        public static readonly string[] ROLES = { "WARRIOR", "ROGUE", "MAGE", "CLERIC" };
        private const int WARRIOR_HEIGHT = 13;
        private const int ROGUE_HEIGHT = 13;
        private const int MAGE_HEIGHT = 13;
        private const int CLERIC_HEIGHT = 13;
        private static readonly int[] HEIGHTS = { WARRIOR_HEIGHT, ROGUE_HEIGHT, MAGE_HEIGHT, CLERIC_HEIGHT };
        private const int WARRIOR_WIDTH = 9;
        private const int ROGUE_WIDTH = 9;
        private const int MAGE_WIDTH = 9;
        private const int CLERIC_WIDTH = 9;
        private static readonly int[] WIDTHS = { WARRIOR_HEIGHT, ROGUE_HEIGHT, MAGE_HEIGHT, CLERIC_HEIGHT };

        // Info
        public string role;
        private int height, width;
        public List<Attack> projList = new List<Attack>();

        // Movement
        private Vector3 pos;
        private Vector3 rot;
        public float speed, initSpeed, runSpeed;

        
        // Jumping
        private bool jumping = false;
        private bool falling = false;
        public float jumpSpeed;
        private float maxHeight = 40f;
        private float minHeight;

        // Sound
        private List<SoundEffect> soundEffects;
        public static float volume;

        // MouseState
        MouseState prevMouseState;
        MouseState curMouseState;

        // KeyboardState
        KeyboardState prevKeyboardState;
        KeyboardState curKeyboardState;

        //Health and secondary stat
        public float health, secondary, secondaryRegenRate, runCost;
        public int healthMax, secondaryMax;
        public int attCost, spec1Cost, spec2Cost, spec3Cost;
        private float projSpeed;
        private bool isExhausted;
        //swaps stamina for mana when true
        private bool isCaster;

        private bool atEnd;

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

            isExhausted = false;

            // Set player stats
            switch (this.role)
            {
                case "WARRIOR":
                    isCaster = false;
                    initSpeed = 10f;
                    speed = initSpeed;
                    runSpeed = 2f;
                    jumpSpeed = 15f;
                    healthMax = 300;
                    health = healthMax;
                    secondaryMax = 100;
                    secondary = secondaryMax;
                    secondaryRegenRate = 0.15f;
                    projSpeed = 0f;
                    maxAttTime = 0.5f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 10;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = secondaryMax;
                    runCost = 0.2f;
                    break;
                case "ROGUE":
                    isCaster = false;
                    initSpeed = 10f;
                    speed = initSpeed;
                    runSpeed = 3f;
                    jumpSpeed = 15f;
                    healthMax = 100;
                    health = healthMax;
                    secondaryMax = 300;
                    secondary = secondaryMax;
                    secondaryRegenRate = 1f;
                    projSpeed = 0f;
                    maxAttTime = 0.1f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 1;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = secondaryMax;
                    runCost = 2f;
                    break;
                case "MAGE":
                    isCaster = true;
                    initSpeed = 15f;
                    speed = initSpeed;
                    runSpeed = 1f;
                    jumpSpeed = 15f;
                    healthMax = 100;
                    health = healthMax;
                    secondaryMax = 300;
                    secondary = secondaryMax;
                    secondaryRegenRate = 0.23f;
                    projSpeed = 5f;
                    maxAttTime = 0.5f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 10;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = 50;
                    runCost = 0f;
                    break;
                case "CLERIC":
                    isCaster = true;
                    initSpeed = 11f;
                    speed = initSpeed;
                    runSpeed = 1f;
                    jumpSpeed = 15f;
                    healthMax = 300;
                    health = healthMax;
                    secondaryMax = 100;
                    secondary = secondaryMax;
                    secondaryRegenRate = 0.25f;
                    projSpeed = 15f;
                    maxAttTime = 0.5f;
                    maxSpec1Time = 0.5f;
                    maxSpec2Time = 0.5f;
                    maxSpec3Time = 0.5f;
                    attCost = 10;
                    spec1Cost = 20;
                    spec2Cost = 30;
                    spec3Cost = 50;
                    runCost = 0f;
                    break;
            }

            //accommodate for changes caused by deltaTime
            speed *= 5f;
            jumpSpeed *= 5f;
        }



        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(float dt, Camera cam)
        {
            // Regular Gameplay
            Move(dt);
            Abilities(dt, cam);

            // Debugging
            //DebugMode();
        }

        private void Move(float dt)
        {
            // Move Faster
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                if (!isExhausted)
                {
                    speed *= runSpeed;

                    //expend resource
                    secondary -= runCost;
                }
            }
                

            if (this.pos.Z >= 3505)
            {
                atEnd = true;
            }

            // Move forward
            // FOR NOW the player stops at z = 4700 as the current test stage ends there.  
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (this.pos.Z <= 4700)
                    this.pos += new Vector3(0, 0, 1) * speed * dt;
            }

            // Move back
            if (Keyboard.GetState().IsKeyDown(Keys.Back) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (this.pos.Z >= 0 && !atEnd)
                    this.pos -= new Vector3(0, 0, 1) * speed * dt;
                if (this.pos.Z >= 3505 && atEnd)
                    this.pos -= new Vector3(0, 0, 1) * speed * dt;
            }

            // Move left
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (this.pos.X <= 119 - this.width && !atEnd)
                    this.pos += new Vector3(1, 0, 0) * speed * dt;
                if (this.pos.X <= 724 - this.width && atEnd)
                    this.pos += new Vector3(1, 0, 0) * speed * dt;
            }


            // Move right
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (this.pos.X >= -119 + this.width && !atEnd)
                    this.pos -= new Vector3(1, 0, 0) * speed * dt;
                if (this.pos.X >= -724 + this.width && atEnd)
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
            if (secondary < secondaryMax)
            {
                secondary += secondaryRegenRate;
            }
            else if (secondary >= secondaryMax)
            {
                isExhausted = false;
            }

            if (secondary <= 0)
            {
                isExhausted = true;
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
                            soundEffects[0].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                            AttackPattern.singleMel(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList);
                            break;
                        case true:
                            soundEffects[1].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
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
                    && secondary >= spec1Cost
                    && !isExhausted)
                {
                    switch (this.role)
                    {
                        case "WARRIOR":
                            soundEffects[2].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                            AttackPattern.tripleMel(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList); ;
                            break;
                        case "ROGUE":
                            soundEffects[2].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                            AttackPattern.tripleMel(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList);
                            break;
                        case "MAGE":
                            soundEffects[1].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                            soundEffects[1].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                            AttackPattern.quinProj(this.Pos, cam.LookAt + this.Pos, this.projSpeed, this.projList);
                            break;
                        case "CLERIC":
                            soundEffects[1].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                            soundEffects[1].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
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
                    && !isExhausted)
                {
                    switch (this.role)
                    {
                        case "WARRIOR":
                            if (health - 25 > 0 && secondary + 50 < secondaryMax)
                            {
                                soundEffects[5].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                health -= 25;
                                secondary += 50;
                            }
                            else if (health - 25 > 0)
                            {
                                soundEffects[5].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                health -= 25;
                                secondary = secondaryMax;
                            }
                            break;
                        case "ROGUE":
                            if (health - 50 > 0 && secondary + 50 < secondaryMax)
                            {
                                soundEffects[5].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                health -= 50;
                                secondary += 100;
                            }
                            else if (health - 50 > 0)
                            {
                                soundEffects[5].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                health -= 50;
                                secondary = secondaryMax;
                            }
                            break;
                        case "MAGE":
                            if (pos.Z < 4700 - 400)
                            {
                                soundEffects[4].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                spec3Timer = maxSpec3Time;
                                this.pos.Z += 400;
                                secondary -= spec2Cost;
                            }
                            break;
                        case "CLERIC":
                            if (pos.Z < 4700 - 200)
                            {
                                soundEffects[4].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                spec3Timer = maxSpec3Time;
                                this.pos.Z += 200;
                                secondary -= spec2Cost;
                            }
                            break;
                    }

                    spec2Timer = maxSpec2Time;
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
                    && secondary >= spec3Cost
                    && !isExhausted)
                {
                    switch (this.role)
                    {
                        case "WARRIOR":
                        case "ROGUE":
                            if (health != healthMax)
                            {
                                soundEffects[6].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                health = healthMax;
                                secondary = 0;
                                isExhausted = true;
                            }
                            spec3Timer = maxSpec3Time;
                            break;
                        case "MAGE":
                            if (health + 25 < healthMax)
                            {
                                soundEffects[3].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                health += 25;
                                secondary -= spec3Cost;
                            }
                            else
                            {
                                soundEffects[3].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                                health = healthMax;
                                secondary -= spec3Cost;
                            }

                            break;
                        case "CLERIC":
                            soundEffects[3].Play(volume: volume, pitch: 0.0f, pan: 0.0f);
                            if (health + 50 < healthMax)
                            {
                                health += 50;
                                secondary -= spec3Cost;
                            }
                            else
                            {
                                secondary -= spec3Cost;
                                health = healthMax;
                            }
                    break;
                    }
                   
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
        private void DebugMode(float dt)
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

            // Test the damage mechanic
            if (Keyboard.GetState().IsKeyDown(Keys.U))
                health -= 3;
            // test the secondary resource mechanic
            if (Keyboard.GetState().IsKeyDown(Keys.P))
                secondary -= 3;

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

        public int Height
        {
            get { return height; }
            set { height = value; }
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
