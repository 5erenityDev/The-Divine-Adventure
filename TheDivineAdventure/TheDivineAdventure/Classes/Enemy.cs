using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

namespace TheDivineAdventure
{
    class Enemy
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constants
        private const int demonHeight = 23;

        // Movement
        private Vector3 pos;


        // Sound
        private List<SoundEffect> soundEffects;

        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Enemy(List<SoundEffect> s)
        {
            soundEffects = s;
            pos = new Vector3(0, demonHeight, 100);
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
