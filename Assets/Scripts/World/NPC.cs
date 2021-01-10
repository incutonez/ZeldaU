using NPCs;

namespace World
{
    public class NPC : Character<Characters>
    {
        public override void SetAnimationBase()
        {
            Animation = gameObject.AddComponent<Base.Animation>();
            Animation.AnimationSprites = Manager.Game.Graphics.NPCAnimations[CharacterType];
        }
    }
}
