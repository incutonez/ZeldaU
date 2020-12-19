using UnityEngine;

/// <summary>
/// This class represents the Enemy class in the world
/// </summary>
public class WorldEnemy : WorldCharacter<Enemy>
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SwordHandler sword = collision.gameObject.GetComponent<SwordHandler>();
        if (sword != null)
        {
            character.TakeDamage(sword.GetDamage(), sword.GetDamageModifier());
            if (character.IsDead())
            {
                DestroySelf();
            }
        }
    }
}
