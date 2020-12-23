using UnityEngine;

public class Matter
{
    public Matters type;
    public WorldColors? color;
    /// <summary>
    /// This is only used for Transition matters... it's the x value for the next screen... if this is a positive value, then
    /// that means this transition is on the right side of the screen, and if it's negative, then it's on the left side
    /// </summary>
    public int transitionX;
    /// <summary>
    /// This is only used for Transition matters... it's the y value for the next screen... if this is a positive value, then
    /// that means this transition is on the top of the screen, and if it's negative, then it's on the bottom
    /// </summary>
    public int transitionY;
    public bool canMove = false;
    public bool canBurn = false;
    public bool canBomb = false;

    public Sprite GetSprite()
    {
        return GameHandler.sceneBuilder.LoadSprite(type.GetDescription());
    }

    public bool CanEnter()
    {
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
        return GameHandler.sceneBuilder.HexToColor(hex);
    }
}
