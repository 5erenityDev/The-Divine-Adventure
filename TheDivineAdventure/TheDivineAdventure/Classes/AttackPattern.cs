using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TheDivineAdventure
{
    class AttackPattern
    {
        /////////////////
        ///CONSTRUCTOR///
        /////////////////
        AttackPattern()
        {

        }

        ///////////////
        ///FUNCTIONS///
        ///////////////
        public static void singleProj(Vector3 origin, Vector3 target, float speed, List<Projectile> projList)
        {
            projList.Add(new Projectile(origin, target, speed));
        }

        public static void tripleProj(Vector3 origin, Vector3 target, float speed, List<Projectile> projList)
        {
            projList.Add(new Projectile(origin, target + new Vector3(20, 0, 0), speed));
            projList.Add(new Projectile(origin, target, speed));
            projList.Add(new Projectile(origin, target - new Vector3(20, 0, 0), speed));
        }

        public static void quinProj(Vector3 origin, Vector3 target, float speed, List<Projectile> projList)
        {
            projList.Add(new Projectile(origin, target + new Vector3(40, 0, 0), speed));
            projList.Add(new Projectile(origin, target + new Vector3(20, 0, 0), speed));
            projList.Add(new Projectile(origin, target, speed));
            projList.Add(new Projectile(origin, target - new Vector3(20, 0, 0), speed));
            projList.Add(new Projectile(origin, target - new Vector3(40, 0, 0), speed));
        }
    }
}
