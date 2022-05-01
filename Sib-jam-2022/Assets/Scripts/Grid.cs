using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] grid;
    private Vector3 origin;
    public Grid(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        grid = new int[width, height];
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize;
    }
    
    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / cellSize), Mathf.FloorToInt(worldPosition.z / cellSize));
    }
    
    public void SetValue(int x, int y, int value)
    {
        if (x < 0 && x >= width && y < 0 && y >= height)
            return;
        grid[x, y] = value;
    }
    
    public void SetValue(Vector3 position, int value)
    {
        SetValue(GetGridPosition(position).x, GetGridPosition(position).y, value);
    }
    
}
