using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

public class GarageCar : MonoBehaviour
{
    public string transportId;

    public bool isAway = false;

    private int speed = 1;

    private Vector3 originalPosition;

    private Transform frontWheel;
    private Transform rearWheel;

    private void Start()
    {
        originalPosition = transform.position;
        frontWheel = transform.GetChild(0);
        rearWheel = transform.GetChild(1);
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
        while (transform.position.x > originalPosition.x)
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
        StartCoroutine(Rotate(frontWheel.transform, true));
        StartCoroutine(Rotate(rearWheel.transform, true));
        StartCoroutine(DriveRight());
    }

    public void DriveIn(bool _)
    {
        StartCoroutine(Rotate(frontWheel.transform, false));
        StartCoroutine(Rotate(rearWheel.transform, false));
        StartCoroutine(DriveLeft());
        isAway = false;
    }

    IEnumerator Rotate(Transform tf, bool clockwise)
    {
        for (var i = 0; i < 90; i++)
        {
            tf.Rotate(Vector3.forward, clockwise ? -10 : 10);
            yield return new WaitForSeconds(0.01f);
        }
    }
}