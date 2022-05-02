using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid: MonoBehaviour
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize;
    [SerializeField] private Transform cellPrefab;
    
    private Item[,] grid;

    private void Awake()
    {
        grid = new Item[gridSize.x, gridSize.y];
        GenerateGrid();
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + origin;
    }

    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / cellSize),
            Mathf.FloorToInt(worldPosition.z / cellSize));
    }

    public void SetValue(int x, int y, Item value)
    {
        if (x < 0 && x >= gridSize.x && y < 0 && y >= gridSize.y)
            return;
        grid[x, y] = value;
    }

    public void SetValue(Vector3 position, Item value)
    {
        SetValue(GetGridPosition(position).x, GetGridPosition(position).y, value);
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var cell = Instantiate(cellPrefab, GetWorldPosition(x, y), Quaternion.identity);
                cell.transform.parent = transform;
                cell.name = $"Cell {x}, {y}";
                SetValue(x, y, cell.GetComponent<Item>());
            }
        }
    }
}