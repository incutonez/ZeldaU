using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// When this catches player, the player returns to the beginning of the castle
    /// </summary>
    public class Wallmaster : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 6f;
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
                Enemies.Wallmaster
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BaseColor, EnemyHelper.CommonBlueLight, EnemyHelper.AccentColor, EnemyHelper.CommonBlue }
            });
        }
    }
}