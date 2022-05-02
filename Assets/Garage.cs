using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public List<GarageCar> cars = new List<GarageCar>();

    public List<GameObject> carsPrefabs = new List<GameObject>();

    private Map map;

    private GameObject selectedCar;

    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<Map>();
        // Добавляем мопед, самый первоначальный транспорт
        var moped = AddTransport("Moped");
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

    public GameObject AddTransport(string transportName)
    {
        var prefab = carsPrefabs.First(x => x.name == transportName);
        if (prefab == null) throw new Exception("Prefab with name " + transportName + " not found");
        var newTransport = Instantiate(prefab, new Vector3(), Quaternion.identity);
        newTransport.transform.position = new Vector3(114, -90, 0);
        newTransport.transform.parent = transform;

        var garageCar = newTransport.GetComponent<GarageCar>();
        garageCar.transportId = transportName;

        if (selectedCar != null)
        {
            selectedCar.SetActive(false);
        }

        map.AddTransport(transportName, garageCar.DriveIn);

        selectedCar = newTransport;

        return newTransport;
    }

    // Update is called once per frame
    void Update()
    {
    }
}