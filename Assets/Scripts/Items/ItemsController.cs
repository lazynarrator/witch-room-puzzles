using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Взаимодействие с итемами
/// </summary>
public class ItemsController : MonoBehaviour
{
    private List<Item> items; //Список доступных итемов
    private static ItemsController instance;
    public static ItemsController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ItemsController>();
            return instance;
        }
    }
    
    private void OnValidate()
    {
        var data = DataManager.GetDataManager();
        items = data.Items;
    }
    
    /// <summary>
    /// Все доступные итемы
    /// </summary>
    /// <returns>Массив доступных итемов</returns>
    public Item[] GetItemsList()
    {
        Item[] itemList = new Item[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            itemList[i] = items[i];
        }

        return itemList;
    }
}