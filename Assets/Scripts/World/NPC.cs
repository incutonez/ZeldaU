using NPCs;

namespace World
{
    public class NPC : Character<Base.Character>
    {
        public override void SetAnimationBase()
        {
            Animation = gameObject.AddComponent<Base.Animation>();
            Animation.AnimationSprites = Manager.Game.Sprites.NPCAnimations[(Characters)BaseCharacter.characterType];
        }
    }
}
