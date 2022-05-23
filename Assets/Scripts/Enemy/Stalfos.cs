using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Stalfos : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 4f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 0.25f;
            // TODO: Verify the shoot damage of the sword
            WeaponDamage = 0.25f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.Stalfos
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BaseColor, EnemyHelper.CommonOrange, EnemyHelper.AccentColor, EnemyHelper.CommonRed }
            });
        }
    }
}