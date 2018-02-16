using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackHandler : MonoBehaviour {

	public void Back()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
