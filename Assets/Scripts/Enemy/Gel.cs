using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Gel : World.Enemy
    {
        public static readonly Color TEAL = Utilities.HexToColor("004058");

        public override void SetHealth()
        {
            Health = 2f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.Gel,
                Enemies.GelBlue
            });
            colors.AddRange(new List<Color[]> {
                // We don't replace anything in the base one because it want it to black
                null,
                new Color[] { EnemyHelper.BASE_COLOR, TEAL }
            });
        }
    }
}