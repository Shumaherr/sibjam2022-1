using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private int cellSize;

    private Vector3 parent;

    public int CellSize
    {
        get => cellSize;
        set => cellSize = value;
    }

    [SerializeField] private Transform cellPrefab;

    private Item[,] grid;

    private void Awake()
    {
        parent = transform.parent.position;

        grid = new Item[gridSize.x, gridSize.y];
        cellPrefab.localScale = new Vector3(cellSize, cellSize, cellSize);
        GenerateGrid();
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize * 2.5f + origin;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
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
        for (var i = gridSize.x - 1; i >= 0; i--)
        {
            for (int j = gridSize.y - 1; j >= 0; j--)
            {
                var x = parent.x + i * cellSize * 2.5f;
                var y = parent.y + j * cellSize * 2.5f;
                var z = parent.z;
                var position = new Vector3(x, y, z);
                var cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.transform.parent = transform;
                cell.name = $"Cell {i}, {j}";
                SetValue(i, j, cell.GetComponent<Item>());
            }
        }
    }

    public float GetLeftBound()
    {
        return GetWorldPosition(0, 0).x;
    }

    public float GetRightBound()
    {
        return GetWorldPosition(gridSize.x - 1, gridSize.y - 1).x;
    }

    public float GetTopBound()
    {
        return GetWorldPosition(gridSize.x - 1, gridSize.y - 1).y;
    }

    public float GetBottomBound()
    {
        return GetWorldPosition(0, 0).y;
    }
}