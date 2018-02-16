using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnButterfly : MonoBehaviour {

    public GameObject flight;
    public GameObject butterfly;

	// Use this for initialization
	IEnumerator Start () {
		for(int i = 0; i < TimeController.butterflies; i++)
        {
            yield return new WaitForSeconds((float)Random.Range(1, 8) / 4);
            print(1);
            GameObject fly = GameObject.Instantiate(butterfly);
            fly.transform.position = new Vector3(this.transform.position.x + ((float)Random.Range(-4, 4) / 2), this.transform.position.y, this.transform.position.z);
        }
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("StartMenu");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
