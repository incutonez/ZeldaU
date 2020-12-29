using UnityEngine;

public class Matter
{
    public Matters type = Matters.None;
    public WorldColors? color { get; set; }
    /// <summary>
    /// This is an override for the CanEnter method... by default, we have certain Matters that are considered
    /// something the player can enter, but there are some scenarios where that's not always true
    /// </summary>
    public bool? canEnter;
    public bool canMove = false;
    public bool canBurn = false;
    public bool canBomb = false;

    public Sprite GetSprite()
    {
        return GameHandler.SceneBuilder.LoadSprite(type.GetDescription());
    }

    public bool CanEnter()
    {
        if (canEnter.HasValue)
        {
            return canEnter.Value;
        }
        switch (type)
        {
            case Matters.stairsDown:
            case Matters.stairsUp:
            case Matters.dock:
            case Matters.door:
            case Matters.sandB:
            case Matters.sandBL:
            case Matters.sandBR:
            case Matters.sandC:
            case Matters.sandCL:
            case Matters.sandCR:
            case Matters.sandT:
            case Matters.sandTL:
            case Matters.sandTR:
                return true;
        }
        return false;
    }

    public bool IsTransition()
    {
        return type == Matters.Transition;
    }

    public Color GetColor()
    {
        if (IsTransition())
        {
            return Constants.COLOR_INVISIBLE;
        }
        string hex = color.HasValue ? color.GetDescription() : WorldColors.Tan.GetDescription();
        return Utilities.HexToColor(hex);
    }
}
