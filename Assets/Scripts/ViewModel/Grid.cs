using NPCs;
using System.Collections.Generic;

namespace ViewModel
{
    public class Grid
    {
        /// <summary>
        /// This is only used for Transition matters... it's the actual name of the transition that you'd like to load.  This is
        /// useful for things that are not in the overworld/underworld, like shops.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// This is only used for Transition matters... it's the x value for the next screen... if this is a positive value, then
        /// that means this transition is on the right side of the screen, and if it's negative, then it's on the left side
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// This is only used for Transition matters... it's the y value for the next screen... if this is a positive value, then
        /// that means this transition is on the top of the screen, and if it's negative, then it's on the bottom
        /// </summary>
        public int Y { get; set; }
        public WorldColors? AccentColor { get; set; }
        public WorldColors? GroundColor { get; set; }
        public List<Tile> Tiles { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Character> Characters { get; set; }
        public List<ItemViewModel> Items { get; set; }
    }

    public class ItemViewModel
    {
        public List<float> Coordinates { get; set; }
        public Base.Item Item { get; set; }
    }

    public class Character
    {
        public List<float> Coordinates { get; set; }
        public Characters Type { get; set; }
    }

    public class Enemy
    {
        List<EnemyChild> Children { get; set; }
        public int Count { get; set; }
        public Enemies Type { get; set; }
    }

    public class EnemyChild
    {
        public List<float> Coordinates { get; set; }
        public World.Enemy Enemy { get; set; }
    }

    public class Tile
    {
        public WorldColors? AccentColor { get; set; }
        public List<TileChild> Children { get; set; }
        public Tiles Type { get; set; }
    }

    public class TileChild
    {
        /// <summary>
        /// This is a list of x, y coordinates, and if 4 values are specified, it becomes the max x, y range to keep adding this matter type
        /// </summary>
        public List<float> Coordinates { get; set; }
        public Tiles TileType { get; set; } = Tiles.None;
        public Grid Transition { get; set; }
    }
}