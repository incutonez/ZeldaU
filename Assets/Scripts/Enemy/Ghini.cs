using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// You can only destroy the main Ghini... the other ones cannot be attacked
    /// </summary>
    public class Ghini : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 22f;
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
                Enemies.Ghini
            });
            colors.AddRange(new List<Color[]> {
                new[] { EnemyHelper.BaseColor, EnemyHelper.CommonBlue, EnemyHelper.AccentColor, EnemyHelper.CommonBlueLight }
            });
        }
    }
}
