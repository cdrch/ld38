﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public bool m_isOpen = false;
    private bool m_isMoving = false;

    public float m_closedRotation; // 0
    public float m_openedRotation; // 1
    private float m_rotationTime;

	// Use this for initialization
	void Start ()
    {
        if (m_isOpen)
        {
            m_openedRotation = this.gameObject.transform.localRotation.z;
        }
        else
        {
            m_closedRotation = this.gameObject.transform.localRotation.z;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isMoving)
        {
            m_rotationTime += (m_isOpen ? -1 : 1) * Time.deltaTime;
            if (m_rotationTime > 1f || m_rotationTime < 0f)
            {
                m_isMoving = false;
            }
            float currentRotationAngle = Mathf.Lerp(m_closedRotation, m_openedRotation, m_rotationTime);
            Debug.Log(currentRotationAngle);
            Quaternion currentRotation = this.gameObject.transform.localRotation;
            this.gameObject.transform.localRotation.Set(currentRotation.x, currentRotation.y, currentRotationAngle, currentRotation.w);
        }
	}

    public override bool Interact(PlayerController interactor)
    {
        if (!m_isMoving)
        {
            Debug.Log("Opening door");
            m_isOpen = !m_isOpen;
            m_rotationTime = m_isOpen ? 1f : 0f;
            m_isMoving = true;
            return true;
        }
        return false;
    }
}
