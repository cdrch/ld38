using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
    private Dictionary<ItemType, ItemType[]> m_recipes;
    public ItemType[] m_recipeOrder = new ItemType[] { ItemType.HAMMER, ItemType.RAM, ItemType.DYNAMITE };
    public int m_currentRecipe = 0;
    private GameObject m_player;

    private RecipeContainer m_recipeContainer;

    public GameObject m_hammerPrefab;
    public GameObject m_ramPrefab;
    public GameObject m_dynamitePrefab;

    void Awake()
    {
        m_recipes = new Dictionary<ItemType, ItemType[]>();
        m_recipes.Add(ItemType.HAMMER, new ItemType[] { ItemType.ROCK, ItemType.STICK, ItemType.WIRE });
        m_recipes.Add(ItemType.RAM, new ItemType[] { ItemType.CART, ItemType.LOG, ItemType.WIRE });
        m_recipes.Add(ItemType.DYNAMITE, new ItemType[] { ItemType.TUBE, ItemType.FUSE, ItemType.GUN_POWDER, ItemType.MATCH });

        Transform inventoryScreen = this.gameObject.transform.GetChild(0);
        int lastChild = inventoryScreen.childCount - 1;
        m_recipeContainer = inventoryScreen.GetChild(lastChild).gameObject.GetComponent<RecipeContainer>();
        m_player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    // Use this for initialization
    void Start()
    {
        if (!m_recipeContainer)
        {
            Debug.Log("Couldn't find RecipeContainer!");
        }
        else
        {
            Debug.Log("Setting New Recipe");
            SetNewRecipe();
        }
        
        if (!m_player)
        {
            Debug.Log("Couldn't find Player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetNewRecipe()
    {
        Debug.Log("Setting new recipe: " + m_recipeOrder[m_currentRecipe]);
        m_recipeContainer.SetNewRecipe(m_recipeOrder[m_currentRecipe], GetCurrentRecipeList());
    }

    public ItemType[] GetCurrentRecipeList()
    {
        if (m_currentRecipe >= m_recipeOrder.Length)
        {
            m_recipeContainer.gameObject.SetActive(false);
            return null;
        }
        return m_recipes[m_recipeOrder[m_currentRecipe]];
    }

    private GameObject CraftCurrentRecipe(GameObject prefab)
    {
        m_currentRecipe++;
        if (GetCurrentRecipeList() != null)
        {
            SetNewRecipe();
        }
        return Instantiate(prefab, m_player.transform.position, Quaternion.identity);
    }

    public GameObject CraftCurrentRecipe()
    {
        switch(m_recipeOrder[m_currentRecipe])
        {
            case ItemType.HAMMER:
                return CraftCurrentRecipe(m_hammerPrefab);
            case ItemType.RAM:
                return CraftCurrentRecipe(m_ramPrefab);
            case ItemType.DYNAMITE:
                return CraftCurrentRecipe(m_dynamitePrefab);
            default:
                return null;
        }
    }

    public bool IsItemInRecipe(ItemType type)
    {
        ItemType[] recipe = GetCurrentRecipeList();
        if (recipe == null)
        {
            return false;
        }
        foreach (ItemType curType in GetCurrentRecipeList())
        {
            if (type == curType)
            {
                return true;
            }
        }
        return false;
    }

    public void ResetAllChecks()
    {
        m_recipeContainer.ResetAllChecks();
    }

    public void SetCheckForItemType(ItemType type)
    {
        m_recipeContainer.SetCheckForItemType(type);
    }

    public void EnableCraftButtonIfAble()
    {
        m_recipeContainer.EnableCraftButtonIfAble();
    }
}
