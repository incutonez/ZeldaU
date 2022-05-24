using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// If sucked in, the player will randomly lose their Magical Shield
    /// </summary>
    public class LikeLike : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 20f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 2f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.LikeLike
            });
            colors.AddRange(new List<Color[]> {
                new[] { EnemyHelper.BaseColor, EnemyHelper.CommonOrange, EnemyHelper.AccentColor, EnemyHelper.CommonRed }
            });
        }
    }
}
