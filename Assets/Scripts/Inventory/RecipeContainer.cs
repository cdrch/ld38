using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeContainer : MonoBehaviour {
    private Checkmark[] m_checkmarks;
    private CraftButton m_craftButton;

    // Use this for initialization
    void Start ()
    {
        m_checkmarks = this.gameObject.GetComponentsInChildren<Checkmark>();
        m_craftButton = this.gameObject.GetComponentInChildren<CraftButton>();

        if (!m_craftButton)
        {
            Debug.Log("Couldn't find craft button!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ResetAllChecks()
    {
        foreach (Checkmark check in m_checkmarks)
        {
            check.Uncheck();
        }
    }

    public void SetCheckForItemType(ItemType type)
    {
        foreach (Checkmark check in m_checkmarks)
        {
            if (check.GetItemType() == type)
            {
                check.Check();
                break;
            }
        }
    }

    private bool AreAllItemsChecked()
    {
        foreach (Checkmark check in m_checkmarks)
        {
            if (!check.IsChecked())
            {
                return false;
            }
        }
        return true;
    }

    public void EnableCraftButtonIfAble()
    {
        if (AreAllItemsChecked())
        {
            m_craftButton.Enable();
        }
        else
        {
            m_craftButton.Disable();
        }
    }
}
