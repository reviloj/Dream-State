using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextHandler : MonoBehaviour {

    public void Next()
    {
        print(1);
        SceneManager.LoadScene("Items");
    }
}
