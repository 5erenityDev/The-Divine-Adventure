﻿using Microsoft.Xna.Framework;
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
        ///PROJECTILES///
        public static void singleProj(Vector3 origin, Vector3 target, float speed, List<Attack> projList)
        {
            projList.Add(new Attack(origin, target, speed));
        }

        public static void tripleProj(Vector3 origin, Vector3 target, float speed, List<Attack> projList)
        {
            projList.Add(new Attack(origin, target + new Vector3(20, 0, 0), speed));
            projList.Add(new Attack(origin, target, speed));
            projList.Add(new Attack(origin, target - new Vector3(20, 0, 0), speed));
        }

        public static void quinProj(Vector3 origin, Vector3 target, float speed, List<Attack> projList)
        {
            projList.Add(new Attack(origin, target + new Vector3(40, 0, 0), speed));
            projList.Add(new Attack(origin, target + new Vector3(20, 0, 0), speed));
            projList.Add(new Attack(origin, target, speed));
            projList.Add(new Attack(origin, target - new Vector3(20, 0, 0), speed));
            projList.Add(new Attack(origin, target - new Vector3(40, 0, 0), speed));
        }

        ///MELEE///
        public static void singleMel(Vector3 origin, Vector3 target, float speed, List<Attack> projList)
        {
            projList.Add(new Attack(origin + new Vector3(0, 0, 10), target, speed));
        }
        public static void tripleMel(Vector3 origin, Vector3 target, float speed, List<Attack> projList)
        {
            projList.Add(new Attack(origin + new Vector3(0, 0, 10), target, speed));
            projList.Add(new Attack(origin + new Vector3(20, 0, 10), target, speed));
            projList.Add(new Attack(origin + new Vector3(-20, 0, 10), target, speed));
        }
    }
}
