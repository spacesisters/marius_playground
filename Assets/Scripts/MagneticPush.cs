using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticPush : MonoBehaviour
{
    public float radius;
    public float force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*
    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 forceDirection = rb.position - transform.position;
        rb.AddForce(forceDirection);
    }
    */

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.GetComponent<Rigidbody>() != null
                && col.gameObject.layer != 12) // check if gameobject is not a shot
            {
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                Vector3 forceDirection = rb.position - transform.position;
                float distance = forceDirection.magnitude;
                rb.AddForce(-forceDirection * force * (radius - distance));
                //Debug.Log("Force Direction: " + forceDirection);
            }
        }
    }
}
