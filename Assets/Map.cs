using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct TransportConfig
{
    public string name;
    public GameObject prefab;
}

public class Map : MonoBehaviour
{
    private Dictionary<int, GwNode> nodes;
    public Dictionary<int, GameObject> destinations;

    public GameObject destinationPrefab;

    private bool isOpen = false;

    private List<GameObject> transports = new List<GameObject>();

    private List<Color> destinationsColors = new List<Color>()
    {
        Color.red,
        Color.white,
        Color.green,
        Color.cyan,
        Color.magenta
    };

    public List<TransportConfig> transportConfigs;

    public void Start()
    {
        nodes = Graphway.instance.nodes;
        destinations = InitializeDestinations();
    }

    public void ToggleMap()
    {
        // Двигаем камеру, чтоб скрыть/показать карту
        var pos = Camera.main.transform.position;
        pos.z = isOpen ? -400 : -740;
        Camera.main.transform.position = pos;
        isOpen = !isOpen;
    }

    public Transport GetTransport(string transportName)
    {
        return transports.Select(x => x.GetComponent<Transport>())
            .First(x => x.transportId == transportName);
    }

    public void AddTransport(string transportName, Action<bool> callback)
    {
        var config = transportConfigs.FirstOrDefault(conf => conf.name == transportName);
        var instance = Instantiate(config.prefab, new Vector3(), Quaternion.identity);
        instance.transform.parent = transform;
        var transport = instance.GetComponent<Transport>();
        transport.transportId = transportName;
        transport.FinishCallback = callback;
        // Координаты гаража на карте
        instance.transform.localPosition = new Vector3(30, 60, -91);
        transports.Add(instance);
    }

    /**
     * Генерация точек доставки
     */
    private Dictionary<int, GameObject> InitializeDestinations()
    {
        if (destinationPrefab == null)
        {
            throw new Exception("Destination prefab is missing");
        }

        var excludeNodes = new List<int>() {0, 40, 3};

        // Удалить ближайшие к старту точки, чтоб не возить коробки рядом со складом
        var startExcluded = nodes.Values.Where(x => !excludeNodes.Contains(x.nodeID)).ToList();
        var shuffled = Shuffle(startExcluded);

        var result = new Dictionary<int, GameObject>();

        for (var i = destinationsColors.Count - 1; i >= 0; i--)
        {
            var destination = Instantiate(destinationPrefab);
            destination.transform.parent = transform;
            destination.transform.localScale = new Vector3(30, 30, 30);
            destination.GetComponent<Destination>().SetColor(destinationsColors[i]);

            var targetNode = shuffled[i];
            destination.transform.position = targetNode.position;
            result[targetNode.nodeID] = destination;
        }

        return result;
    }

    private static IList<T> Shuffle<T>(IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Random.Range(0, list.Count);
            (list[k], list[n]) = (list[n], list[k]);
        }

        return list;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateDestinations()
    {
    }
}