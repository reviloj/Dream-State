using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandSpawn : MonoBehaviour {

    public Terrain terrain;
    public int numberOfChests; // number of objects to place
    public int numberOfSpiderSpawners; // number of objects to place
    private int currentChests; // number of placed objects
    private int currentSpawners; // number of placed objects
    public GameObject chest; // GameObject to place
    public GameObject spiderSpawner; // GameObject to place

    private int terrainWidth;
    // terrain size z
    private int terrainLength;
    // terrain x position
    private int terrainPosX;
    // terrain z position
    private int terrainPosZ;

    // Use this for initialization
    void Start()
    {
        terrainWidth = 760;
        // terrain size z
        terrainLength = 760;
        // terrain x position
        terrainPosX = (int)terrain.transform.position.x + 90;
        // terrain z position
        terrainPosZ = (int)terrain.transform.position.z + 70;

        // generate objects
        while (currentChests <= numberOfChests / 2)
        {
            // generate random x position
            int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
            // generate random z position
            int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
            // get the terrain height at the random position
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // create new gameObject on random position
            GameObject newObject = (GameObject)Instantiate(chest, new Vector3(posx, posy, posz), Quaternion.identity);
            currentChests += 1;
        }
        while (currentSpawners <= numberOfSpiderSpawners / 10)
        {
            // generate random x position
            int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
            // generate random z position
            int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
            // get the terrain height at the random position
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // create new gameObject on random position
            GameObject newObject = (GameObject)Instantiate(spiderSpawner, new Vector3(posx, posy + 1, posz), Quaternion.identity);
            currentSpawners += 1;
        }
        StartCoroutine(RandomChests());
        StartCoroutine(RandomSpawners());
    }

    public IEnumerator RandomChests()
    {
        while (true) {
            yield return new WaitForSeconds(Mathf.Clamp(60 / (Time.timeSinceLevelLoad / 200 + 1), 15, 10000));
            if (currentChests <= numberOfChests)
            {
                // generate random x position
                int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
                // generate random z position
                int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
                // get the terrain height at the random position
                float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
                // create new gameObject on random position
                GameObject newObject = (GameObject)Instantiate(chest, new Vector3(posx, posy, posz), Quaternion.identity);
                currentChests += 1;
            }
        }
    }
    public IEnumerator RandomSpawners()
    {
        while (currentSpawners <= numberOfSpiderSpawners)
        {
            yield return new WaitForSeconds(Mathf.Clamp(120 / (Time.timeSinceLevelLoad / 200 + 1), 15, 100000));
            // generate random x position
            int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
            // generate random z position
            int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
            // get the terrain height at the random position
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // create new gameObject on random position
            GameObject newObject = (GameObject)Instantiate(chest, new Vector3(posx, posy, posz), Quaternion.identity);
            currentChests += 1;
        }
    }

        // Update is called once per frame
        void Update () {
		
	}
}
