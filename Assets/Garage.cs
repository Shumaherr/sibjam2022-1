using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public List<GameObject> cars = new List<GameObject>();

    private Map map;

    private GameObject selectedCar;

    // Start is called before the first frame update
    public void Start()
    {
        map = FindObjectOfType<Map>();
        UnlockTransport("Moped");

        SelectTransport("Moped");
    }

    public void SendSelectedTransport()
    {
        // Выбираем текущую машины из гаража
        var transport = selectedCar.GetComponent<GarageCar>();
        // Получаем цвета грузов
        var colors = transport.GetCargoColors();
        // Отправляем машину из гаража
        transport.DriveAway();
        // Отправляем машину на карте
        map.GetTransport(transport.transportId).SendBoxes(colors);
    }

    public void SelectTransport(string transportName)
    {
        foreach (var car in cars)
        {
            var pos = car.transform.position;
            pos.z = car.GetComponent<GarageCar>().transportId == transportName
                ? 0
                : 1000;
            car.transform.position = pos;
        }
    }

    public void UnlockTransport(string transportName)
    {
        // make it visible
        var transport = GetComponentsInChildren<GarageCar>().FirstOrDefault(x => x.transportId == transportName);
        if (transport == null) throw new Exception("transport not found");
        map.AddTransport(transportName, transport.DriveIn);
        selectedCar = transport.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }
}