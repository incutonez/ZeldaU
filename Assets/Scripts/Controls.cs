using UnityEngine;

public static class Controls
{
    public static readonly string INVENTORY = "Inventory";
    public static readonly string HORIZONTAL = "Horizontal";
    public static readonly string VERTICAL = "Vertical";

    public static bool IsRightKey()
    {
        return Input.GetButton(HORIZONTAL) && Input.GetAxisRaw(HORIZONTAL) > 0;
    }

    public static bool IsLeftKey()
    {
        return Input.GetButton(HORIZONTAL) && Input.GetAxisRaw(HORIZONTAL) < 0;
    }
    public static bool IsUpKey()
    {
        return Input.GetButton(VERTICAL) && Input.GetAxisRaw(VERTICAL) > 0;
    }

    public static bool IsDownKey()
    {
        return Input.GetButton(VERTICAL) && Input.GetAxisRaw(VERTICAL) < 0;
    }

    public static bool IsInventoryKeyDown()
    {
        return Input.GetButtonDown(INVENTORY);
    }
}
