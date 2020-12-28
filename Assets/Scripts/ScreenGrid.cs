using System;
using UnityEngine;

/// <summary>
/// Idea taken from https://www.youtube.com/watch?v=waEsGu--9P8
/// </summary>
// TODOJEF: DO https://www.youtube.com/watch?v=8jrAWtI8RXg
// After this go back to https://www.youtube.com/watch?v=alU04hvz6L4
// And then https://www.youtube.com/watch?v=db0KWYaWfeM
public class ScreenGrid<T>
{
    public const int HEATMAP_MAX = 100;
    public const int HEATMAP_MIN = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    public int Width { get; set; }
    public int Height { get; set; }
    public float CellSize { get; set; }
    public T[,] Cells { get; set; }
    public TextMesh[,] CellsText { get; set; }
    public Vector3 Origin { get; set; }
    public bool DebugMode { get; set; } = false;

    public ScreenGrid(int width, int height, float cellSize, Vector3 origin, Func<ScreenGrid<T>, int, int, T> createFunc)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        Cells = new T[width, height];
        CellsText = new TextMesh[width, height];
        Origin = origin;

        for (int i = 0; i < Cells.GetLength(0); i++)
        {
            for (int j = 0; j < Cells.GetLength(1); j++)
            {
                Cells[i, j] = createFunc(this, i, j);
            }
        }

        if (DebugMode)
        {
            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    CellsText[i, j] = CreateWorldText(Cells[i, j]?.ToString(), null, GetWorldPosition(i, j) + new Vector3(CellSize, CellSize) * 0.5f);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                CellsText[eventArgs.x, eventArgs.y].text = Cells[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize + Origin;
    }

    public void GetXY(Vector3 position, out int x, out int y)
    {
        x = Mathf.FloorToInt((position - Origin).x / CellSize);
        y = Mathf.FloorToInt((position - Origin).y / CellSize);
    }

    public void TriggerChange(int x, int y)
    {
        if (OnGridValueChanged != null)
        {
            OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void SetValue(int x, int y, T value)
    {
        if (IsValid(x, y))
        {
            Cells[x, y] = value;
            if (DebugMode)
            {
                CellsText[x, y].text = Cells[x, y]?.ToString();
            }
            if (OnGridValueChanged != null)
            {
                OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            }
        }
    }

    public bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public void SetValue(Vector3 position, T value)
    {
        int x;
        int y;
        GetXY(position, out x, out y);
        SetValue(x, y, value);
    }

    public T GetValue(int x, int y)
    {
        if (IsValid(x, y))
        {
            return Cells[x, y];
        }
        return default(T);
    }

    public T GetValue(Vector3 position)
    {
        int x;
        int y;
        GetXY(position, out x, out y);
        return GetValue(x, y);
    }

    // TODOJEF: THROWAWAY
    // text, parent, localPosition, fontSize, color, anchor, alignment, sortingOrder
    public TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition)
    {
        GameObject gameObject = new GameObject("BLAH", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh mesh = gameObject.GetComponent<TextMesh>();
        mesh.text = text;
        mesh.alignment = TextAlignment.Left;
        mesh.fontSize = 30;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.color = Color.white;
        return mesh;
    }
}
