using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(getUp());
    }
	
    public IEnumerator getUp()
    {
        Vector3 rotatePoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.2f);
        Debug.Log(this.transform.localEulerAngles.x);
        while (this.transform.localEulerAngles.x >= 270)
        {
            yield return new WaitForSeconds(0.005f);
            this.transform.RotateAround(rotatePoint, Vector3.left, 4);
        }
    }
    
}
