using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool m_isOpen = false;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public override bool Interact(PlayerController interactor)
    {
        Debug.Log("Opening Door");
        if (m_isOpen)
        {
            this.GetComponent<Animator>().Play("Close");
        }
        else
        {
            this.GetComponent<Animator>().Play("Open");
        }
        m_isOpen = !m_isOpen;
        return true;
    }
}
