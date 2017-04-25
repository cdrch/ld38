using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	void Start ()
	{
		
	}

    /**
    * Given a PlayerController, method should attempt to interact with the Object
    * this script is attached to. 
    * param - PlayerController - controller for the player
    * returns {bool} - True if the interaction was successful, false if not
    */
    public virtual bool Interact(PlayerController interacter)
    {
        // Consider adding some general code to show an icon over whatever is being interacted with.
        return false; // You can't actually do anything with this...
    }
}
