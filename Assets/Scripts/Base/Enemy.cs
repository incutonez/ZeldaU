using NPCs;
using System;

namespace Base
{
    [Serializable]
    public class Enemy : Character
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

        public void SetFrameRates(Base.Animation animator)
        {
            float action = 0f;
            float idle = 0f;
            float walk = 0f;
            switch (characterType)
            {
                case Enemies.OctorokBlue:
                case Enemies.Octorok:
                    walk = 0.3f;
                    action = 0.33f;
                    idle = 1f;
                    break;
            }
            animator.ActionFrameRate = action;
            animator.IdleFrameRate = idle;
            animator.WalkFrameRate = walk;
        }
    }

}