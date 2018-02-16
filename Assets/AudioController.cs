using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public static AudioController instance;
    public Animator anim;

    public GameObject guy;
    public Movement move;
    public float speed;
    private AudioSource[] sources;
    private float jumpCD = 0.5f;
    private float timeStamp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(this);
        }
    }

	// Use this for initialization
	void Start () {
        move = guy.GetComponent<Movement>();
        sources = GetComponentsInChildren<AudioSource>();
        sources[0].Play();
        timeStamp = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (!move.grounded)
        {
            sources[1].mute = true;
            sources[2].mute = true;
            return;
        }
        speed = move.moveSpeed;
        if (!anim.GetBool("Shift") && speed > 0.1f)
        {
            sources[2].mute = true;
            sources[1].mute = false;
        } else if (anim.GetBool("Shift") && speed > 0.1)
        {
            sources[2].mute = false;
            sources[1].mute = true;
        } else
        {
            sources[2].mute = true;
            sources[1].mute = true;
        }

        
	}

    public void Jump()
    {
        if (timeStamp <= Time.time)
        {
            sources[3].Play();
            timeStamp = Time.time + jumpCD;
        }
    }

    public void Oof()
    {
        if (!(GameObject.FindObjectOfType<DummyHealthBar>().GetHealth() <= 0))
        {
            sources[4].Play();
        }
    }

}
