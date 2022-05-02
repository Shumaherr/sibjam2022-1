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
    }

    public void SendSelectedTransport()
    {
        // Выбираем текущую машины из гаража
        var transport = selectedCar.GetComponent<GarageCar>();
        if (transport.isAway) return;

        // Получаем цвета грузов
        var colors = transport.GetCargoColors();
        // Если груза нет, ничего не делаем
        if (colors.Count == 0) return;

        // Отправляем машину из гаража
        transport.DriveAway();
        // Отправляем машину на карте
        map.GetTransport(transport.transportId).SendBoxes(colors);
    }

    public void SelectTransport(string transportName)
    {
        // Скрываем все машины в гараже кроме выбранной
        foreach (var car in cars)
        {
            var pos = car.transform.position;
            var id = car.GetComponent<GarageCar>().transportId;
            if (id == transportName)
            {
                pos.z = 0;
                selectedCar = car;
            }
            else
            {
                pos.z = 1000;
            }

            car.transform.position = pos;
        }
    }

    public void UnlockTransport(string transportName)
    {
        // Разблокируем транспорт, добавляем его на миникарту
        var transport = GetComponentsInChildren<GarageCar>().FirstOrDefault(x => x.transportId == transportName);
        if (transport == null) throw new Exception("transport not found");
        map.AddTransport(transportName, transport.DriveIn);
        selectedCar = transport.gameObject;
        SelectTransport(transportName);
    }
}