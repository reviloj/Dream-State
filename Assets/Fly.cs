using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        StartCoroutine(fly());
	}

    public IEnumerator fly()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            this.transform.Translate(new Vector3(0, 0f, -0.2f));
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
