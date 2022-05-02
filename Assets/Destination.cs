using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public readonly int id;

    public Color color;

    public void SetColor(Color color)
    {
        this.color = color;
        GetComponent<SpriteRenderer>().color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}