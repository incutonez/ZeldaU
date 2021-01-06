using Audio;
using UnityEngine;

namespace Manager
{
    public class Sword : MonoBehaviour
    {
        public new bool enabled = false;

        private RectTransform swordTransform;
        private SpriteRenderer swordRenderer;
        private Items swordType;
        private DamageAttribute damage;
        private BoxCollider2D swordCollider;

        public void Awake()
        {
            swordTransform = GetComponent<RectTransform>();
            swordRenderer = GetComponent<SpriteRenderer>();
            swordCollider = GetComponent<BoxCollider2D>();
            ToggleSword(false);
        }

        public void Swing(Vector3 playerPosition)
        {
            Game.Shield.ToggleShields(false);
            ToggleSword(true);
            SetPosition(playerPosition);
            // TODOJEF: Have to determine which Sword FX should play... need to know if at full health
            Game.Audio.PlayFX(FX.SwordSlash);
        }

        public void ToggleSword(bool enabled = false)
        {
            swordTransform.gameObject.SetActive(enabled);
        }

        public void SetPosition(Vector3 movement)
        {
            if (movement.x > 0)
            {
                swordTransform.localPosition = Constants.SWORD_RIGHT;
                swordTransform.localRotation = Constants.SWORD_X_ROTATION;
                swordRenderer.flipY = false;
                swordCollider.offset = Constants.SWORD_COLLIDER_POSITIVE;
            }
            else if (movement.x < 0)
            {
                swordTransform.localPosition = Constants.SWORD_LEFT;
                swordTransform.localRotation = Constants.SWORD_X_ROTATION;
                swordRenderer.flipY = true;
                swordCollider.offset = Constants.SWORD_COLLIDER_NEGATIVE;
            }
            else if (movement.y > 0)
            {
                swordTransform.localPosition = Constants.SWORD_UP;
                swordTransform.localRotation = Constants.SWORD_Y_ROTATION;
                swordRenderer.flipY = false;
                swordCollider.offset = Constants.SWORD_COLLIDER_POSITIVE;
            }
            else if (movement.y < 0 || movement == Vector3.zero)
            {
                swordTransform.localPosition = Constants.SWORD_DOWN;
                swordTransform.localRotation = Constants.SWORD_Y_ROTATION;
                swordRenderer.flipY = true;
                swordCollider.offset = Constants.SWORD_COLLIDER_NEGATIVE;
            }
        }

        public void SetSword(Base.Item item)
        {
            swordType = item.Type;
            damage = swordType.GetAttribute<DamageAttribute>();
        }

        public float GetDamage()
        {
            return Constants.BASE_SWORD_POWER;
        }

        public float GetDamageModifier()
        {
            return damage.WeaponDamage;
        }
    }
}