using System;
using System.Linq;
using UnityEngine;


public class ClicknDropItem : Item
{
    private Transform erzatzItem;

    private Garage garage;

    private void Start()
    {
        garage = GameObject.FindObjectOfType<Garage>();
    }

    private void OnMouseDown()
    {
        if (state == State.OnStore)
        {
            AudioManager.instance.PlayBoxTake();

            if (erzatzItem != null)
            {
                TryToPlaceItem();
                return;
            }

            state = State.StartMoving;
        }
        else if (state == State.IsMoving)
        {
            AudioManager.instance.PlayBoxPut();

            erzatzItem.position = gameObject.transform.position;
            erzatzItem.parent = garage.selectedCar.transform;
            Destroy(gameObject);
        }
    }

    private void TryToPlaceItem()
    {
        Ray ray = GameManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.GetComponent<Grid>()
                .SetValue(GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition),
                    erzatzItem.GetComponent<Item>());
        }

        Debug.Log(12);
    }

    private void OnMouseUp()
    {
    }

    private void Update()
    {
        switch (state)
        {
            case State.OnStore:
                break;
            case State.StartMoving:
                StartMove();
                break;
            case State.IsMoving:
                ChangePostion();
                break;
            case State.EndMoving:
                break;
            case State.InCar:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangePostion()
    {
        if (erzatzItem == null)
            return;
        Vector3 newPos = erzatzItem.transform.position;
        newPos.x = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        newPos.y = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        newPos.z = 0;
        transform.position = newPos;
    }

    private void StartMove()
    {
        //TODO check if we can drag
        Debug.Log("Start Drag");
        state = State.IsMoving;

        erzatzItem = Instantiate(gameObject).transform;
        erzatzItem.parent = transform.parent;
        erzatzItem.transform.position = transform.position;
        erzatzItem.transform.localPosition = transform.localPosition;
        erzatzItem.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;

        GetComponent<BoxCollider2D>().isTrigger = true;
        var replacerRb = GetComponent<Rigidbody2D>();
        replacerRb.SetRotation(Quaternion.Euler(Vector3.zero));
        replacerRb.freezeRotation = true;

        var parsedName = name.Split('_')[1].Split('(')[0].Split('x').Select(int.Parse).ToArray();
        var boxSize = new Vector2(parsedName[0], parsedName[1]);
        GameManager.Instance.FlyingItem = transform;

        erzatzItem.GetComponent<ClicknDropItem>().state = State.IsMoving;
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a = 0.5f;
        GetComponent<SpriteRenderer>().color = newColor;
    }
}