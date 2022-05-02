using System;
using UnityEngine;


public class ClicknDropItem : Item
{
    private Transform flyingItem;

    private void OnMouseDown()
    {
        if (flyingItem != null)
        {
            TryToPlaceItem();
            return;
        }

        state = State.StartMoving;
    }

    private void TryToPlaceItem()
    {
        Ray ray = GameManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.GetComponent<Grid>()
                .SetValue(GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition),
                    flyingItem.GetComponent<Item>());
        }
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
        if (flyingItem == null)
            return;
        Vector3 newPos = flyingItem.transform.position;
        newPos.x = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        newPos.y = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        newPos.z = 0;
        flyingItem.transform.position = newPos;
    }

    private void StartMove()
    {
        //TODO check if we can drag
        Debug.Log("Start Drag");
        state = State.IsMoving;
        flyingItem = Instantiate(gameObject).transform;
        Color newColor = flyingItem.GetComponent<SpriteRenderer>().color;
        newColor.a = 0.5f;
        flyingItem.GetComponent<SpriteRenderer>().color = newColor;
    }
}