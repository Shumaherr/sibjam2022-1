using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> huge;
    public List<GameObject> big;
    public List<GameObject> meduim;
    public List<GameObject> small;

    public int left = -130;
    public int right = -15;

    public float spawnTime = 3f;
    public float scale = 4f;

    void Start()
    {
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

        var position = new Vector3(Random.Range(left, right), 120f, Quaternion.identity.z);
        var box = Instantiate(prefab, position, Quaternion.identity);
        box.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
    }
}