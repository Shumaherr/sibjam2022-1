using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;

    public int Width => width;

    public int Height => height;

    public float CellSize => cellSize;

    private int height;
    private float cellSize;
    private Item[,] grid;
    private Vector3 origin;
    public Grid(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        grid = new Item[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Debug.DrawLine(GetWorldPosition(0, this.height), GetWorldPosition(this.width, this.height), Color.red, 0);
                Debug.DrawLine(GetWorldPosition(this.width, 0), GetWorldPosition(this.width, this.height), Color.red, 0);

            }
        }
          }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize;
    }
    
    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / cellSize), Mathf.FloorToInt(worldPosition.z / cellSize));
    }
    
    public void SetValue(int x, int y, Item value)
    {
        if (x < 0 && x >= width && y < 0 && y >= height)
            return;
        grid[x, y] = value;
    }
    
    public void SetValue(Vector3 position, Item value)
    {
        SetValue(GetGridPosition(position).x, GetGridPosition(position).y, value);
    }
    
}
