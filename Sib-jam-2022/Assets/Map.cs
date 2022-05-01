using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    private Dictionary<int, GwNode> nodes;
    public Dictionary<int, GameObject> destinations;

    public GameObject destinationPrefab;

    private List<Color> destinationsColors = new List<Color>()
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.cyan,
        Color.magenta
    };

    public void Start()
    {
        nodes = Graphway.instance.nodes;
        destinations = InitializeDestinations();
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

        var shuffled = Shuffle(nodes.Values.ToList());

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