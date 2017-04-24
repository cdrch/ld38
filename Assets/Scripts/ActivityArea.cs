using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class ActivityArea : Interactable {
    private Inventory m_playerInventory;

    public ItemType[] m_itemTypesNeeded = new ItemType[3] { ItemType.HAMMER, ItemType.RAM, ItemType.DYNAMITE };
    public int m_currentItemTypeNeeded = 0;

    // Use this for initialization
    void Start ()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        m_playerInventory = player.GetComponent<Inventory>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override bool Interact(PlayerController interacter)
    {
        if (m_currentItemTypeNeeded == 3)
        {
            return false;
        }

        if (m_playerInventory.CheckInventoryForItemType(m_itemTypesNeeded[m_currentItemTypeNeeded]))
        {
            m_currentItemTypeNeeded++;
            Debug.Log("Using Item");
            return true;
        }
        Debug.Log("Don't have the right item!");
        return false;
    }
}
