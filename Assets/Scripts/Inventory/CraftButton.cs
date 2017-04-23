using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour {
    public Color m_enabledColor;
    public Color m_disabledColor;

    private bool m_isEnabled;
    private Inventory m_playerInventory;

	// Use this for initialization
	void Start ()
    {
        m_playerInventory = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Inventory>();
        if (!m_playerInventory)
        {
            Debug.Log("Couldn't find player Inventory!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_isEnabled && m_playerInventory.IsOpen() && Input.GetButtonDown("Interact"))
        {
            m_playerInventory.CraftRecipe();
        }
	}

    public bool IsEnabled()
    {
        return m_isEnabled;
    }

    public void Enable()
    {
        m_isEnabled = true;
        this.gameObject.GetComponent<Image>().color = m_enabledColor;
    }

    public void Disable()
    {
        m_isEnabled = false;
        this.gameObject.GetComponent<Image>().color = m_disabledColor;
    }
}
