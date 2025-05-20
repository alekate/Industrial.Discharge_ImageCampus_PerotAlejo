using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float maxSpeed;

    public float x;
    public float z;


    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    {
        Movement();
    }

    public void Movement(){
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        float y = 0;

        if (Input.GetKey(KeyCode.Space))
        {
            y = 1;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            y = -1; 
        }
         

        if (Mathf.Abs(z) < 0.01f && Mathf.Abs(x) < 0.01f && Mathf.Abs(y) < 0.01f)
        {
            return;
        }

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = (forward * z + right * x + Vector3.up * y).normalized;
        rb.AddForce(direction * speed, ForceMode.Acceleration);

        
        Vector3 velocity = rb.velocity;

        if (velocity.magnitude > maxSpeed)
        { 
            rb.velocity = velocity.normalized * maxSpeed;
        }


    }





}
