using NPCs;
using UnityEngine;

namespace World
{
    /// <summary>
    /// This class represents the Enemy class in the world
    /// </summary>
    public class Enemy : Character<Enemies>
    {
        public override void SetAnimationBase()
        {
            Animation = gameObject.AddComponent<Base.Animation>();
            SetFrameRates(Animation);
            Animation.AnimationSprites = Manager.Game.Sprites.EnemyAnimations[CharacterType];
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Manager.Sword sword = collision.gameObject.GetComponent<Manager.Sword>();
            if (sword != null)
            {
                TakeDamage(sword.GetDamage(), sword.GetDamageModifier());
                if (IsDead())
                {
                    DestroySelf();
                }
            }
        }

        public void SetFrameRates(Base.Animation animator)
        {
            float action = 0f;
            float idle = 0f;
            float walk = 0f;
            switch (CharacterType)
            {
                case Enemies.OctorokBlue:
                case Enemies.Octorok:
                    walk = 0.3f;
                    action = 0.33f;
                    idle = 1f;
                    break;
            }
            animator.ActionFrameRate = action;
            animator.IdleFrameRate = idle;
            animator.WalkFrameRate = walk;
        }
    }
}
