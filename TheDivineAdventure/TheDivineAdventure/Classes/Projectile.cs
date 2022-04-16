using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    class Projectile
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constant / Readonly

        // Info

        // Movement
        private Vector3 pos;
        private Vector3 rot;
        private float speed = 1f;


        // Sound

        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Projectile()
        {

        }



        ///////////////
        ///FUNCTIONS///
        ///////////////
        public void Update(GameTime gameTime)
        {

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
