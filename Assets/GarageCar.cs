using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

public class GarageCar : MonoBehaviour
{
    public string transportId;

    private bool isAway = false;

    private int speed = 1;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public List<Color> GetCargoColors()
    {
        var colors = new List<Color>()
        {
            Color.red,
            Color.white,
            Color.green,
            Color.cyan,
            Color.magenta
        };
        // @TODO: Получить сет цветов всех коробок в грузовом отсеке
        return new List<Color>() {colors[Random.Range(0, 5)], colors[Random.Range(0, 5)]};
    }

    private IEnumerator DriveRight()
    {
        while (transform.position.x < 500)
        {
            var pos = transform.position;
            pos.x += speed;
            transform.position = pos;
            yield return null;
        }
    }

    private IEnumerator DriveLeft()
    {
        while (transform.position.x > 114)
        {
            var pos = transform.position;
            pos.x -= speed;
            transform.position = pos;
            yield return null;
        }
    }

    public void DriveAway()
    {
        isAway = true;
        StartCoroutine(DriveRight());
    }

    public void DriveIn(bool _)
    {
        StartCoroutine(DriveLeft());
        isAway = false;
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        // var pos = FindObjectOfType<Map>().GetTransport(transportId).transform.position;
        // var x = Vector2.Distance(pos, new Vector2(13.8f, 0f));

        // transform.position = new Vector3(x, transform.position.y, transform.position.z) * 10;
    }
}