using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public Item[] m_items;

    // Use this for initialization
    void Start () {
        m_items = new Item[16];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddItemToInventory(Item item)
    {
        bool itemPlaced = false;
        for (int i = 0; i < m_items.Length; i++)
        {
            if (m_items[i] == null)
            {
                m_items[i] = item;
                itemPlaced = true;
                break;
            }
        }
        if (itemPlaced)
        {
            // "The item has been put in your inventory!"
            // Play some happy SFX
            Debug.Log(item.name + " has been put in your inventory!");
        }
        else
        {
            // "Your inventory is full!"
            // Play some sad SFX
            Debug.Log("Your inventory is too full to hold " + item.name + "!");
        }
    }

    public bool CheckInventoryForItem(Item item)
    {
        for (int i = 0; i < m_items.Length; i++)
        {
            if (m_items[i] == item)
            {
                return true;
            }
        }
        return false;
    }

    public Item RemoveItemFromInventory(Item item)
    {
        for (int i = 0; i < m_items.Length; i++)
        {
            if (m_items[i] == item)
            {
                item = m_items[i];
                m_items[i] = null;
                Debug.Log(item.name + " was taken from your inventory.");
                return item;
            }
        }
        Debug.Log(item.name + " is not in your inventory.");
        return null;
    }
}
