using UnityEngine;

public class ShieldHandler : MonoBehaviour
{
    public SpriteRenderer shield;
    public SpriteRenderer shieldRight;
    public SpriteRenderer shieldLeft;
    public new RectTransform transform;

    private const float xPos = -0.035f;
    private Sprite shieldSprite;
    private Sprite shieldMagicalSprite;
    private Sprite shieldSideSprite;
    private Sprite shieldMagicalSideSprite;

    private void Awake()
    {
        shield.sprite = null;
        // Cache our images, so we're not loading them every time
        shieldSprite = ItemManager.Instance.LoadSpriteByItemType(Items.Shield);
        shieldSideSprite = ItemManager.Instance.LoadSprite("ShieldSide");
        shieldMagicalSprite = ItemManager.Instance.LoadSpriteByItemType(Items.ShieldMagical);
        shieldMagicalSideSprite = ItemManager.Instance.LoadSprite("ShieldMagicalSide");
    }

    public void ToggleShields(bool enabled = false, bool? rightShield = null, bool? leftShield = null)
    {   
        // Idea from https://gamedev.stackexchange.com/questions/125464/multiple-sprite-animation-layers-overlayed-in-unity-animator
        // If we were last moving down or we haven't moved at all
        ToggleShield(enabled);
        ToggleRightShield(rightShield.HasValue ? rightShield.Value : enabled);
        ToggleLeftShield(leftShield.HasValue ? leftShield.Value : enabled);
    }

    public void ToggleShield(bool enabled = false)
    {
        shield.enabled = enabled;
    }

    public void ToggleRightShield(bool enabled = false)
    {
        shieldRight.enabled = enabled;
    }

    public void ToggleLeftShield(bool enabled = false)
    {
        shieldLeft.enabled = enabled;
    }

    public void SetShield(Items itemType)
    {
        if (itemType == Items.Shield)
        {
            transform.localPosition = new Vector3(xPos, -0.037f);
            shield.sprite = shieldSprite;
            shieldLeft.sprite = shieldSideSprite;
            shieldRight.sprite = shieldSideSprite;
        }
        else if (itemType == Items.ShieldMagical)
        {
            transform.localPosition = new Vector3(xPos, -0.03f);
            shield.sprite = shieldMagicalSprite;
            shieldLeft.sprite = shieldMagicalSideSprite;
            shieldRight.sprite = shieldMagicalSideSprite;
        }
    }
}
