using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool m_isOpen = false;

    private float timeUntilDoorClose;
    private Collider m_collider;

	// Use this for initialization
	void Start ()
    {
        timeUntilDoorClose = 5f;
        m_collider = GetComponentInChildren<Collider>();
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
            StartCoroutine(CloseDoorAfterWait(timeUntilDoorClose));
        }
        m_isOpen = !m_isOpen;
        return true;
    }

    private IEnumerator CloseDoorAfterWait(float secondsToWait)
    {
        m_collider.enabled = false;
        yield return new WaitForSeconds(secondsToWait);
        this.GetComponent<Animator>().Play("Close");
        m_isOpen = false;
        m_collider.enabled = true;
    }
}
