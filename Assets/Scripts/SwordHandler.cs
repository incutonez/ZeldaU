using Audio;
using System.Collections;
using UnityEngine;

public class SwordHandler : MonoBehaviour
{
    public new bool enabled = false;

    private Sprite swordSprite;
    private RectTransform swordTransform;
    private SpriteRenderer swordRenderer;
    private WorldObjectData worldObjectData;
    private Items swordType;
    private DamageAttribute damage;

    public void Awake()
    {
        swordTransform = GetComponent<RectTransform>();
        swordRenderer = GetComponent<SpriteRenderer>();
        worldObjectData = GetComponent<WorldObjectData>();
    }

    public IEnumerator Swing(Vector3 playerPosition)
    {
        GameHandler.shieldHandler.ToggleShields(false);
        ToggleSword(true);
        SetPosition(playerPosition);
        // TODOJEF: Have to determine which Sword FX should play... need to know if at full health
        AudioManager.Instance.PlayFX(FX.SwordSlash);
        yield return new WaitForSeconds(Constants.ATTACK_LENGTH);
        ToggleSword(false);
    }

    public void ToggleSword(bool enabled = false)
    {
        this.enabled = enabled;
        worldObjectData.SetObjectSize(enabled ? swordSprite.bounds.size : Vector3.zero);
        swordRenderer.enabled = enabled;
    }

    public void SetPosition(Vector3 movement)
    {
        if (movement.x > 0)
        {
            swordTransform.localPosition = Constants.SWORD_RIGHT;
            swordTransform.localRotation = Constants.SWORD_X_ROTATION;
            swordRenderer.flipY = false;
        }
        else if (movement.x < 0)
        {
            swordTransform.localPosition = Constants.SWORD_LEFT;
            swordTransform.localRotation = Constants.SWORD_X_ROTATION;
            swordRenderer.flipY = true;
        }
        else if (movement.y > 0)
        {
            swordTransform.localPosition = Constants.SWORD_UP;
            swordTransform.localRotation = Constants.SWORD_Y_ROTATION;
            swordRenderer.flipY = false;
        }
        else if (movement.y < 0 || movement == Vector3.zero)
        {
            swordTransform.localPosition = Constants.SWORD_DOWN;
            swordTransform.localRotation = Constants.SWORD_Y_ROTATION;
            swordRenderer.flipY = true;
        }
    }

    public void SetSword(Item item)
    {
        swordType = item.itemType;
        swordSprite = item?.GetSprite();
        worldObjectData.SetObjectData(swordSprite, false);
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
