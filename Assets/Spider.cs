using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {

    public Animator anim;
    public float followDistance;
    public float defaultSpeed;
    public bool random;
    private float dir;
    private float speed;
    public bool walking = true;
    private Transform target;
    private Vector3 randomTarget;
    private float distanceToTarget;
	
	void Start()
    {
        randomTarget = new Vector3();
        InvokeRepeating("Walk", 0f, 5f);
        anim.SetTrigger("StartWalk");
        speed = defaultSpeed;
        target = GameObject.FindWithTag("Feet").transform;
    }

    void FixedUpdate()
    {
        distanceToTarget = Vector3.Distance(target.position, this.transform.position);
        if (walking) {
            float step = speed * Time.deltaTime;
            if (distanceToTarget < followDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                transform.LookAt(target);
            } else
            {
                transform.position = Vector3.MoveTowards(transform.position, randomTarget, step);
                transform.LookAt(randomTarget);
            }
        }
    }

    void Walk()
    {
        if (random)
        {
            speed = Random.Range(2f, 5f);
        }
        if (distanceToTarget > followDistance)
        {
            randomTarget = new Vector3(transform.position.x + Random.Range(-100, 100), transform.position.y, transform.position.z + Random.Range(-100, 100));
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Dreamer" && walking)
        {
            walking = false;
            CancelInvoke();
            this.transform.LookAt(col.gameObject.transform.position);
            anim.SetTrigger("Attack");

            Vector3 dir = col.gameObject.transform.position - this.transform.position;
            dir = dir.normalized;
            col.gameObject.GetComponent<Rigidbody>().AddForce(dir * 1000);

            col.gameObject.GetComponent<Movement>().TakeDamage(10);
            Destroy(gameObject, 5f);
        }
    }
}
