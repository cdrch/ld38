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

    public CraftingManager m_craftingManger;

    public Color m_normalItemColor;
    public Color m_tintedItemColor;

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
        m_craftingManger = this.gameObject.GetComponent<CraftingManager>();
    }

    private void InitializeInventoryRows()
    {
        m_inventoryRows = new GameObject[4][];
        for (int row = 0; row < 4; row++)
        {
            m_inventoryRows[row] = new GameObject[4];
            for (int col = 0; col < 4; col++)
            {
                m_inventoryRows[row][col] = m_inventoryScreen.transform.GetChild(row).GetChild(col).gameObject;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isOpen)
        {
            // TODO: Handle input here
        }
	}

    public bool IsOpen()
    {
        return m_isOpen;
    }
    
    public bool AddItemToInventory(Item item)
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

            Debug.Log(item.name + " has been put in your inventory!");
            return true;
        }
        else
        {
            Debug.Log("Your inventory is too full to hold " + item.name + "!");
            return false;
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
        if (m_isOpen)
        {
            SetItemImages(m_items);
            UpdateCraftingChecklist(m_items);
        }
    }

    private void SetSlotImageAndColor(ref GameObject slot, Item item)
    {
        slot.transform.GetChild(0).GetComponent<Image>().sprite = item.GetSprite();

        bool itemInRecipe = m_craftingManger.IsItemInRecipe(item.GetItemType());
        slot.GetComponent<Image>().color = itemInRecipe ? m_tintedItemColor : m_normalItemColor;
    }

    private void ResetSlotImageAndColor(ref GameObject slot)
    {
        slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
        slot.GetComponent<Image>().color = m_normalItemColor;
    }

    private void UpdateCraftingChecklist(Item[] items)
    {
        m_craftingManger.ResetAllChecks();
        foreach (Item item in items)
        {
            if (item)
            {
                m_craftingManger.SetCheckForItemType(item.GetItemType());
            }
        }
        m_craftingManger.EnableCraftButtonIfAble();
    }

    private void SetItemImages(Item[] items)
    {
        int row = 0;
        int col = 0;

        for (int idx = 0; idx < items.Length; idx++)
        {
            if (items[idx] != null)
            {
                SetSlotImageAndColor(ref m_inventoryRows[row][col], items[idx]);
            }
            else
            {
                ResetSlotImageAndColor(ref m_inventoryRows[row][col]);
            }

            col++;
            if (col == 4)
            {
                row++;
                col = 0;
            }
        }
    }

    public void SetItems(Item[] items)
    {

    }

    public void CraftRecipe()
    {
        ItemType[] recipe = m_craftingManger.GetCurrentRecipeList();

        foreach (ItemType type in recipe)
        {
            for (int i = 0; i < m_items.Length; i++)
            {
                if (m_items[i] && m_items[i].GetItemType() == type)
                {
                    Debug.Log(m_items[i].name + " was taken from your inventory.");
                    m_items[i] = null;
                    break;
                }
            }
        }
        
        GameObject craftedItem = m_craftingManger.CraftCurrentRecipe();
        AddItemToInventory(craftedItem.GetComponent<Item>());
        SetItemImages(m_items);

        UpdateCraftingChecklist(m_items);
    }
}
