using UnityEngine;

public enum State
{
    OnStore,
    StartMoving,
    IsMoving,
    EndMoving,
    InCar
}
public class Item: MonoBehaviour
{
    protected float x, y;
    protected int width, height;
    protected State state;
    
    protected Item()
    {
        state = State.OnStore;
    }
}