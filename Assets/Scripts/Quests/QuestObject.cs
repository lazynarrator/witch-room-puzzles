using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Объект, с которым связан квест
/// </summary>
public class QuestObject : MonoBehaviour
{
    [SerializeField, Tooltip("Квесты, связанные с данным объектом")]
    [ValueDropdown("QuestList", ExpandAllMenuItems = true)]
    [OnValueChanged(nameof(BaseQuestList), InvokeOnInitialize = true)]
    public List<string> quests = new List<string>();
    
    /// <summary>
    /// Cписок квестов объекта
    /// </summary>
    [SerializeField, HideInInspector]
    public List<QuestManager.BaseQuest> baseQuests = new List<QuestManager.BaseQuest>();

    private void BaseQuestList()
    {
        baseQuests.Clear();
        List<QuestManager.BaseQuest> list = QuestManager.Instance.questsList;
            
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < quests.Count; j++)
            {
                if (list[i].name == quests[j])
                {
                    baseQuests.Add(list[i]);
                }
            }
        }
    }
    
    #region Editor
    
    /// <summary>
    /// Отобразить список всех квестов (в текстовом виде для просмотра в инспекторе)
    /// </summary>
    private IEnumerable QuestList()
    {
        ValueDropdownList<string> drop = new ValueDropdownList<string>();
        List<QuestManager.BaseQuest> list = QuestManager.Instance.questsList;
        
        foreach (var quest in list)
        {
            drop.Add(quest.name);
        }
        
        IEnumerable treeView = drop;
        return treeView;
    }
    
    #endregion Editor
}