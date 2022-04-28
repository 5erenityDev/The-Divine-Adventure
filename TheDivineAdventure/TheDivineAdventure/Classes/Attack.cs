using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    class Attack
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constant / Readonly

        // Info
        private bool isMelee, timeToDestroy; //destroys object when true

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
        public Attack(Vector3 origin, Vector3 target, float pSpeed)
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
            if (pSpeed <= 0)
            {
                isMelee = true;
            }
            else
            {
                isMelee = false;
                speed = pSpeed * 50;
                vel.X = (target.X / distance) * speed;
                vel.Y = (target.Y / distance) * speed;
                vel.Z = (target.Z / distance) * speed;
            }

        }

        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(float dt, Player player)
        {
            if (timer > 0f)
            {
                if (CheckCollision(player))
                {
                    player.Health -= 50;
                    timeToDestroy = true;
                }
                else
                {
                    pos.X += vel.X * dt;
                    pos.Y += vel.Y * dt;
                    pos.Z += vel.Z * dt;
                }

                timer = timer - dt;
            }
            else
            {
                timeToDestroy = true;
            }
        }

        public void Update(float dt, List<Enemy> enemyList)
        {
            if (timer > 0f)
            {
                foreach (Enemy e in enemyList)
                {
                    if (CheckCollision(e))
                    {
                        e.Health -= 50;
                        timeToDestroy = true;
                    }
                }
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

        private bool CheckCollision(Player player)
        {
            if (isMelee)
            {
                return false;
            }
            else
            {
                if (this.boundingSphere.Intersects(new BoundingBox(
                    new Vector3(player.Pos.X - 5, player.Pos.Y - player.Height, player.Pos.Z - 5),
                    new Vector3(player.Pos.X + 5, player.Pos.Y + player.Height, player.Pos.Z + 5))))
                {
                    return true;
                }
                return false;
            }
        }

        private bool CheckCollision(Enemy enemy)
        {
            if (isMelee)
            {
                return false;
            }
            else
            {
                if (this.boundingSphere.Intersects(new BoundingBox(
                    new Vector3(enemy.Pos.X - 5, enemy.Pos.Y - enemy.Height, enemy.Pos.Z - 5),
                    new Vector3(enemy.Pos.X + 5, enemy.Pos.Y + enemy.Height, enemy.Pos.Z + 5))))
                {
                    return true;
                }
                return false;
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

        public bool IsMelee
        {
            get { return isMelee; }
            set { isMelee = value; }
        }

        private BoundingSphere boundingSphere
        {
            get { return new BoundingSphere(pos, 0.010000003f); }
        }
    }
}
