using NPCs;
using UnityEngine;

namespace World
{
    /// <summary>
    /// This class represents the Enemy class in the world
    /// </summary>
    public class Enemy : Character<global::Enemy>
    {
        public override void SetAnimationBase()
        {
            Animator = gameObject.AddComponent<AnimatorBase>();
            BaseCharacter.SetFrameRates(Animator);
            Animator.AnimationSprites = Manager.Game.Sprites.EnemyAnimations[(Enemies)BaseCharacter.characterType];
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
