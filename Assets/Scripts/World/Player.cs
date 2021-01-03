using System;
using System.Collections;
using UnityEngine;

namespace World
{
    public class Player : Character<BaseCharacter>
    {
        public const float SPEED = 5f;
        public UIInventory uiInventory { get; set; }
        public event EventHandler OnInitialize;
        public event EventHandler OnTakeDamage;

        private Inventory inventory { get; set; }

        private void Start()
        {
            Animator.CanAttack = false;
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

        public IEnumerator AnimateEnter()
        {
            return Animator.AnimateEnter();
        }

        public IEnumerator AnimateExit()
        {
            return Animator.AnimateExit();
        }

        public override void SetAnimationBase()
        {
            Animator = gameObject.AddComponent<AnimatorPlayer>();
            Animator.AnimationSprites = Manager.Game.Sprites.PlayerAnimations;
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
