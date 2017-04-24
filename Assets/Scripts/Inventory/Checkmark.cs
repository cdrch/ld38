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
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Init()
    {
        if (!m_checkImage)
        {
            m_checkImage = this.gameObject.transform.GetChild(0).gameObject;
            if (!m_checkImage)
            {
                Debug.Log("Could not find check image!");
            }
        }
    }

    public bool IsChecked()
    {
        return m_isChecked;
    }

    public ItemType GetItemType()
    {
        return m_itemType;
    }

    public void SetItemType(ItemType type)
    {
        m_itemType = type;
    }

    private void SetCheckImageVisibility(bool visible)
    {
        m_checkImage.SetActive(visible);
    }

    public void Check()
    {
        Debug.Log("Checked");
        m_isChecked = true;
        SetCheckImageVisibility(m_isChecked);
    }

    public void Uncheck()
    {
        m_isChecked = false;
        SetCheckImageVisibility(m_isChecked);
    }
}
