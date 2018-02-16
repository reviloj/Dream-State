using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameController : MonoBehaviour {

    void Start()
    {
        Cursor.visible = true;
    }
	
	// Update is called once per frame
	void Update () {
        Cursor.visible = true;
    }

    public void Play()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
