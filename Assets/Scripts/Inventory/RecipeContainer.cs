using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeContainer : MonoBehaviour {
    private Checkmark[] m_checkmarks;
    private CraftButton m_craftButton;

    public GameObject m_checkmarkPrefab;
    public Sprite m_hammerRecipe;
    public Sprite m_ramRecipe;
    public Sprite m_dynamiteRecipe;
    private Dictionary<ItemType, Sprite> m_recipeSprites;

    private float[] m_threeChecks = new float[3] { -5, -35, -64 };
    private float[] m_fourChecks = new float[4] { 5, -19, -44, -69 };

    // Use this for initialization
    void Start ()
    {
        m_checkmarks = new Checkmark[4];
        m_craftButton = this.gameObject.GetComponentInChildren<CraftButton>();

        m_recipeSprites = new Dictionary<ItemType, Sprite>();
        m_recipeSprites.Add(ItemType.HAMMER, m_hammerRecipe);
        m_recipeSprites.Add(ItemType.RAM, m_ramRecipe);
        m_recipeSprites.Add(ItemType.DYNAMITE, m_dynamiteRecipe);

        if (!m_craftButton)
        {
            Debug.Log("Couldn't find craft button!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void DestroyCurrentChecks()
    {
        foreach(Checkmark check in m_checkmarks)
        {
            if (check)
            {
                Destroy(check.gameObject);
            }
        }
    }

    private void SpawnChecks(ItemType[] itemTypes, float[] offsets)
    {
        int index = 0;
        List<Checkmark> checks = new List<Checkmark>();
        foreach (ItemType type in itemTypes)
        {
            GameObject check = Instantiate(m_checkmarkPrefab, this.transform);
            Vector3 newPos = this.transform.position + new Vector3(-68f, offsets[index++], 0f);
            check.transform.SetPositionAndRotation(newPos, Quaternion.identity);
            check.GetComponent<Checkmark>().SetItemType(type);
            check.GetComponent<Checkmark>().Init();
            checks.Add(check.GetComponent<Checkmark>());
        }
        m_checkmarks = checks.ToArray();
    }

    public void SetNewRecipe(ItemType recipe, ItemType[] itemTypes)
    {
        DestroyCurrentChecks();
        SpawnChecks(itemTypes, itemTypes.Length == 3 ? m_threeChecks : m_fourChecks);
        GetComponent<Image>().sprite = m_recipeSprites[recipe];
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
