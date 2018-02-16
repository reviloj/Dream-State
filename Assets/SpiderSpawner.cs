using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawner : MonoBehaviour {

    public GameObject prefab;

	void Start()
    {
        InvokeRepeating("SpawnSpider", 0f, 5f);
    }

    void SpawnSpider()
    {
        Instantiate(prefab, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
    }
}
