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

    private Rigidbody m_rigidBody;
    private Collider m_collider;
    private Inventory m_inventory;

    private bool pressedJump = false;

	void Start ()
	{
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        m_inventory = GetComponent<Inventory>();
	}

	void Update ()
	{
        if (!m_inventory.IsOpen())
        {
            HandleWalk();
            HandleJump();
            CheckForInteractables();
        }
        HandleInventory();
	}

    private void HandleInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            m_inventory.ToggleOpen();
        }
    }

    private void HandleWalk()
    {
        // Reset the velocity to zero
        m_rigidBody.velocity = new Vector3(0f, m_rigidBody.velocity.y, 0f);

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

        m_rigidBody.MovePosition(newPosition);
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

                m_rigidBody.velocity = m_rigidBody.velocity + jumpVector;
            }
        }
        else
        {
            pressedJump = false;
        }
    }

    private bool CheckGrounded()
    {
        float sizeX = m_collider.bounds.size.x;
        float sizeZ = m_collider.bounds.size.z;
        float sizeY = m_collider.bounds.size.y;

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
        GameObject closestObject = null;
        foreach (Collider c in nearbyObjects)
        {
            if (c.gameObject.GetComponent<Interactable>())
            {
                if (Vector3.Distance(transform.position, c.transform.position) < shortestDistance)
                {
                    closestObject = c.gameObject;
                }
            }
            else if (c.GetComponentInParent<Interactable>())
            {
                if (Vector3.Distance(transform.position, c.transform.position) < shortestDistance)
                {
                    closestObject = c.gameObject.transform.parent.gameObject;
                }
            }
        }

        if (closestObject != null)
        {
            // Place floating text over the interactable object
            //Debug.Log("Near " + closestCollider.gameObject.name);
            // Check for interaction - this may need to be moved into a separate method if 
            if (Input.GetButtonDown("Interact"))
            {
                //Debug.Log(closestObject.name);
                closestObject.GetComponent<Interactable>().Interact(this);
            }
        }
    }
}
