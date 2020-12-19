﻿using System.Collections;
using UnityEngine;

public class WorldPlayer : WorldCharacter<BaseCharacter>
{
    public UIInventory uiInventory;
    public const float SPEED = 1f;

    private Rigidbody2D rb2d;
    private CharacterAnimation characterAnimation;
    private Inventory inventory;
    private Vector3 movement;
    private bool isAttacking = false;
    private float? lastAttack = 0f;

    public new void Awake()
    {
        base.Awake();
        rb2d = GetComponent<Rigidbody2D>();
        characterAnimation = GetComponent<CharacterAnimation>();
        inventory = new Inventory(UseItem);
        uiInventory = FindObjectOfType<UIInventory>();
        uiInventory.SetInventory(inventory);
        uiInventory.SetPlayer(this);
    }

    private void Start()
    {
        // TODOJEF????
        SpriteRenderer shield = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        shield.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WorldItem itemWorld = collision.GetComponent<WorldItem>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        WorldEnemy worldEnemy = collision.gameObject.GetComponent<WorldEnemy>();
        if (worldEnemy != null)
        {
            Debug.Log(worldEnemy.GetTouchDamage());
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Items.PotionBlue:
                // TODOJEF: Flash character red
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                break;
        }
    }

    public IEnumerator Attack()
    {
        if (inventory.hasSword)
        {
            isAttacking = true;
            yield return StartCoroutine(characterAnimation.Attack());
            lastAttack = Constants.ATTACK_DELAY;
            isAttacking = false;
        }
    }

    // Taken from https://www.youtube.com/watch?v=Bf_5qIt9Gr8
    public void Move()
    {
        if (!isAttacking)
        {
            if (!lastAttack.HasValue && Input.GetKey(KeyCode.RightControl))
            {
                StartCoroutine(Attack());
            }
            else
            {
                float moveX = 0f;
                float moveY = 0f;
                if (lastAttack.HasValue)
                {
                    // Keep decrementing until we've hit the threshold
                    lastAttack -= Time.deltaTime;
                    if (lastAttack <= Constants.ATTACK_DELAY_THRESHOLD)
                    {
                        lastAttack = null;
                    }
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    moveY = 1f;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    moveY = -1f;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    moveX = -1f;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    moveX = 1f;
                }
                movement = new Vector3(moveX, moveY).normalized;
                characterAnimation.Animate(movement);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            rb2d.velocity = Vector2.zero;
        }
        else
        {
            // Taken from https://www.youtube.com/watch?v=Bf_5qIt9Gr8
            rb2d.velocity = movement * SPEED;
        }
    }
}
