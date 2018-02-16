using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditText : MonoBehaviour {

    public Text uses;

	// Use this for initialization
	void Start () {
		
	}

    public void updateText(int text)
    {
        uses.text = text.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
