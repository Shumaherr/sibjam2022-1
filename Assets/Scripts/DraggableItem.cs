using System;
using UnityEngine;

public class DraggableItem : Item
{

    DraggableItem()
    {
        state = State.OnStore;
    }

    private void OnMouseDown()
    {
        state = State.StartMoving;
    }
    
    private void OnMouseUp()
    {
        state = State.EndMoving;
    }

    private void Update()
    {
        switch (state)
        {
            case State.OnStore:
                break;
            case State.StartMoving:
                StartDrag();
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
        Vector3 newPos = transform.position;
        newPos.x = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        newPos.y = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        newPos.z = 0;
        transform.position = newPos;
    }

    private void StartDrag()
    {
        //TODO check if we can drag
        Debug.Log("Start Drag");
        state = State.IsMoving;
    }
}