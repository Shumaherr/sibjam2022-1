using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform grid;
    private Grid gridComponent;
    private GameObject gridGO;
    private Transform flyingItem;

    public Transform FlyingItem
    {
        get => flyingItem;
        set => flyingItem = value;
    }


    [SerializeField] private Camera mainCamera;

    public Camera MainCamera => mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        gridComponent = grid.GetComponent<Grid>();
    }

    void DrawCell(Vector3 pos, int size)
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (FlyingItem != null)
        {
            var groundPlane = new Plane(Vector3.forward, Vector3.zero);
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                if (!IsPointInGrid(worldPosition))
                    return;
                int x = RoundToValue(worldPosition.x, gridComponent.CellSize * 2);
                int y = RoundToValue(worldPosition.y, gridComponent.CellSize * 2);


                FlyingItem.transform.position = new Vector3(x, y, 0);

                /*if (Input.GetMouseButtonDown(0))
                {
                    PlaceBuilding(x, y);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(buildingToPlace.gameObject);
                    buildingToPlace = null;
                }*/
            }
        }
    }

    public static int RoundToValue(float d, int value = 5) =>
        Mathf.RoundToInt(d / value) * value;

    private bool IsPointInGrid(Vector3 point)
    {
        if (point.x > gridComponent.GetLeftBound() && point.x < gridComponent.GetRightBound() &&
            point.y > gridComponent.GetBottomBound() && point.y < gridComponent.GetTopBound())
        {
            return true;
        }

        return false;
    }

    public void BuyCar(string name, int price)
    {
        var cashText = GameObject.Find("CashText").GetComponent<Text>();
        var cash = int.Parse(cashText.text.Split(' ')[1]);
        if (cash >= price)
        {
            FindObjectOfType<Garage>().UnlockTransport(name);
            cashText.text = "Деньги " + (cash - price);
        }
    }
}