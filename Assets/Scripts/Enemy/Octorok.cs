using NPCs;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Octorok : World.Enemy
    {
        public override void SetHealth()
        {
            Health = 2f;
        }

        public override void SetAttackStrength()
        {
            TouchDamage = 1f;
            WeaponDamage = 1f;
        }

        public override void SetFrameRates()
        {
            // TODOJEF: Figure out a good multiplier for this... it's too fast, but we do need to speed up the animations slightly
            Animation.ActionFrameRate = 0.33f / Movement.Speed;
            Animation.IdleFrameRate = 1f;
            Animation.WalkFrameRate = 0.3f / Movement.Speed;
        }

        /// <summary>
        /// This method is set in all of the enemy classes and called from EnemyHelper
        /// </summary>
        /// <param name="subTypes"></param>
        /// <param name="colors"></param>
        public static void GetConfig(List<Enemies> subTypes, List<Color[]> colors)
        {
            subTypes.AddRange(new List<Enemies> {
                Enemies.Octorok,
                Enemies.OctorokBlue
            });
            colors.AddRange(new List<Color[]> {
                new Color[] { EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_RED, EnemyHelper.ACCENT_COLOR, EnemyHelper.COMMON_ORANGE },
                new Color[] { EnemyHelper.BASE_COLOR, EnemyHelper.COMMON_BLUE, EnemyHelper.ACCENT_COLOR, EnemyHelper.COMMON_BLUE_LIGHT }
            });
        }
    }
}
