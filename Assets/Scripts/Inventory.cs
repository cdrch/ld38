using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private bool m_isOpen = false;

    public Item[] m_items;
    public GameObject m_inventoryScreen;
    public GameObject[][] m_inventoryRows;

    // Use this for initialization
    void Start ()
    {
        m_items = new Item[16];
        m_inventoryScreen = this.gameObject.transform.GetChild(0).gameObject;
        if (m_inventoryScreen)
        {
            m_inventoryScreen.SetActive(m_isOpen);
            InitializeInventoryRows();
        }
        else
        {
            Debug.LogError("Couldn't find Inventory Object");
        }
    }

    private void InitializeInventoryRows()
    {
        m_inventoryRows = new GameObject[4][];
        for (int row = 0; row < 4; row++)
        {
            m_inventoryRows[row] = new GameObject[4];
            for (int col = 0; col < 4; col++)
            {
                m_inventoryRows[row][col] = m_inventoryScreen.transform.GetChild(row).GetChild(col).GetChild(0).gameObject;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isOpen)
        {

        }
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
            item.gameObject.SetActive(false);
            item.transform.parent = gameObject.transform;
            item.transform.localPosition = Vector3.zero;

            // "The item has been put in your inventory!"
            // TODO: Play some happy SFX
            Debug.Log(item.name + " has been put in your inventory!");
        }
        else
        {
            // "Your inventory is full!"
            // TODO: Play some sad SFX
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

    public void ToggleOpen()
    {
        m_isOpen = !m_isOpen;
        m_inventoryScreen.SetActive(m_isOpen);
        SetItemImages(m_items);
    }

    private void SetItemImages(Item[] items)
    {
        int row = 0;
        int col = 0;

        for (int idx = 0; idx < items.Length; idx++)
        {
            if (items[idx] != null)
            {
                m_inventoryRows[row][col].GetComponent<Image>().sprite = items[idx].GetSprite();

                col++;
                if (col == 4)
                {
                    row++;
                    col = 0;
                }
            }
            else
            {
                break;
            }
        }
    }

    public void SetItems(Item[] items)
    {

    }
}
