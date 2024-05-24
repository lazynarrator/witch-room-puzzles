using UnityEngine;

/// <summary>
/// Итем в БД
/// </summary>
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string Name = "Item";
    //public Sprite Icon;
    public string Id;
}