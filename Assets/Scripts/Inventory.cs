using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] m_items;
    private bool m_isOpen = false;
    public GameObject m_inventoryScreen;

    // Use this for initialization
    void Start ()
    {
        m_items = new Item[16];
        m_inventoryScreen = this.gameObject.transform.GetChild(0).gameObject;
        if (m_inventoryScreen)
        {
            m_inventoryScreen.SetActive(m_isOpen);
        }
        else
        {
            // Oops!
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isOpen)
        {

        }
	}

    public void ToggleOpen()
    {
        m_isOpen = !m_isOpen;
        m_inventoryScreen.SetActive(m_isOpen);
    }
}
