using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(speed, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Enemy")
        {
            Animator enemyAnim = collision.collider.gameObject.GetComponent<Animator>();
            enemyAnim.SetTrigger("die");
            Destroy(collision.collider.gameObject, 2);
        }

        Debug.Log("Shot Collision!");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}