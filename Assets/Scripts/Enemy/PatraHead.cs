using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class PatraHead : World.Enemy
    {
        public override void SetHealth()
        {
            // TODOJEF: GET?
            Health = 20f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 4f;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.PatraHead
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_ORANGE, EnemyHelper.ACCENT_COLOR, EnemyHelper.COMMON_RED }
            });
        }
    }
}