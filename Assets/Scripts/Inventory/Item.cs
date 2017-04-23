using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Item : Interactable {
    public Sprite m_GUISprite;
    public ItemType m_itemType;
    public bool m_shouldHighlight;

    public AudioClip m_pickUp;
    public AudioClip m_failPickUp;
    private AudioSource m_audio;

    // Use this for initialization
    void Start ()
    {
        m_audio = GetComponent<AudioSource>();
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
            bool successfullyPickedUp = interacter.GetComponent<Inventory>().AddItemToInventory(i);
            m_audio.PlayOneShot(successfullyPickedUp ? m_pickUp : m_failPickUp, 1f);
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
