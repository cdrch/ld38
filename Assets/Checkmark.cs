using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkmark : MonoBehaviour {

    private bool m_isChecked;
    private GameObject m_checkImage;

    // What type of item this is, for CraftingManager's use
    public ItemType m_itemType;

	// Use this for initialization
	void Start ()
    {
        m_checkImage = this.gameObject.transform.GetChild(0).gameObject;

        if (!m_checkImage)
        {
            Debug.Log("Could not find check image!");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public ItemType GetItemType()
    {
        return m_itemType;
    }

    private void SetCheckImageVisibility(bool visible)
    {
        m_checkImage.SetActive(visible);
    }

    public void Check()
    {
        m_isChecked = true;
        SetCheckImageVisibility(m_isChecked);
    }

    public void Uncheck()
    {
        m_isChecked = false;
        SetCheckImageVisibility(m_isChecked);
    }
}
