using Microsoft.Xna.Framework;

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
        private Vector3 dest;
        private float speed = 10f;

        // Timer
        private float timer;        //how long the projectile stays active


        // Sound

        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Projectile(Player entity, Vector3 camRot)
        {
            // Timer stats
            timer = 1f;
            timeToDestroy = false;

            initPos = entity.Pos;
            pos = initPos;
            dest = camRot;
        }

        public Projectile(Enemy entity, Vector3 camRot)
        {
            // Timer stats
            timer = 1f;
            timeToDestroy = false;

            initPos = entity.Pos;
            pos = initPos;
            dest = camRot;
        }

        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime)
        {
            if (timer > 0f)
            {
                pos.X += dest.Y * speed;
                pos.Y -= dest.X * speed;
                pos.Z += speed;
                timer = timer - (float)gameTime.ElapsedGameTime.TotalSeconds;
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
