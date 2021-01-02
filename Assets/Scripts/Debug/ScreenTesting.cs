using UnityEngine;

public class ScreenTesting : MonoBehaviour
{
    public Screen ScreenTileVisual;

    private ScreenTile ScreenTile { get; set; }

    private void Start()
    {
        //ScreenTile = new ScreenTile(Constants.GRID_COLUMNS, Constants.GRID_ROWS, 1f, new Vector3(-8f, -7.5f));
        //ScreenTileVisual.SetGrid(ScreenTile.Grid);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    ScreenTile.SetTileMatterType(position, Matters.bush);
        //}
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    ScreenTile.SetTileMatterType(position, Matters.grave);
        //}
    }
}
