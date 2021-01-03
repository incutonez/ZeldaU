using UnityEngine;

namespace Manager
{
    public class Prefabs
    {
        public RectTransform WorldDoor { get; set; }
        public RectTransform WorldTransition { get; set; }
        public RectTransform WorldScreen { get; set; }
        public RectTransform Enemy { get; set; }
        public RectTransform NPC { get; set; }
        public RectTransform Item { get; set; }
        public RectTransform Player { get; set; }
        public RectTransform UIHeart { get; set; }

        public Prefabs()
        {
            LoadAll();
        }

        public void LoadAll()
        {
            WorldDoor = LoadPrefab("WorldDoor");
            WorldTransition = LoadPrefab("WorldTransition");
            WorldScreen = LoadPrefab("WorldScreen");
            Enemy = LoadPrefab("Enemy");
            NPC = LoadPrefab("NPC");
            Item = LoadPrefab("Item");
            Player = LoadPrefab("Character");
            UIHeart = LoadPrefab("HeartTemplate");
        }

        public RectTransform LoadPrefab(string name)
        {
            return Resources.Load<RectTransform>($"{Constants.PATH_PREFABS}{name}");
        }
    }
}