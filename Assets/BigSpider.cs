using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpider : MonoBehaviour {

    public Animator anim;
    public float defaultSpeed;
    public bool random;
    private float dir;
    private float speed;

    void Start()
    {
        InvokeRepeating("Walk", 0f, 5f);
        anim.SetTrigger("StartWalk");
        speed = defaultSpeed;
    }

    void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        this.transform.position += this.transform.forward * step;
    }

    void Walk()
    {
        if (random)
        {
            speed = Random.Range(1f, 4f);
        }
        dir = Random.Range(-180f, 180f);
        this.transform.rotation = Quaternion.Euler(0f, dir, 0f);
    }
}
