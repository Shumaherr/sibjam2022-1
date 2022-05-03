using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> huge;
    public List<GameObject> big;
    public List<GameObject> meduim;
    public List<GameObject> small;

    public int left = -75;
    public int right = 75;
    public int spawnHeight = 110;

    public float spawnTime = 3f;
    public float scale = 4f;

    public float spawnTimeMultiplier;

    void Start()
    {
        Physics.gravity = new Vector3(0, -50.0F, 0);

        InvokeRepeating("SpawnBox", spawnTime, spawnTime);
    }

    void SpawnBox()
    {
        var rand = Random.Range(0, 100);

        List<GameObject> prefabs;
        if (rand < 5)
        {
            prefabs = huge;
        }
        else if (rand < 20)
        {
            prefabs = big;
        }
        else if (rand < 60)
        {
            prefabs = meduim;
        }
        else
        {
            prefabs = small;
        }


        var prefab = prefabs[Random.Range(0, prefabs.Count)];
        prefab.transform.localScale = new Vector3(scale, scale, 1f);

        var position = new Vector3(Random.Range(left, right), spawnHeight, Quaternion.identity.z + 50);
        var box = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        box.transform.parent = transform;
        box.transform.localPosition = position;
        box.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -50);
    }

    // Update is called once per frame
    void Update()
    {
    }
}