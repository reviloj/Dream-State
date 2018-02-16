using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticMove : MonoBehaviour {

    public Transform target;
    public float speed;

	// Update is called once per frame
	void Update () {
        float dist = Vector3.Distance(transform.position, target.position);
        float step = speed * Time.deltaTime * dist;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
