using Microsoft.Xna.Framework;
using System;

namespace TheDivineAdventure
{
    class Projectile
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constant / Readonly

        // Info
        private bool timeToDestroy; //destroys object when true

        // Movement
        private Vector3 initPos, pos;
        private Vector3 dest, vel;
        private float distance;
        private float speed;

        // Timer
        private float timer;        //how long the projectile stays active


        // Sound

        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Projectile(Vector3 origin, Vector3 target, float pSpeed)
        {
            // Timer stats
            timer = 1f;
            timeToDestroy = false;

            // Set the projectiles initial position
            initPos = origin;
            pos = initPos;

            // Set the projectiles destintation
            dest.X = target.X - pos.X;
            dest.Y = target.Y - pos.Y;
            dest.Z = target.Z - pos.Z;

            // Find the distance between them
            distance = (float)Math.Sqrt(
                        target.X * target.X
                        + target.Y * target.Y
                        + target.Z * target.Z);

            // Determine the velocity on each axis using the distance
            speed = pSpeed*50;
            vel.X = (target.X / distance) * speed;
            vel.Y = (target.Y / distance) * speed;
            vel.Z = (target.Z / distance) * speed;
        }

        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime)
        {
            if (timer > 0f)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                pos.X += vel.X * dt;
                pos.Y += vel.Y * dt;
                pos.Z += vel.Z * dt;
                timer = timer - dt;
            }
            else
            {
                timeToDestroy = true;
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

        public bool TimeToDestroy
        {
            get { return timeToDestroy; }
            set { timeToDestroy = value; }
        }
    }
}
