using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    private Dictionary<int, GwNode> nodes;
    private List<GameObject> destinations;

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

    private List<GameObject> InitializeDestinations()
    {
        if (destinationPrefab == null)
        {
            throw new Exception("Destination prefab is missing");
        }

        var destinations = new List<GameObject>();
        var nodesList = Shuffle(nodes.Values.ToList());

        for (var i = nodesList.Count - 1; i >= 0; i--)
        {
            var randomIndex = Random.Range(0, nodesList.Count - 1);
            var destinationNode = nodesList[randomIndex];
            nodesList = nodesList.Where(node => node.nodeID != destinationNode.nodeID).ToList();
            var dest = Instantiate(destinationPrefab);
            dest.transform.position = destinationNode.position;
            dest.transform.parent = transform;
            destinations.Add(dest);
        }

        return destinations;
    }

    public IList<T> Shuffle<T>(IList<T> list)
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