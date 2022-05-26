using Enums;

namespace World
{
    public class NPC : Character<Characters>
    {
        protected override void SetAnimationBase()
        {
            Animation = gameObject.AddComponent<Base.Animation>();
            Animation.AnimationSprites = Manager.Game.Graphics.NpcAnimations[CharacterType];
        }
    }
}
