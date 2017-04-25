using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    public PlayerMovement playerObj;

	void Start ()
	{
        playerObj = transform.parent.GetComponent<PlayerMovement>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hitting a thing: " + collision.collider.tag);
        if (collision.collider.tag == "Solid")
        {
            //Debug.Log("Hit Solid!");
            playerObj.SetReadyToJump();
        }
    }
}
