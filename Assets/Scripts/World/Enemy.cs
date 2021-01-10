using NPCs;
using UnityEngine;

namespace World
{
    /// <summary>
    /// This class represents the Enemy class in the world
    /// </summary>
    public class Enemy : Character<Enemies>
    {
        public Base.AI AI { get; set; }

        private void Awake()
        {
            AI = GetComponent<Base.AI>();
        }

        public override void SetAnimationBase()
        {
            Animation = gameObject.AddComponent<Base.Animation>();
            SetFrameRates();
            Animation.AnimationSprites = Manager.Game.Graphics.EnemyAnimations[CharacterType];
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

        public override void Enable()
        {
            base.Enable();
            // TODO: Potentially generate random speeds here?
            AI.Reset();
        }

        public virtual void SetFrameRates()
        {
            Animation.ActionFrameRate = 0f;
            Animation.IdleFrameRate = 0f;
            Animation.WalkFrameRate = 0f;
        }
    }
}
