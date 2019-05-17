using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > 0.9f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.9f);
        }
        if (transform.position.z < -0.9f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.9f);
        }
    }

    void FixedUpdate()
    {
        float move_X = Input.GetAxisRaw("Horizontal");
        float move_Y = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 200.0f, 0));
        }

        rb.AddForce(new Vector3(move_X, 0.0f, move_Y), ForceMode.Impulse);
    }
}
