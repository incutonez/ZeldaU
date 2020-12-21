using UnityEngine;

public class Matter
{
    public Matters type;
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
}
