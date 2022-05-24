using System;
using System.Collections.Generic;
using NPCs;
using UnityEngine;

namespace ViewModel {
  public class Enemy {
    public int Count { get; set; } = 1;
    public Enemies Type { get; set; }
    public float Speed { get; set; } = 3f;
    public float X { get; set; } = 0f;
    public float Y { get; set; } = 0f;
    /// <summary>
    /// This comes in as pairs... the first color is the target, and the second is the replacement.
    /// If a 3rd color is specified, then that's the 2nd target color, and so on.
    /// </summary>
    public WorldColors[] Colors { get; set; }
  }
}
