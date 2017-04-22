using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code heavily drawn from: https://gamedevacademy.org/tutorial-multi-level-platformer-game-in-unity/

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpSpeed = 7f;
    public float verticalLookSpeed = 10f;
    public float rotationInterpolationSpeed = 0.15f;

    // How far away the code will check for interactables
    public float interactionRadius = 2f;

    private Rigidbody rb;
    private Collider col;

    private Inventory inv;

    private bool pressedJump = false;

	void Start ()
	{
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        inv = GetComponent<Inventory>();
	}

	void Update ()
	{
        HandleWalk();
        HandleJump();
	}

    private void HandleWalk()
    {
        // Reset the velocity to zero
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

        // The amount that should be moved in this frame
        float distance = walkSpeed * Time.deltaTime;

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis * distance, 0f, vAxis * distance);

        if (movement.x != 0 || movement.z != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationInterpolationSpeed);
        }        

        Vector3 currPosition = transform.position;
        Vector3 newPosition = currPosition + movement;

        rb.MovePosition(newPosition);
    }

    private void HandleJump()
    {
        float jAxis = Input.GetAxis("Jump");
        bool isGrounded = CheckGrounded();

        if (jAxis > 0f)
        {
            if (!pressedJump && isGrounded)
            {
                pressedJump = true;
                Vector3 jumpVector = new Vector3(0f, jumpSpeed, 0f);

                rb.velocity = rb.velocity + jumpVector;
            }
        }
        else
        {
            pressedJump = false;
        }
    }

    private bool CheckGrounded()
    {
        float sizeX = col.bounds.size.x;
        float sizeZ = col.bounds.size.z;
        float sizeY = col.bounds.size.y;

        // Position of the 4 bottom corners of the game object
        // We add 0.01 in Y as a skin so that there is some distance between the point and the floor
        Vector3 corner1 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner2 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner3 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);
        Vector3 corner4 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);

        // Send a short ray to detect ground
        bool grounded1 = Physics.Raycast(corner1, new Vector3(0, -1, 0), 0.01f);
        bool grounded2 = Physics.Raycast(corner2, new Vector3(0, -1, 0), 0.01f);
        bool grounded3 = Physics.Raycast(corner3, new Vector3(0, -1, 0), 0.01f);
        bool grounded4 = Physics.Raycast(corner4, new Vector3(0, -1, 0), 0.01f);

        // If any corner is grounded, the object is grounded
        return (grounded1 || grounded2 || grounded3 || grounded4);
    }

    private void CheckForInteractables()
    {
        List<Collider> nearbyObjects = new List<Collider>(Physics.OverlapSphere(transform.position, interactionRadius));
        float shortestDistance = float.MaxValue;
        Collider closestCollider = null;
        foreach (Collider c in nearbyObjects)
        {
            if (c.GetComponent<Interactable>())
            {
                if (Vector3.Distance(transform.position, c.transform.position) < shortestDistance)
                {
                    closestCollider = c;
                }
            }
        }

        if (closestCollider != null)
        {
            // Place floating text over the interactable object

            // Check for interaction - this may need to be moved into a separate method if 
            if (Input.GetButtonDown("Interact"))
            {
                closestCollider.gameObject.GetComponent<Interactable>().Interact(this);
            }
        }
    }

    private void PickUpItem()
    {
        //Collider[] nearbyColPhysics.OverlapSphere()
    }
}
