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
        
        gridGO = new GameObject("Grid");
        gridGO.transform.position = Vector3.zero;
        
    }

    void DrawCell(Vector3 pos, int size)
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}