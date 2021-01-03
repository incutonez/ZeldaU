using NPCs;
using UnityEngine;

namespace World
{
    /// <summary>
    /// This class represents the Enemy class in the world
    /// </summary>
    public class Enemy : Character<global::Enemy>
    {
        public override void LoadSprites()
        {
            Manager.Game.Sprites.GetEnemySprites(
                (Enemies) BaseCharacter.characterType,
                ActionUpAnimation,
                ActionDownAnimation,
                ActionRightAnimation,
                ActionLeftAnimation,
                IdleUpAnimation,
                IdleDownAnimation,
                IdleRightAnimation,
                IdleLeftAnimation,
                WalkUpAnimation,
                WalkDownAnimation,
                WalkRightAnimation,
                WalkLeftAnimation
            );
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Manager.Sword sword = collision.gameObject.GetComponent<Manager.Sword>();
            if (sword != null)
            {
                BaseCharacter.TakeDamage(sword.GetDamage(), sword.GetDamageModifier());
                if (BaseCharacter.IsDead())
                {
                    DestroySelf();
                }
            }
        }
    }
}
