using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour {
    public Camera camera;
    public static int butterflies;
    public Canvas pause;
    public static TimeController instance = null;
    private bool cursor = false;
    /*
    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    */
    void Start()
    {
        Cursor.visible = false;
        pause = GameObject.FindWithTag("Escape").GetComponent<Canvas>();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update() {
        if (butterflies == 9)
            SceneManager.LoadScene("DeathScene");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause.enabled)
            {
                Cursor.visible = cursor;
            } else
            {
                cursor = Cursor.visible;
                Cursor.visible = true;
            }

            pause.enabled = !pause.enabled;
            Time.timeScale = (Time.timeScale == 0 ? 1 : 0);
            camera.GetComponent<MouseLook>().enabled = !camera.GetComponent<MouseLook>().enabled;

        }
        if (Input.GetKeyDown(KeyCode.Tab) && !pause.enabled)
        {
            freeze();
        }
    }
    public void freeze()
    {
        camera.GetComponent<MouseLook>().move = !camera.GetComponent<MouseLook>().move;
        Cursor.visible = !Cursor.visible;
    }
    public void freeze(bool look, bool mouse)
    {
        camera.GetComponent<MouseLook>().move = look;
        Cursor.visible = mouse;
    }

    public void Quit(){
        SceneManager.LoadScene("StartMenu");
    }
}
