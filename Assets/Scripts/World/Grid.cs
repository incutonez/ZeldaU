using System;
using UnityEngine;

namespace World {
  /// <summary>
  /// Idea taken from https://www.youtube.com/watch?v=waEsGu--9P8
  /// </summary>
  public class Grid<T> {
    public const int HeatmapMax = 100;
    public const int HeatmapMin = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

    public class OnGridValueChangedEventArgs : EventArgs {
      public int X;
      public int Y;
    }

    public int Width { get; set; }
    public int Height { get; set; }
    public float CellSize { get; set; }
    public T[,] Cells { get; set; }
    public TextMesh[,] CellsText { get; set; }
    public Vector3 Origin { get; set; }
    public bool DebugMode { get; set; } = false;

    public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<T>, int, int, T> createFunc) {
      Width = width;
      Height = height;
      CellSize = cellSize;
      Cells = new T[width, height];
      CellsText = new TextMesh[width, height];
      Origin = origin;

      for (int i = 0; i < Cells.GetLength(0); i++) {
        for (int j = 0; j < Cells.GetLength(1); j++) {
          Cells[i, j] = createFunc(this, i, j);
        }
      }
    }

    public void EachCell(Action<T, int, int> func) {
      for (int x = 0; x < Width; x++) {
        for (int y = 0; y < Height; y++) {
          func(GetViewModel(x, y), x, y);
        }
      }
    }

    public Vector3 GetWorldPosition(float x, float y) {
      return new Vector3(x, y) * CellSize + Origin;
    }

    /// <summary>
    /// This will return the x value only from GetWorldPosition... needed for times when our player is panning from
    /// one screen to the other
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public float GetWorldPositionX(float x) {
      return GetWorldPosition(x, 0f).x;
    }

    /// <summary>
    /// This will return the y value only from GetWorldPosition... needed for times when our player is panning from
    /// one screen to the other
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public float GetWorldPositionY(float y) {
      return GetWorldPosition(0f, y).y;
    }

    public void GetXY(Vector3 position, out int x, out int y) {
      x = Mathf.FloorToInt((position - Origin).x / CellSize);
      y = Mathf.FloorToInt((position - Origin).y / CellSize);
      if (x < 0) {
        x = 0;
      }

      if (y < 0) {
        y = 0;
      }
    }

    public void TriggerChange(int x, int y) {
      if (OnGridValueChanged != null) {
        OnGridValueChanged(this, new OnGridValueChangedEventArgs {X = x, Y = y});
      }
    }

    public void SetValue(int x, int y, T value) {
      if (IsValid(x, y)) {
        Cells[x, y] = value;
        if (DebugMode) {
          CellsText[x, y].text = Cells[x, y]?.ToString();
        }

        if (OnGridValueChanged != null) {
          OnGridValueChanged(this, new OnGridValueChangedEventArgs {X = x, Y = y});
        }
      }
    }

    public bool IsValid(int x, int y) {
      return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public T GetViewModel(int x, int y) {
      if (IsValid(x, y)) {
        return Cells[x, y];
      }

      return default(T);
    }

    public T GetViewModel(Vector3 position) {
      int x;
      int y;
      GetXY(position, out x, out y);
      return GetViewModel(x, y);
    }
  }
}
