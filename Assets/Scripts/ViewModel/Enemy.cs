using NPCs;

namespace ViewModel {
  public class Enemy {
    public int Count { get; set; } = 1;
    public Enemies Type { get; set; }
    public float Speed { get; set; } = 3f;
    public float X { get; set; } = 0f;
    public float Y { get; set; } = 0f;
  }
}
