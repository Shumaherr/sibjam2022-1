using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Garage : MonoBehaviour
{
    public List<GameObject> cars = new List<GameObject>();

    private Map map;

    public GameObject selectedCar;

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
            var id = car.GetComponent<GarageCar>().transportId;
            if (id == transportName)
            {
                car.SetActive(true);
                // pos.z = 0;
                selectedCar = car;
            }
            else
            {
                car.SetActive(false);
            }
        }
    }

    public void UnlockTransport(string transportName)
    {
        // Разблокируем транспорт, добавляем его на миникарту
        var transports = cars.Select(x => x.GetComponent<GarageCar>());
        var transport = transports.FirstOrDefault(x => x.transportId == transportName);
        if (transport == null) throw new Exception("transport not found");
        if (map != null)
        {
            map.AddTransport(transportName, transport.DriveIn);
        }
        else
        {
            Debug.Log("Активируй карту");
        }

        selectedCar = transport.gameObject;
        SelectTransport(transportName);
    }

    public void BuyTransport(string n, int price)
    {
        var cashText = GameObject.Find("CashText").GetComponent<Text>();
        var cash = int.Parse(cashText.text.Split(' ')[1]);
        if (cash >= price)
        {
            FindObjectOfType<Garage>().UnlockTransport(n);
            cashText.text = "Деньги " + (cash - price);
        }
    }
}