using NPCs;
using System;
using System.Collections;
using UnityEngine;

namespace World
{
    public class Player : Character<Characters>
    {
        public const float SPEED = 5f;
        public PlayerInventory UiInventory { get; set; }
        public event EventHandler OnInitialize;
        public event EventHandler OnTakeDamage;

        private Base.Inventory Inventory { get; set; }

        private void Start()
        {
            Animation.CanAttack = false;
            SpriteRenderer shield = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            shield.gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item itemWorld = collision.GetComponent<Item>();
            if (itemWorld != null)
            {
                Inventory.AddItem(itemWorld.GetItem());
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
                TakeDamage(worldEnemy.TouchDamage * Inventory.damageModifier);
                OnTakeDamage?.Invoke(this, EventArgs.Empty);
                if (IsDead())
                {
                    DestroySelf();
                }
            }
        }

        public IEnumerator AnimateEnter()
        {
            return Animation.AnimateEnter();
        }

        public IEnumerator AnimateExit()
        {
            return Animation.AnimateExit();
        }

        public override void SetAnimationBase()
        {
            Animation = gameObject.AddComponent<Animation.Player>();
            Animation.AnimationSprites = Manager.Game.Graphics.PlayerAnimations;
        }

        private void UseItem(Base.Item item)
        {
            switch (item.Type)
            {
                case Items.PotionBlue:
                    // TODOJEF: Flash character red
                    Inventory.RemoveItem(new Base.Item { Type = item.Type, Amount = 1 });
                    break;
            }
        }

        public override void SetHealth()
        {
            Health = 32f;
        }

        public override void Initialize(Characters characterType)
        {
            base.Initialize(characterType);
            Inventory = new Base.Inventory(UseItem);
            UiInventory = Manager.Game.Inventory;
            UiInventory.SetInventory(Inventory);
            UiInventory.SetPlayer(this);
            OnInitialize?.Invoke(this, EventArgs.Empty);
        }
    }
}
