using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GarageCar : MonoBehaviour
{
    public string transportId;

    public bool isAway = false;

    private int speed = 1;

    private Vector3 originalPosition;
    private Text cashText;
    private Transform frontWheel;
    private Transform rearWheel;

    private void Start()
    {
        originalPosition = transform.position;
        cashText = GameObject.Find("CashText").GetComponent<Text>();
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

    private int totalPrice = 0;


    private IEnumerator DriveRight()
    {
        while (transform.position.x < 500)
        {
            var pos = transform.position;
            pos.x += speed;
            transform.position = pos;
            yield return null;
        }

        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.name.StartsWith("Box"))
            {
                totalPrice = child.GetComponent<Box>().Price;
                Destroy(child);
            }
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

        var cash = int.Parse(cashText.text.Split(' ')[1]);
        cashText.text = "Деньги " + (cash + totalPrice);
        totalPrice = 0;
        // Прибавить к общей сумме бабло
    }

    public void DriveAway()
    {
        isAway = true;

        //Play FMOD sound Event.
        AudioManager.instance.PlayDriveAway(transportId);

        StartCoroutine(DriveRight());
    }

    public void DriveIn(bool _)
    {
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