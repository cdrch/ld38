using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code heavily drawn from: https://gamedevacademy.org/tutorial-multi-level-platformer-game-in-unity/

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
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
    private CapsuleCollider m_collider;
    private Inventory m_inventory;
    private Animator m_animator;
    
    private bool pressedJump = false;

    private int layerMask;
    private float skinWidth;

    private Camera currentCamera;
    private bool sceneHasBeenChanged;
    private bool needToUpdateZAxis;
    private bool needToUpdateXAxis;

    private Vector2 customXAxis;
    private Vector2 customZAxis;

    public Transform oldX;
    public Transform oldZ;
    public Transform newX;
    public Transform newZ;


    void Start ()
	{
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
        m_inventory = GetComponent<Inventory>();
        m_animator = GetComponent<Animator>();
        
        m_animator.SetBool("IsWalking", false);
        m_animator.SetBool("IsInAir", false);

        layerMask = 1 << 8;
        layerMask = ~layerMask;
        skinWidth = 0.1f;
    }

	void Update ()
	{
        // Reset the velocity to zero - moved here from HandleWalk to prevent the player from falling over while the inventory is open
        m_rigidBody.velocity = new Vector3(0f, m_rigidBody.velocity.y, 0f);

        if (!m_inventory.IsOpen())
        {
            HandleWalk();
            HandleJump();
            CheckForInteractables();
        }
        else
        {
            m_animator.SetBool("IsWalking", false);
        }
        
        if (CheckGrounded())
        {
            m_animator.SetBool("IsInAir", false);
            HandleInventory();
        }            
        else
            m_animator.SetBool("IsInAir", true);
    }

    private void HandleInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            m_inventory.ToggleOpen();
        }
    }

    private void CalculateCustomAxes(bool updateX, bool updateZ)
    {
        if (updateX)
        {
            oldX.rotation = newX.rotation;
            needToUpdateXAxis = false;
        }
        if (updateZ)
        {
            oldZ.rotation = newZ.rotation;
            needToUpdateZAxis = false;
        }
    }

    // Call this whenever the view camera changes
    public void OnCameraChange(Camera cam)
    {
        //newX.rotation = cam.transform.rotation;
        newX.rotation = new Quaternion(cam.transform.rotation.x, 0f, cam.transform.rotation.z, cam.transform.rotation.w);
        newZ.rotation = new Quaternion(cam.transform.rotation.x, 0f, cam.transform.rotation.z, cam.transform.rotation.w);
        needToUpdateXAxis = true;
        needToUpdateZAxis = true;
        sceneHasBeenChanged = true;
    }

    private void HandleWalk()
    {
        // The amount that should be moved in this frame
        float distance = walkSpeed * Time.deltaTime;

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        /*
        // Calculate custom axes - wow this is overly complex and hacky
        if (sceneHasBeenChanged)
        {
            if (hAxis == 0 && vAxis == 0 && needToUpdateXAxis && needToUpdateZAxis)
            {
                CalculateCustomAxes(true, true);
            }
            else if (hAxis == 0 && needToUpdateXAxis)
            {
                CalculateCustomAxes(true, false);
            }
            else if (vAxis == 0 && needToUpdateZAxis)
            {
                CalculateCustomAxes(false, true);
            }

            if (!needToUpdateZAxis && !needToUpdateXAxis)
            {
                sceneHasBeenChanged = false;
            }
        }
        */
        Vector3 movement = new Vector3(hAxis * distance, 0f, vAxis * distance);
        /*
        Vector3 hAxisMovement = oldX.right * hAxis * distance;
        Vector3 vAxisMovement = oldZ.forward * vAxis * distance;

        Vector3 movement = hAxisMovement + vAxisMovement;
        */
        if (movement.x != 0 || movement.z != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationInterpolationSpeed);
            m_animator.SetBool("IsWalking", true);
        }        

        Vector3 currPosition = transform.position;
        Vector3 newPosition = currPosition + movement;

        m_rigidBody.MovePosition(newPosition);

        if (currPosition == newPosition)
        {
            m_animator.SetBool("IsWalking", false);
        }
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
        Vector3 corner1 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + skinWidth + m_collider.center.y, sizeZ / 2);
        Vector3 corner2 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + skinWidth + m_collider.center.y, sizeZ / 2);
        Vector3 corner3 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + skinWidth + m_collider.center.y, -sizeZ / 2);
        Vector3 corner4 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + skinWidth + m_collider.center.y, -sizeZ / 2);

        // Additional checks to see if 
        Vector3 center0 = transform.position + new Vector3(0f, -sizeY / 2 + skinWidth + m_collider.center.y, 0f);
        Vector3 center1 = transform.position + new Vector3(sizeX / 4, -sizeY / 2 + skinWidth + m_collider.center.y, sizeZ / 4);
        Vector3 center2 = transform.position + new Vector3(-sizeX / 4, -sizeY / 2 + skinWidth + m_collider.center.y, sizeZ / 4);
        Vector3 center3 = transform.position + new Vector3(sizeX / 4, -sizeY / 2 + skinWidth + m_collider.center.y, -sizeZ / 4);
        Vector3 center4 = transform.position + new Vector3(-sizeX / 4, -sizeY / 2 + skinWidth + m_collider.center.y, -sizeZ / 4);

        // Send a short ray to detect ground
        bool grounded1 = Physics.Raycast(corner1, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded2 = Physics.Raycast(corner2, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded3 = Physics.Raycast(corner3, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded4 = Physics.Raycast(corner4, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded5 = Physics.Raycast(center0, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded6 = Physics.Raycast(center1, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded7 = Physics.Raycast(center2, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded8 = Physics.Raycast(center3, new Vector3(0, -1, 0), skinWidth, layerMask);
        bool grounded9 = Physics.Raycast(center4, new Vector3(0, -1, 0), skinWidth, layerMask);
        /*
        Debug.DrawRay(corner1, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(corner2, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(corner3, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(corner4, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(center0, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(center1, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(center2, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(center3, new Vector3(0, -1, 0), Color.red, 0.01f);
        Debug.DrawRay(center4, new Vector3(0, -1, 0), Color.red, 0.01f);
        */
        // If any corner is grounded, the object is grounded
        return (grounded1 || grounded2 || grounded3 || grounded4 || grounded5 || grounded6 || grounded7 || grounded8 || grounded9);
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
