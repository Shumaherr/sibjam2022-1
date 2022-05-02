using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Transport : MonoBehaviour
{
    public int MAX_SPEED = 50;
    public int ACCELERATION = 5;

    private GwWaypoint[] waypoints;
    private float speed = 0;

    private GwNode storage;

    private Random rng = new Random();

    private Map map;

    public Action<bool> FinishCallback;

    public string transportId;

    private int checkpoints = 0;
    private int checkpointsDone = 0;

    private void Start()
    {
        map = gameObject.GetComponentInParent<Map>();
        storage = Graphway.instance.nodes.First(n => n.Value.nodeID == 40).Value;
    }

    private bool isDriving = false;

    private readonly Queue<Vector3> routes = new Queue<Vector3>();


    /**
     * Узнаем все цвета коробок из кузова и строим маршрут
     */
    public void SendBoxes(ICollection<Color> colors)
    {
        if (colors.Count == 0) return;

        // Маршрут от склада, через все точки и обратно на склад
        var destinations = map.destinations.Values
            .Where(d => colors.Contains(d.GetComponent<Destination>().color))
            .Select(d => d.GetComponent<Destination>().transform.position).ToList();

        destinations.Add(storage.position);
        destinations.ForEach(d => routes.Enqueue(d));
        checkpoints = destinations.Count;
    }

    void Update()
    {
        if (routes.Count > 0 && !isDriving)
        {
            var route = routes.Dequeue();
            Graphway.FindPath(transform.position, route, FindPathCallback, true, false);
        }

        // Move towards waypoints (if has waypoints)
        if (waypoints != null && waypoints.Length > 0 && isDriving)
        {
            // Increase speed
            speed = Mathf.Lerp(speed, MAX_SPEED, Time.deltaTime * ACCELERATION);
            speed = Mathf.Clamp(speed, 0, MAX_SPEED);

            // Look at next waypoint 
            var vectorToTarget = waypoints[0].position - transform.position;
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

            // Move toward next waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0].position,
                Time.deltaTime * waypoints[0].speed * speed);

            // Check if reach waypoint target
            if (Vector3.Distance(transform.position, waypoints[0].position) < 0.1f)
            {
                // Move on to next waypoint
                NextWaypoint();
            }
        }
        else
        {
            // Reset speed
            speed = 0;
            isDriving = false;
        }
    }

    private void NextWaypoint()
    {
        if (waypoints.Length > 1)
        {
            var newWaypoints = new GwWaypoint[waypoints.Length - 1];

            for (var i = 1; i < waypoints.Length; i++)
            {
                newWaypoints[i - 1] = waypoints[i];
            }

            waypoints = newWaypoints;
        }
        else
        {
            waypoints = null;
            checkpointsDone++;
            if (checkpoints == checkpointsDone)
            {
                FinishCallback(true);
                checkpointsDone = 0;
            }
        }
    }

    private void FindPathCallback(GwWaypoint[] path)
    {
        // Graphway returns 'null' if no path found
        // OR GwWaypoint array of waypoints to destination

        if (path == null)
        {
            Debug.Log("Path to target position not found!");
        }
        else
        {
            waypoints = path;
            isDriving = true;
        }
    }
}