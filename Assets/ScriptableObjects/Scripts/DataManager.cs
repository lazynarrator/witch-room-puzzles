using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Точка входа для работы с условной БД
/// </summary>
[CreateAssetMenu(fileName = "DataManager", menuName = "Scriptable Objects/DataManager")]
public class DataManager : ScriptableObject
{
    private const string ManagerFilename = "DataManager";
    private static DataManager instance;
    
    /// <summary>
    /// Все доступные в БД итемы
    /// </summary>
    public List<Item> Items;
    
    /// <summary>
    /// Вернуть итем из БД по Id
    /// </summary>
    public Item GetItem(string _id) => Items.Find(i => i.Id == _id);
    
    public static DataManager GetDataManager()
    {
        if (!instance)
        {
            instance = Resources.Load<DataManager>(ManagerFilename);
        }

        return instance;
    }
}