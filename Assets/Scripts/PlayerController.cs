using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    public float maxSpeed;
    public float xAcc;
    public GameObject playerCapsule;
    public GameObject forceSphere;
    public GameObject shot;

    public float maxMagnetRadius;
    public float maxMagnetForce;

    public AudioClip sfxJump;
    public AudioClip sfxImpact;
    public AudioClip sfxShot;
    public AudioClip sfxForcefield;

    private Rigidbody rb;
    private Animator anim;
    private Material mat;

    private int shotDir;
    private float magnetRadius;

    private bool isGrounded;
    private bool isPulling;
    private bool isPushing;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true;
        anim = playerCapsule.GetComponent<Animator>();
        magnetRadius = 0.0f;
        forceSphere.transform.localScale = new Vector3(magnetRadius, magnetRadius, 1.5f);
        mat = forceSphere.GetComponent<Renderer>().material;
        audioSource = GetComponent<AudioSource>();
    }

    public void playImpactSound()
    {
        audioSource.PlayOneShot(sfxImpact);
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            audioSource.PlayOneShot(sfxShot);
            GameObject tmp_shot = (GameObject)Instantiate(shot, playerCapsule.transform.position, playerCapsule.transform.rotation);
            if (tmp_shot != null)
            {
                tmp_shot.GetComponent<Shot>().SetDirection(shotDir);
                tmp_shot.GetComponent<Shot>().SetPlayer(this);
                //Debug.Log(shotDir);
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            isPushing = true;
            forceSphere.SetActive(true);
            mat.SetVector("Color_45AE0E17", Color.red * 8f);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            isPulling = true;
            forceSphere.SetActive(true);
            mat.SetVector("Color_45AE0E17", Color.blue * 8f);
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            isPushing = false;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            isPulling = false;
        }

        if (isPushing || isPulling)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            if (magnetRadius < 7)
            {
                magnetRadius += 0.8f;
                forceSphere.transform.localScale = new Vector3(magnetRadius, magnetRadius, 1.5f);
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, maxMagnetRadius);
            foreach (Collider col in colliders)
            {
                if (col.gameObject.GetComponent<Rigidbody>() != null
                    && col.gameObject.layer != 12) // check if gameobject is not a shot
                {
                    Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                    Vector3 forceDirection = rb.position - transform.position;
                    float distance = forceDirection.magnitude;
                    if (isPushing)
                    {
                        rb.AddForce(forceDirection.normalized * maxMagnetForce * (maxMagnetRadius - distance));
                        
                    }
                    else if (isPulling)
                    {
                        rb.AddForce(2 * -forceDirection.normalized * maxMagnetForce * (maxMagnetRadius - distance));
                    }
                    //Debug.Log("Radius: " + maxMagnetRadius + " - Distance: " + distance + " - Force: " + maxMagnetForce);
                    //Debug.Log("Force Direction: " + forceDirection);
                }
            }
        }
        else
        {
            if (magnetRadius > 0)
            {
                magnetRadius -= 0.4f;
                forceSphere.transform.localScale = new Vector3(magnetRadius, magnetRadius, 1.5f);
                if (magnetRadius < 0.5)
                {
                    forceSphere.SetActive(false);
                }
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
            audioSource.PlayOneShot(sfxJump);
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
