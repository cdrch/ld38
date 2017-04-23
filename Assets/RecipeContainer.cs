using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeContainer : MonoBehaviour {
    private Checkmark[] m_checkmarks;

    // Use this for initialization
    void Start ()
    {
        m_checkmarks = this.gameObject.GetComponentsInChildren<Checkmark>();
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
}
