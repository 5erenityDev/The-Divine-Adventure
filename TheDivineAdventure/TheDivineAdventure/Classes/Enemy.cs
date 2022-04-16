using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    class Enemy
    {
        ///////////////
        ///VARIABLES///
        ///////////////
        // Constant / Readonly
        // Make sure that the role and height have the same index
        // (EX: DEMON is at index 0 of ROLES, while DEMON_HEIGHT is also at index 0 of HEIGHTS)
        public static readonly string[] ROLES = { "DEMON", "HELLHOUND", "IMP", 
                                    "GOBLIN", "OGRE", "GARGOYLE", "SKELETON" };
        private const int DEMON_HEIGHT = 23;
        private const int HELLHOUND_HEIGHT = 13;
        private const int IMP_HEIGHT = 0;
        private const int GOBLIN_HEIGHT = 0;
        private const int OGRE_HEIGHT = 23;
        private const int GARGOYLE_HEIGHT = 13;
        private const int SKELETON_HEIGHT = 0;
        private static readonly int[] HEIGHTS = { DEMON_HEIGHT, HELLHOUND_HEIGHT, IMP_HEIGHT,
                                    GOBLIN_HEIGHT, OGRE_HEIGHT, GARGOYLE_HEIGHT, SKELETON_HEIGHT };
        
        // Info
        public string role;
        private int height;
        public List<Projectile> projList = new List<Projectile>();

        // Movement
        private Vector3 pos;


        // Sound
        private List<SoundEffect> soundEffects;



        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        public Enemy(List<SoundEffect> s, string r)
        {
            soundEffects = s;
            role = r;
            height = HEIGHTS[Array.IndexOf(ROLES, role)];
            pos = new Vector3(0, height, 100);
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
