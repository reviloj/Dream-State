using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    private GameObject weapon = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(weapon != null)
        {
            this.weapon.transform.localPosition = new Vector3(0, 0, 0);
        }
	}

    public void equipWeapon(GameObject obj)
    {
        this.weapon = obj;
        this.weapon.transform.parent = this.transform;
        this.weapon.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        this.weapon.transform.localEulerAngles = this.transform.localEulerAngles;
        this.weapon.transform.Rotate(new Vector3(0, -90, -90));
    }
}
