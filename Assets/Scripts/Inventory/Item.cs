﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {
    public Sprite m_GUISprite;
    public ItemType m_itemType;
    public bool m_shouldHighlight;

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public ItemType GetItemType()
    {
        return m_itemType;
    }

    public Item TryToPickUp()
    {
        // Put any possible conditions that might prevent pickup here.
        return this;
    }

    public override bool Interact(PlayerController interacter)
    {
        Item i = TryToPickUp();
        if (i != null)
        {
            interacter.GetComponent<Inventory>().AddItemToInventory(i);
            return true;
        }
        else // If something prevented pick up
        {
            return false;
        }        
    }

    public Sprite GetSprite()
    {
        return m_GUISprite;
    }
}