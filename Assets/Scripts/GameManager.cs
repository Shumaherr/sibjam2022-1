using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Vector2Int inventorySize;
    [SerializeField] private Transform cellPrefab;
    private Grid grid;
    private GameObject gridGO;


    [SerializeField] private Camera mainCamera;

    public Camera MainCamera => mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(inventorySize.x, inventorySize.y, 1, Vector3.zero);
        gridGO = new GameObject("Grid");
        gridGO.transform.position = Vector3.zero;
        for (int i = 0; i < grid.Width; i++)
        {
            for (int j = 0; j < grid.Height; j++)
            {
                var cell = Instantiate(cellPrefab, new Vector3(i, j, 0), Quaternion.identity, gridGO.transform);
                cell.parent = gridGO.transform;
            }
        }
    }

    void DrawCell(Vector3 pos, int size)
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}