﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code heavily drawn from: https://gamedevacademy.org/tutorial-multi-level-platformer-game-in-unity/

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpSpeed = 7f;
    public float verticalLookSpeed = 10f;
    public float rotationInterpolationSpeed = 0.15f;

    Inventory m_inventory;

    Rigidbody rb;
    Collider col;

    private bool pressedJump = false;
    private bool pressedInventory = false;

	void Start ()
	{
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        m_inventory = GetComponent<Inventory>();
	}

	void Update ()
	{
        HandleWalk();
        HandleJump();
        HandleInventory();
	}

    private void HandleInventory()
    {
        float iAxis = Input.GetAxis("Fire1");

        if (iAxis > 0f && !pressedInventory)
        {
            m_inventory.ToggleOpen();
            pressedInventory = true;
        }
        else if (iAxis == 0f)
        {
            pressedInventory = false;
        }
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
}
