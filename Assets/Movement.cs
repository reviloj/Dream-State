using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{

    AudioController ac;
    public float gravity = 10.0f;
	public float maxVelocityChange = 6.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
    public bool grounded = true;
    public float speed;
    private int collisions;
    private int frames;
    public float moveSpeed;
    private bool alive = true;

    private Rigidbody rb;
    private Animator anim;

    public Animator getAnim()
    {
        return anim;
    }

    void Awake()
    {
        ac = AudioController.instance;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        rb.useGravity = false;
        collisions = 0;
        frames = 0;
    }

    void Start()
    {
        ac = AudioController.instance;
        alive = true;
    }

    void FixedUpdate()
    {
        if (GameObject.FindObjectOfType<DummyHealthBar>().GetHealth() <= 0)
        {
            rb.velocity = new Vector3(0, 0, 0);
            if(alive == true)
            {
                StartCoroutine(FallDown());
                alive = false;

            }
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Shift", false);
        } else
        {
            anim.SetBool("Shift", true);
        }

        if (grounded)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            if (!anim.GetBool("Shift"))
            {
                velocityChange /= 15;
            }
            if ((targetVelocity.x != 0 || targetVelocity.y != 0) && anim.GetBool("Shift"))
            {
                sprintTiredness();
            }
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButton("Jump"))
            {
                ExhaustionBar tired = GameObject.FindObjectOfType<ExhaustionBar>();
                tired.ReduceExhaustion(2f);
                ac.Jump();
                rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                grounded = false;
                anim.SetTrigger("Jump");
                anim.ResetTrigger("Land");
            }
        }
        else { anim.SetTrigger("Jump"); }
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

        moveSpeed = transform.InverseTransformDirection(rb.velocity).z;
        anim.SetFloat("MoveSpeed", moveSpeed);


        if (collisions <= 0)
        {
            if (frames > 120)
            {
                anim.SetTrigger("Jump");
                anim.ResetTrigger("Land");
                frames = 0;

            } else
            {
                frames++;
            }
        }

        if (Input.GetKeyDown("f"))
        {
            GameObject[] chests;
            chests = GameObject.FindGameObjectsWithTag("Chest");
            GameObject closestChest = null;
            float smallestDistance = Mathf.Infinity;
            foreach (GameObject chest in chests)
            {
                float distance = Vector3.Distance(transform.position, chest.transform.position);
                if (distance < smallestDistance)
                {
                    closestChest = chest;
                    smallestDistance = distance;
                }
            }
            if (closestChest != null && smallestDistance <= 5f)
            {
                closestChest.GetComponent<Chest>().Open();
            }
        }
    }

    void OnCollisionStay()
    {
        grounded = true;
        anim.SetTrigger("Land");
        anim.ResetTrigger("Jump");
    }

    void onCollisionEnter(Collision col)
    {
        collisions++;
    }

    void onCollisionExit()
    {
        collisions--;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
    private void sprintTiredness()
    {
        ExhaustionBar tired = GameObject.FindObjectOfType<ExhaustionBar>();
        tired.ReduceExhaustion(5f * Time.deltaTime);
        //print(4f * Time.deltaTime);
    }

    public void TakeDamage(int dmg)
    {
        ac.Oof();
        DummyHealthBar dhb = GameObject.FindObjectOfType<DummyHealthBar>();
        dhb.ReduceHealth(dmg);
    }
    private IEnumerator FallDown()
    {
        while (this.transform.rotation.x >= -0.4) {
            yield return new WaitForSeconds(0.01f);
            this.transform.Rotate(new Vector3(-1, 0, 0));
        }
        yield return new WaitForSeconds(4f);
        FindObjectOfType<DummyInventoryControl>().inventory.transform.Translate(new Vector3( -500, 0, 0));
        DadCell[] cells = FindObjectOfType<DummyInventoryControl>().inventory.GetComponentsInChildren<DadCell>();
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].GetItem() != null && cells[i].GetComponentInChildren<ClickItem>().type == ClickItem.Type.scroll && cells[i].loc == DadCell.Location.backpack)
            {
                TimeController.butterflies++;
            }
        }
        FindObjectOfType<DummyInventoryControl>().inventory.SetActive(true);
        SceneManager.LoadScene("DeathScene");
    }
}
