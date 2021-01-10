using Audio;
using UnityEngine;

namespace Manager
{
    public class Sword : MonoBehaviour
    {
        private SpriteRenderer Renderer { get; set; }
        private Items SwordType { get; set; }
        private DamageAttribute Damage { get; set; }
        private BoxCollider2D Collider { get; set; }
        private Base.Animation PlayerAnimation { get; set; }

        public void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<BoxCollider2D>();
            PlayerAnimation = Game.Player.Animation;
            ToggleSword(false);
        }

        private void Update()
        {
            // We call this in update for every frame because it's possible the player has been pushed, and if that's the
            // case, we want the sword to reposition to where the player is
            SetPosition(PlayerAnimation.LastMovement);
        }

        public void Swing()
        {
            Game.Shield.ToggleShields(false);
            ToggleSword(true);
            // TODOJEF: Have to determine which Sword FX should play... need to know if at full health
            Game.Audio.PlayFX(FX.SwordSlash);
        }

        public void ToggleSword(bool enabled = false)
        {
            gameObject.SetActive(enabled);
        }

        public void SetPosition(Vector3 movement)
        {
            if (movement.x > 0)
            {
                transform.localPosition = Constants.SWORD_RIGHT;
                transform.localRotation = Constants.SWORD_X_ROTATION;
                Renderer.flipY = false;
                Collider.offset = Constants.SWORD_COLLIDER_POSITIVE;
            }
            else if (movement.x < 0)
            {
                transform.localPosition = Constants.SWORD_LEFT;
                transform.localRotation = Constants.SWORD_X_ROTATION;
                Renderer.flipY = true;
                Collider.offset = Constants.SWORD_COLLIDER_NEGATIVE;
            }
            else if (movement.y > 0)
            {
                transform.localPosition = Constants.SWORD_UP;
                transform.localRotation = Constants.SWORD_Y_ROTATION;
                Renderer.flipY = false;
                Collider.offset = Constants.SWORD_COLLIDER_POSITIVE;
            }
            else if (movement.y < 0 || movement == Vector3.zero)
            {
                transform.localPosition = Constants.SWORD_DOWN;
                transform.localRotation = Constants.SWORD_Y_ROTATION;
                Renderer.flipY = true;
                Collider.offset = Constants.SWORD_COLLIDER_NEGATIVE;
            }
        }

        public void SetSword(Base.Item item)
        {
            SwordType = item.Type;
            Damage = SwordType.GetAttribute<DamageAttribute>();
        }

        public float GetDamage()
        {
            return Constants.BASE_SWORD_POWER;
        }

        public float GetDamageModifier()
        {
            return Damage.WeaponDamage;
        }
    }
}