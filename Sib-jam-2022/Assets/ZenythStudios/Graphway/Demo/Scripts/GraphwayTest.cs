﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GraphwayTest : MonoBehaviour
{
    public const int MAX_SPEED = 50;
    public const int ACCELERATION = 5;

    [Tooltip("Enable Debug Mode to see algoritm in action slowed down. MAKE SURE GIZMOS ARE ENABLED!")]
    public bool debugMode = false;

    private GwWaypoint[] waypoints;
    private float speed = 0;

    private GwNode storage;

    private Random rng = new Random();

    private Map map;

    private void Start()
    {
        map = gameObject.GetComponentInParent<Map>();
        storage = Graphway.instance.nodes.First(n => n.Value.nodeID == 40).Value;
    }

    private Queue<GwWaypoint[]> paths = new Queue<GwWaypoint[]>();

    /**
     * Узнаем все цвета коробок из кузова и строим маршрут
     */
    private void SendBoxes(ICollection<Color> colors)
    {
        var path = new List<Vector3>() {storage.position};
        var destinations = map.destinations.Values
            .Where(d => colors.Contains(d.GetComponent<Destination>().color))
            .Select(d => d.GetComponent<Destination>().transform.position);

        foreach (var destination in path.Concat(destinations))
        {
            
        }
    }

    void Update()
    {
        // Handle mouse click
        if (Input.GetMouseButtonDown(0))
        {
            var dest = map.destinations.ElementAtOrDefault(rng.Next(0, map.destinations.Count - 1)).Value;
            Graphway.FindPath(transform.position, dest.transform.position, FindPathCallback, true, debugMode);
        }

        // Move towards waypoints (if has waypoints)
        if (waypoints != null && waypoints.Length > 0)
        {
            // Increase speed
            speed = Mathf.Lerp(speed, MAX_SPEED, Time.deltaTime * ACCELERATION);
            speed = Mathf.Clamp(speed, 0, MAX_SPEED);

            // Look at next waypoint 
            transform.LookAt(waypoints[0].position);

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
        }

        // Draw Path
        if (debugMode && waypoints != null && waypoints.Length > 0)
        {
            Vector3 startingPoint = transform.position;

            for (int i = 0; i < waypoints.Length; i++)
            {
                Debug.DrawLine(startingPoint, waypoints[i].position, Color.green);

                startingPoint = waypoints[i].position;
            }
        }
    }

    private void NextWaypoint()
    {
        if (waypoints.Length > 1)
        {
            GwWaypoint[] newWaypoints = new GwWaypoint[waypoints.Length - 1];

            for (int i = 1; i < waypoints.Length; i++)
            {
                newWaypoints[i - 1] = waypoints[i];
            }

            waypoints = newWaypoints;
        }
        else
        {
            waypoints = null;
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
        }
    }


    void OnGUI()
    {
        // GUIStyle style = new GUIStyle();
        // style.normal.textColor = Color.red;
        //
        // GUI.Label(new Rect(20, 20, 200, 20),
        //     "INSTRUCTIONS: Left click on a roadway node to find the quickest path to it.", style);
        // GUI.Label(new Rect(Screen.width - 260, 20, 200, 20), "Make sure GIZMOS are ENABLED! ^^^", style);
    }
}