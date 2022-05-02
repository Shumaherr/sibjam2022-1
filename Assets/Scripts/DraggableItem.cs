using System;
using UnityEngine;

internal enum State
{
    OnStore,
    StartDrag,
    IsDragging,
    EndDrag,
    InCar
}
public class DraggableItem : Item
{
    private State state;
    

    DraggableItem()
    {
        state = State.OnStore;
    }

    private void OnMouseDown()
    {
        state = State.StartDrag;
    }
    
    private void OnMouseUp()
    {
        state = State.EndDrag;
    }

    private void Update()
    {
        switch (state)
        {
            case State.OnStore:
                break;
            case State.StartDrag:
                StartDrag();
                break;
            case State.IsDragging:
                ChangePostion();
                break;
            case State.EndDrag:
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
        state = State.IsDragging;
    }
}