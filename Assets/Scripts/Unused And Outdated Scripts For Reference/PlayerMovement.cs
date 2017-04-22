using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 0f;
    public float maxSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    public float jumpHeight = 5f; // This is how high you want the character to jump
    public float timeToJumpApex = 2f; // This is how long you want it to take for the max jump height to be achieved
    private float gravity; // Based on jumpHeight and timeToJumpApex
    private float jumpVelocity; // Based on jumpHeight and timeToJumpApex

    public float jumpCooldown = 0.5f;
    private float timeToNextJump;
    public int maxJumps = 1;
    private int jumpsRemaining;

    private Vector3 movement;

    private Rigidbody rb;

    private Vector3 prevVelocity;
    private Vector3 targetVelocity;

	void Start ()
	{
        rb = GetComponent<Rigidbody>();
        movement = Vector3.zero;

        // Calculate jump information based on desired variables - see: https://www.youtube.com/watch?v=PlT44xr0iW0&index=3&list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        Debug.Log("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);

        jumpsRemaining = maxJumps;
        timeToNextJump = 0f;
	}

	void Update ()
	{
        /*
        prevVelocity = rb.velocity;
        targetVelocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);

        rb.velocity = Vector3.Lerp(prevVelocity, targetVelocity, 0.1f);
        

        if (Input.GetAxis("Horizontal") != 0 && speed < maxSpeed)
        {
            speed = speed - acceleration * Time.deltaTime;
        }*/
	}

    private void FixedUpdate()
    {
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // information drawn from here: http://answers.unity3d.com/questions/29751/gradually-moving-an-object-up-to-speed-rather-then.html
    }

    private void Move(float h, float v)
    {
        if (v != 0f || h != 0f)
        {
            movement.Set(h, 0f, v);
            speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);
        }
        else
        {
            speed = Mathf.Max(speed - deceleration * Time.deltaTime, 0f);
        }
        //Debug.Log(speed);

        // Normalize the movement vector, then make it proportional to the speed per second
        movement = movement.normalized * speed * Time.deltaTime;

        //movement.Set(movement.x, Input.GetButtonDown, movement.z)
        // Move the player to current position + movement
        rb.MovePosition(transform.position + movement);

        //Debug.Log(Input.GetButtonDown("Jump"));
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0 && timeToNextJump == 0f)
        {
            //Debug.Log("Should be jumping");
            rb.AddForce(0f, jumpVelocity, 0f, ForceMode.VelocityChange);
            jumpsRemaining--;
            timeToNextJump = jumpCooldown;
        }
        else
        {
            timeToNextJump -= Time.deltaTime;
            timeToNextJump = Mathf.Max(timeToNextJump, 0f);
        }
        
    }

    private void Jump(float j)
    {

    }

    public void SetReadyToJump()
    {
        jumpsRemaining = maxJumps;
    }
}
