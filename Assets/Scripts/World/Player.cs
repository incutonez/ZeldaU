using System;
using UnityEngine;

namespace World
{
    public class Player : Character<BaseCharacter>
    {
        public UIInventory uiInventory;
        public const float SPEED = 5f;
        public event EventHandler OnInitialize;
        public event EventHandler OnTakeDamage;

        private Inventory inventory;

        private void Start()
        {
            // TODOJEF: Pick up here, have to get sword rendering
            CanAttack = false;
            SpriteRenderer shield = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            shield.gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item itemWorld = collision.GetComponent<Item>();
            if (itemWorld != null)
            {
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Manager.Game.IsTransitioning)
            {
                return;
            }
            Enemy worldEnemy = collision.collider.GetComponent<Enemy>();
            if (worldEnemy != null)
            {
                BaseCharacter.TakeDamage(worldEnemy.GetTouchDamage() * inventory.damageModifier);
                OnTakeDamage?.Invoke(this, EventArgs.Empty);
                if (BaseCharacter.IsDead())
                {
                    DestroySelf();
                }
            }
        }

        public override void LoadSprites()
        {
            Manager.Sprites.GetPlayerSprites(
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

        public override void AnimateMove(Vector3 Movement)
        {
            base.AnimateMove(Movement);
            Manager.Game.Shield.ToggleShields(LastMovement.y == -1f || LastMovement == Vector3.zero, LastMovement.x > 0f, LastMovement.x < 0f);
        }

        public override void AnimateAction()
        {
            base.AnimateAction();
            Manager.Game.Sword.Swing(LastMovement);
        }

        public override void SpriteAnimator_OnAnimationStop(object sender, EventArgs e)
        {
            base.SpriteAnimator_OnAnimationStop(sender, e);
            // TODOJEF: Don't like this... maybe make it specific to the action?
            Manager.Game.Sword.ToggleSword(false);
        }

        private void UseItem(global::Item item)
        {
            switch (item.Type)
            {
                case Items.PotionBlue:
                    // TODOJEF: Flash character red
                    inventory.RemoveItem(new global::Item { Type = item.Type, Amount = 1 });
                    break;
            }
        }

        public void InitializedCharacter()
        {
            inventory = new Inventory(UseItem);
            uiInventory = Manager.Game.Inventory;
            uiInventory.SetInventory(inventory);
            uiInventory.SetPlayer(this);
            OnInitialize?.Invoke(this, EventArgs.Empty);
        }
    }
}
