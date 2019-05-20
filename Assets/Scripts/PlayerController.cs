using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float maxSpeed;
    public float xAcc;
    public GameObject playerCapsule;
    public GameObject shot;

    public float radius;
    public float force;

    private Rigidbody rb;
    private bool isGrounded;
    private Animator anim;
    private int shotDir;

    private bool xPressed;
    private bool yPressed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
        anim = GetComponent<Animator>();
    }

    
    private void OnCollisionEnter()
    {
        isGrounded = true;
    }

    private void OnCollisionExit()
    {
        isGrounded = false;
    }

    private void OnCollisionStay()
    {
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            GameObject tmp_shot = (GameObject)Instantiate(shot, playerCapsule.transform.position, playerCapsule.transform.rotation);
            if (tmp_shot != null)
            {
                tmp_shot.GetComponent<Shot>().SetDirection(shotDir);
                //Debug.Log(shotDir);
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            yPressed = true;
        }

        else if (Input.GetKey(KeyCode.X))
        {
            xPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            yPressed = false;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            xPressed = false;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.GetComponent<Rigidbody>() != null
                && col.gameObject.layer != 12) // check if gameobject is not a shot
            {
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                Vector3 forceDirection = rb.position - transform.position;
                float distance = forceDirection.magnitude;
                if (yPressed)
                {
                    Debug.Log("Y pressed");
                    rb.AddForce(forceDirection * force * (radius - distance));
                }
                else if (xPressed)
                {
                    rb.AddForce(-forceDirection * force * (radius - distance));
                }
                    
                //Debug.Log("Force Direction: " + forceDirection);
            }
        }

        /*
        if(transform.position.z > 0.9f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.9f);
        }
        if (transform.position.z < -0.9f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.9f);
        }
        */
    }

    void FixedUpdate()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");
        //float move_Y = Input.GetAxisRaw("Vertical");

        if (xDirection > 0)
        {
            shotDir = 1;
        }
        else if(xDirection < 0)
        {
            shotDir = -1;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Jump!");
            rb.AddForce(new Vector3(0, jumpForce * 100.0f, 0));
        }

        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            rb.AddForce(new Vector3(xDirection * xAcc, 0.0f, 0.0f), ForceMode.Acceleration);
        }

        if (Mathf.Abs(rb.velocity.x) > 0.02f)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
