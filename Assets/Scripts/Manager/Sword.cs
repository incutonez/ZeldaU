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

        // We use LateUpdate because the LastMovement property gets set in the Update of Base.Movement, so we want to ensure we have the latest value
        private void LateUpdate()
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
                transform.localPosition = Constants.SwordRight;
                transform.localRotation = Constants.SwordXRotation;
                Renderer.flipY = false;
                Collider.offset = Constants.SwordColliderPositive;
            }
            else if (movement.x < 0)
            {
                transform.localPosition = Constants.SwordLeft;
                transform.localRotation = Constants.SwordXRotation;
                Renderer.flipY = true;
                Collider.offset = Constants.SwordColliderNegative;
            }
            else if (movement.y > 0)
            {
                transform.localPosition = Constants.SwordUp;
                transform.localRotation = Constants.SwordYRotation;
                Renderer.flipY = false;
                Collider.offset = Constants.SwordColliderPositive;
            }
            else if (movement.y < 0 || movement == Vector3.zero)
            {
                transform.localPosition = Constants.SwordDown;
                transform.localRotation = Constants.SwordYRotation;
                Renderer.flipY = true;
                Collider.offset = Constants.SwordColliderNegative;
            }
        }

        public void SetSword(Base.Item item)
        {
            SwordType = item.Type;
            Damage = SwordType.GetAttribute<DamageAttribute>();
        }

        public float GetDamage()
        {
            return Constants.BaseSwordPower;
        }

        public float GetDamageModifier()
        {
            return Damage.WeaponDamage;
        }
    }
}