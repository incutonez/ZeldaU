using NPCs;

namespace World
{
    public class NPC : Character<BaseCharacter>
    {
        public override void SetAnimationBase()
        {
            Animator = gameObject.AddComponent<AnimatorBase>();
            Animator.AnimationSprites = Manager.Game.Sprites.NPCAnimations[(Characters)BaseCharacter.characterType];
        }
    }
}
