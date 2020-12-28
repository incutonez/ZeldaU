using UnityEngine;

// TODOJEF: https://www.youtube.com/watch?v=gD5EQyt7VPk&list=PLzDRvYVwl53uhO8yhqxcyjDImRjO9W722&index=5
public class Testing : MonoBehaviour
{
    private ScreenGrid<HeatMapGridObject> Grid { get; set; }

    public HeatmapVisual HeatMap;
    public BoolVisual HeatMapBool;
    public GenericVisual GenericVisual;

    private void Start()
    {
        Grid = new ScreenGrid<HeatMapGridObject>(20, 10, 6f, Vector3.zero, (ScreenGrid<HeatMapGridObject> grid, int x, int y) => new HeatMapGridObject(grid, x, y));
        //HeatMap.SetGrid(Grid);
        //HeatMapBool.SetGrid(Grid);
        GenericVisual.SetGrid(Grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HeatMapGridObject obj = Grid.GetValue(position);
            if (obj != null)
            {
                obj.AddValue(5);
            }
        }
    }
}

public class HeatMapGridObject
{
    public const int MIN = 0;
    public const int MAX = 100;
    public int Value { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ScreenGrid<HeatMapGridObject> Grid { get; set; }

    public HeatMapGridObject(ScreenGrid<HeatMapGridObject> grid, int x, int y)
    {
        Grid = grid;
        X = x;
        Y = y;
    }

    public void AddValue(int value)
    {
        Value += value;
        Value = Mathf.Clamp(Value, MIN, MAX);
        Grid.TriggerChange(X, Y);
    }

    public float GetNormalizedValue()
    {
        return (float)Value / MAX;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
