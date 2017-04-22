using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	void Start ()
	{
		
	}

    public virtual bool Interact(PlayerController interacter)
    {
        // Consider adding some general code to show an icon over whatever is being interacted with.
        return false; // You can't actually do anything with this...
    }
}
