using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
    public Dictionary<ItemType, ItemType[]> m_recipes;
    public ItemType m_currentRecipe;

    // Use this for initialization
    void Start()
    {
        m_recipes = new Dictionary<ItemType, ItemType[]>();
        m_recipes.Add(ItemType.HAMMER, new ItemType[] { ItemType.ROCK, ItemType.STICK, ItemType.WIRE });
        m_recipes.Add(ItemType.RAM, new ItemType[] { ItemType.WIRE, ItemType.LOG, ItemType.CART });
        m_recipes.Add(ItemType.DYNAMITE, new ItemType[] { ItemType.TUBE, ItemType.FUSE, ItemType.GUN_POWDER, ItemType.MATCH });

        m_currentRecipe = ItemType.HAMMER;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private ItemType[] GetRecipeListForItem(ItemType type)
    {
        return m_recipes[type];
    }

    public bool IsItemInRecipe(ItemType type)
    {
        foreach (ItemType curType in m_recipes[m_currentRecipe])
        {
            if (type == curType)
            {
                return true;
            }
        }
        return false;
    }
}
