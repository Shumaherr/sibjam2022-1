using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    private Dictionary<int, GwNode> nodes;
    public Dictionary<int, GameObject> destinations;

    public GameObject destinationPrefab;

    private bool isOpen = false;

    private List<Color> destinationsColors = new List<Color>()
    {
        Color.red,
        Color.white,
        Color.green,
        Color.cyan,
        Color.magenta
    };

    public void Start()
    {
        nodes = Graphway.instance.nodes;
        destinations = InitializeDestinations();
    }

    public void ToggleMap()
    {
        if (isOpen)
        {
            StartCoroutine(CloseMap());
        }
        else
        {
            StartCoroutine(OpenMap());
        }
    }

    private IEnumerator CloseMap()
    {
        while (transform.position.y > -215)
        {
            var pos = transform.position;
            pos.y -= 0.1f;
            transform.position = pos;
            yield return null;
        }

        isOpen = false;
        yield return null;
    }

    private IEnumerator OpenMap()
    {
        while (transform.position.y < 3.73)
        {
            var pos = transform.position;
            pos.y += 0.1f;
            transform.position = pos;
            yield return null;
        }

        isOpen = true;
        yield return null;
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