using NPCs;
using System;

[Serializable]
public class Enemy : BaseCharacter
{
    public bool CanBomb()
    {
        switch (characterType)
        {
            // TODO: Fill out rest of enemies
            case Enemies.Armos:
                return true;
        }
        return false;
    }

    public bool CanArrow()
    {
        switch (characterType)
        {
            // TODO: Fill out rest of enemies
            case Enemies.Armos:
                return true;
        }
        return false;
    }

    public bool CanWand()
    {
        switch (characterType)
        {
            case Enemies.Armos:
                return true;
        }
        return false;
    }

    public bool CanCandle()
    {
        switch (characterType)
        {
            case Enemies.Armos:
                return true;
        }
        return false;
    }

    public bool CanBoomerang()
    {
        switch (characterType)
        {
            case Enemies.Armos:
                return true;
        }
        return false;
    }
}